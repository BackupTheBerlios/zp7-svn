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
import browse.hooks as hooks
import interop
import autodistrib
import logpagepreview
import locale

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
  
  def xmlsave(self,xml):
    xml.name='song'
    utils.xmlsavedict(xml,self.vals,('title','author','group'))
    xml.add('text').text=self.vals['text']
    if self.src: xml.attrs['source']=";".join(map(str,self.src))

  def xmlload(self,xml):
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
  def __unicode__(self): return u'Obsah'

  def xmlsave(self,xml):
    xml.name='content'

  def xmlload(self,xml):
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
      


sb_song_line_classes={
  'song':SBSong,
  'content':Content
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

  def load(self,fr):
    xml=xmlnode.XmlNode.load(fr)
    for x in xml/'songs':
      song=sb_song_line_classes[x.name]()
      song.xmlload(x)
      self.songs.append(song)
      id=song.dbidtuple()
      if id: self.dbsongs[id]=song
    self.sbtype.xmlload(xml/'sbtype')
    
  def save(self,fw):
    xml=xmlnode.XmlNode('songbook')
    utils.xmlsavearray(self.songs,xml.add('songs'),'song')
    self.sbtype.xmlsave(xml/'sbtype')
    xml.save(fw)
    
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
      autodistrib.PaneGrp(self.formatted[id(song)].panegrp.panes) 
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