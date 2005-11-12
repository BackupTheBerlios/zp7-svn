# -*- coding: UTF-8 -*-

import utils.xmlnode as xmlnode
import utils
import os.path
import sbtype
import format
import printing
import a4distrib
import wx
import paging
import browse
import browse.hooks as hooks
import interop
import distribalg
import logpagepreview
import locale
import zipfile
from cStringIO import StringIO

class _FmtSong:
  panegrp=None
  song=None
  def __init__(self,song,panegrp):
    self.song=song
    self.panegrp=panegrp

class SBSongLikeItem(object):
  def dbidtuple(self): pass
  def format(self,dc,pars,sb): return format.PaneGrp()
  def content_title(self): return u''
  def xmlsave(self,xml,zipfw): raise NotImplemented
  def xmlload(self,xml,zipfr): raise NotImplemented
  def define_advanced_brw(self,brw): pass
  def reformat(self):
    import sbpanel
    sbpanel.reformat_songlike_item(self)

class SBSong(SBSongLikeItem):
  src=None #(str(database),int(songid))
  attrnames=('title','author','group','text')
  vals={}
  
  def __init__(self,vals=None,src=None):
    if vals: self.vals=vals
    else: self.vals={}
    if src: self.src=src
    
  @staticmethod
  def from_db(db,songid):
    song=db[songid]
    return SBSong(song.vals,song.dbidtuple())

  def __getattr__(self,name): 
    if (not name.startswith('__')) and (not name.endswith('__')) and self.vals.has_key(name):
      return self.vals[name]
    return object.__getattr__(self,name)

  def dbidtuple(self):
    return self.src

  def __unicode__(self): return unicode(self.title)
  
  def xmlsave(self,xml,zipfw):
    xml.name='song'
    utils.xmlsavedict(xml,self.vals,('title','author','group'))
    xml.add('text').text=self.vals['text']
    if self.src: xml.attrs['source']=";".join(map(str,self.src))

  def xmlload(self,xml,zipfr):
    utils.xmlloaddict(xml,self.vals,('title','author','group'))
    self.vals['text']=(xml/'text').text
    if 'source' in xml.attrs: 
      db,id=xml.attrs['source'].split(';')
      self.src=(db,int(id))

  def format(self,dc,pars,sb):
    fmt=format.SongFormatter(dc,pars,self.text,sb.rbt.pgwi)
    sb.sbtype.header.printheader(self,fmt.panegrp,sb.rbt)
    fmt.run()
    sb.sbtype.songdelimiter.printdelimiter(fmt.panegrp,sb.rbt)
    return fmt.panegrp

  def content_title(self): return self.title


class _FindSongPage:
  def __init__(self,sb,song):
    self.sb=sb
    self.song=song
  def __call__(self): return self.sb.findpage(self.song).pagenum

class Content(SBSongLikeItem):
  def __unicode__(self): return u'[Obsah]'

  def xmlsave(self,xml,zipfw):
    xml.name='content'

  def xmlload(self,xml,zipfr):
    pass

  def format(self,dc,pars,sb):
    songs=[s for s in sb.songs if s.content_title()]
    songs.sort(locale.strcoll,lambda s:s.content_title().upper())
    actcol=sb.sbtype.content_cols
    acty=0
    colwi=sb.rbt.pgwi/sb.sbtype.content_cols
    panegrp=format.PaneGrp()
    dc.SetFont(sb.rbt.getfont('content').getwxfont())
    lw,lh=dc.GetTextExtent('M')
    for s in songs:
      if acty>sb.rbt.pghi:
        actcol+=1
        acty=0
        
      if actcol>=sb.sbtype.content_cols:
        pane=panegrp.addpane()
        pane.hi=sb.rbt.pghi
        canvas=pane.canvas
        canvas.font(sb.rbt.getfont('content'))
        actcol=0
        acty0=0
      
      canvas.text(actcol*colwi,acty,s.content_title())
      canvas.dynamic_text(actcol*colwi+colwi-3*lw,acty,lambda sb=sb,s=s:sb.findpage(s).pagenum)
      acty+=lh
    return panegrp

class Image(SBSongLikeItem):
  data=''
  img=None
  ext=''
  imgwi=100
  imghi=100
  preserveratio=True
  def __unicode__(self): return u'[Obrázek]'
  
  def xmlsave(self,xml,zipfw):
    xml.name='image'
    xml['id']=id(self)
    xml['ext']=self.ext
    filename='images/%d.%s'%(id(self),self.ext)
    zipfw.writestr(str(filename),self.data)
    xml['imgwi']=self.imgwi
    xml['imghi']=self.imghi
    xml['preserveratio']=int(self.preserveratio)

  def xmlload(self,xml,zipfr):
    imgid=xml['id']
    ext=xml['ext']
    filename='images/%s.%s'%(imgid,ext)
    data=zipfr.read(str(filename))
    self.setdata(data,ext)
    self.imgwi=int(xml.attrs.get('imgwi',100))
    self.imghi=int(xml.attrs.get('imghi',100))
    self.preserveratio=bool(int(xml.attrs.get('preserveratio',1)))

  def setdata(self,data,ext):
    self.ext=ext
    self.data=data
    self.image=wx.ImageFromStream(StringIO(data))

  def format(self,dc,pars,sb):
    panegrp=format.PaneGrp()
    wi=self.imgwi*sb.rbt.pgwi/100
    hi=self.imghi*sb.rbt.pghi/100
    if self.preserveratio:
      k=float(self.image.GetHeight())/float(self.image.GetWidth())
      w1=hi/k;h1=hi
      w2=wi;h2=wi*k
      wi=int(min(w1,w2))
      hi=int(min(h1,h2))
    pane=panegrp.addpane()
    pane.hi=hi
    pane.canvas.stretchimage(self.image,sb.rbt.pgwi/2-wi/2,0,wi,hi)
    return panegrp

  def define_advanced_brw(self,brw):
    brw.check(text=u'Zachovat poměr šířka/výška',model=browse.attr(self,'preserveratio'),autosave=True,event=self.reformat)
    brw.label(text=u'Šířka (% z šířky stránky)')
    brw.spin(model=browse.attr(self,'imgwi'),autosave=True,event=self.reformat)
    brw.label(text=u'Výška (% z výšky stránky)')
    brw.spin(model=browse.attr(self,'imghi'),autosave=True,event=self.reformat)
  
  def reformat(self,ev):
    SBSongLikeItem.reformat(self)

sb_song_line_classes={
  'song':SBSong,
  'content':Content,
  'image':Image
}

class SongBook(object,hooks.Hookable):
  songs=[]
  formatted={} #id(song):formatted
  dbsongs={} #dict((db,songid):SBSong)
  filename=''
  sbtype=None
  rbt=None
  logpages=None
  
  def __init__(self):
    self.songs=hooks.HookableList()
    self.songs.hook(self)
    self.dbsongs={}
    #self.clearhooks()
    self.sbtype=sbtype.SBType()
    self.formatted={}
  
  def __contains__(self,song):
    """@type song: (db,songid)"""
    return self.dbsongs.has_key(song)

  def load(self,filename):
    fr=zipfile.ZipFile(filename,'r')
    
    xml=xmlnode.XmlNode.fromstr(fr.read('songs.xml'))
    for x in xml:
      song=sb_song_line_classes[x.name]()
      song.xmlload(x,fr)
      self.songs.append(song)
      id=song.dbidtuple()
      if id: self.dbsongs[id]=song
      
    xml=xmlnode.XmlNode.fromstr(fr.read('sbtype.xml'))
    self.sbtype.xmlload(xml)
    fr.close()
  
  def save(self,filename):
    fw=zipfile.ZipFile(filename,'w',zipfile.ZIP_DEFLATED)
    
    xml=xmlnode.XmlNode('songs')
    for song in self.songs: song.xmlsave(xml.add('song'),fw)
    fw.writestr('songs.xml',xml.tostr())
    
    xml=xmlnode.XmlNode('sbtype')
    self.sbtype.xmlsave(xml)
    fw.writestr('sbtype.xml',xml.tostr())
    
    fw.close()
    
  def add_dbsong(self,db,songid):
    res=SBSong.from_db(db,songid)
    self.songs.append(res)
    
  def remove_dbsong(self,db,songid):
    dbsong=db[songid]
    songid=dbsong.dbidtuple()
    if self.dbsongs.has_key(songid):
      self.songs.remove(self.dbsongs[songid])

  def __unicode__(self):
    if self.filename: return os.path.basename(self.filename)
    return u'Beze jména'

  def hassong(self,db,songid):
    return int(db[songid].dbidtuple() in self)
    
  def setsongcontain(self,db,songid,contain):
    id=db[songid].dbidtuple()
    if (id in self) and (not contain): self.remove_dbsong(db,songid)
    if (id not in self) and (contain): self.add_dbsong(db,songid)

  def clearformat(self):
    self.formatted.clear()
    self.rbt=None
    self.cleardistrib()
    
  def cleardistrib(self):
    self.a4d=None
    self.logpages=None
    
  def _formatsong(self,song,dc,pars):
    panegrp=song.format(dc,pars,self)
    if not panegrp: panegrp=format.PaneGrp()
    self.formatted[id(song)]=_FmtSong(song,panegrp)
    #self.formatted[id(song)]=_FmtSong(song,fmt.panegrp)
    
  def wantrbt(self):
    if self.rbt: return
    self.rbt=self.sbtype.getreal(wx.PrinterDC(printing.printData))
    
  def _print_header_footer(self):
    hdrfont=self.rbt.getfont('header')
    footfont=self.rbt.getfont('footer')
    for page in self.logpages:
      hdr=self.sbtype.header_text.replace(u'%c',unicode(page.pagenum))
      foot=self.sbtype.footer_text.replace(u'%c',unicode(page.pagenum))
      if hdr:
        self.rbt.dc.SetFont(hdrfont.getwxfont())
        page.canvas.font(hdrfont)
        w,h=self.rbt.dc.GetTextExtent(hdr)
        page.canvas.text(self.rbt.pgwi/2-w/2,-h,hdr)
      if foot:
        self.rbt.dc.SetFont(footfont.getwxfont())
        page.canvas.font(footfont)
        w,h=self.rbt.dc.GetTextExtent(foot)
        page.canvas.text(self.rbt.pgwi/2-w/2,self.rbt.pghi,foot)
    
  def format(self):
    self.cleardistrib()
    
    self.wantrbt()
    for song in self.songs:
      if not self.formatted.has_key(id(song)):
        self._formatsong(song,self.rbt.dc,self.rbt.pars)
    self.logpages=paging.LogPages((self.rbt.pgwi,self.rbt.pghi))
    panegrps=[
      distribalg.PaneGrp(self.formatted[id(song)].panegrp.panes) 
      for song in self.songs
    ]
    alg=self.sbtype.distribalg.creator(self.logpages,panegrps,self.sbtype)
    alg.run()
    alg.printpages()
    
    self._print_header_footer()
      
    if len(self.logpages.pages)==0: return
    self.a4d=a4distrib.A4Distribution(self.sbtype.hcnt,self.sbtype.vcnt,self.logpages.pages,self.sbtype.a4distribtype)

  def drawpage(self,canvas,pgnum):
    if not self.a4d: return
    for i in xrange(self.sbtype.vcnt):
      for j in xrange(self.sbtype.hcnt):
        page=self.a4d.getpage(pgnum/2,j,i,pgnum%2)
        ofsx,ofsy=self.rbt.pageofs(j,i)
        if page: 
          page.draw(format.SubCanvas(canvas,ofsx,ofsy))
        
  def wantformat(self):
    interop.send_flag('format_songbook')
        
  def onappend(self,song): 
    songid=song.dbidtuple()
    if songid: self.dbsongs[songid]=song
    self.wantformat()
    
  def ondelitem(self,key):
    song=self.songs[key]
    self.onremove(song)
    self.wantformat()
    
  def onsetitem(self,key,value):
    self.wantformat()
    
  def onremove(self,song):
    songid=song.dbidtuple()
    if songid and self.dbsongs.has_key(songid): del self.dbsongs[songid]
    if self.formatted.has_key(id(song)): del self.formatted[id(song)]
    self.wantformat()
    
  def logpagepreview(self):
    if self.sbtype.hcnt % 2: return logpagepreview.SimpleLogPagePreview(self)
    return logpagepreview.BookLogPagePreview(self)

  def insert_content(self):
    self.songs.insert(0,Content())

  def insert_image(self,data,ext):
    img=Image()
    img.setdata(data,ext)
    self.songs.insert(0,img)

  def findpage(self,song):
    """
    @type: L{format.LogPage}
    """
    try:
      for page in self.logpages:
        if self.formatted[id(song)].panegrp[0] in page.panes:
          return page
    except:
      pass