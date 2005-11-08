# -*- coding: utf-8 -*- 

import desktop
import desktop.intf
import wx
import wx.grid as gridlib
import utils
import wx.lib.layoutf  as layoutf
import anchors.content
import songbook
import sbtype
import dbview
import wx.lib.buttons as buttons
import browse
import os
import config
import format
import interop

class Preview:
  pagecount=0
  curpage=-1
  infobrw=None
  _zooms=(10,20,50,80,100,200,500)
  curzoom=3
  toolbar=None
  panel=None
  drawpage=None
  formatpgnum=None
  virtw=0
  virth=0
  
  def __init__(self,panel,drawpage=lambda canvas,page:0,formatpgnum=lambda pagenum,pagecount:u''):
    self.panel=panel
    self.drawpage=drawpage
    self.formatpgnum=formatpgnum

  def define_brw(self,brw,title):
    brw.page(text=title)
    brw.vbox()
    self.toolbar=brw.toolbar()
    self.preview=brw.scrollwin(onpaint=self.OnPaint,proportion=1,color='white').ctrl
    brw.endsizer()
    brw.endparent()
    self.define_bar()
  
  def OnPaint(self,event):
    if not self.panel.actsb:
      event.Skip()
      return
    zoom=self._zooms[self.curzoom]/100.0
    dc=wx.PaintDC(self.preview)
    self.preview.PrepareDC(dc)
    dc.SetUserScale(self.panel.actsb.rbt.pkoefx*zoom,self.panel.actsb.rbt.pkoefy*zoom)
    self.drawpage(format.DCCanvas(dc),self.curpage)
  
  def define_bar(self):
    self.toolbar.label(model=lambda: self.formatpgnum(self.curpage,self.pagecount),layoutflags=wx.CENTER,style=wx.ST_NO_AUTORESIZE,size=(100,-1))
    self.toolbar.space((10,10))
    self.toolbar.button(text='<<',event=lambda ev:self.changepage(-1))
    self.toolbar.button(text='>>',event=lambda ev:self.changepage(1))
    self.toolbar.combo(id='zoom',model=['%d%%'%z for z in self._zooms],curmodel=browse.attr(self,'curzoom'),event=self.changezoom)
    self.toolbar.realize()
    
  def setvirtsize(self,w,h):
    self.virtw=w
    self.virth=h
    self.updatevirtsize()

  def updatevirtsize(self):
    if not self.panel.actsb:
      self.preview.SetVirtualSize((0,0))
      return
    zoom=self._zooms[self.curzoom]/100.0
    self.preview.SetVirtualSize((self.virtw*self.panel.actsb.rbt.pkoefx*zoom,self.virth*self.panel.actsb.rbt.pkoefy*zoom))

  def update(self,pagecount):
    self.pagecount=pagecount
    self.changepage()

  def changepage(self,d=0):
    self.curpage+=d
    if self.curpage<0: self.curpage=0
    if self.curpage>=self.pagecount: self.curpage=self.pagecount-1
    self.refresh()
    
  def refresh(self):
    self.preview.Refresh()
    self.toolbar.loadall()

  def changezoom(self,ev=None):
    self.toolbar['zoom'].save()
    self.updatevirtsize()
    self.refresh()
    
# u"Strana %d/%d"%(self.curpage+1,self.pagecount)
 
class SBPanel(anchors.content.IContent):
  #songv=None
  #panel=None
  content_visible=False
  sbs=None
  predefined_sb_list=[u"- Vyberte zpěvník",u"- Nový",u"- Načíst"]
  ignore_change_sb=False
  opensbs=[]
  actsb=None
  alloccol=None
  brw=None
  preview=None
  logpreview=None
  lpp=None # type=LogPagePreview
  
  #pagecount=0
  #curpage=-1
  #infobrw=None
  #_zooms=(10,20,50,80,100,200,500)
  #curzoom=3
  #ptoolbar=None

  def __init__(self):
    desktop.register_menu(self.create_toolbar)
    desktop.register_menu(self.create_menu)
    self.opensbs=[]
    interop.define_flag('format_songbook',self.format_songbook)
  
  def on_create_control(self,parent,evtbinder):
    #self.box1=wx.BoxSizer(wx.HORIZONTAL)
    #self.panel=wx.Window(parent,-1)
    #self.notebook=wx.Notebook(parent,-1)

    self.brw=browse.Browse(parent)
    #self.brw=browse.Browse(self.panel)

    #self.notebook.SetConstraints(layoutf.Layoutf('t=t#1;l!200;r=r#1;b=b#1',(parent,)))
    #self.panel.SetConstraints(layoutf.Layoutf('t=t#1;l=l#1;r!200;b=b#1',(parent,)))
    #self.panel.Bind(wx.EVT_SIZE,lambda ev:self.panel.Layout(),None)

    self.brw.hbox()
    
    #self.brw.vbox(border=10,layoutflags=wx.FIXED_MINSIZE,proportion=0)
    self.brw.vbox(border=10)
    #self.brw.hbox(border=2,layoutflags=wx.CENTER)
    self.brw.hbox(border=2)
    self.brw.label(text=u'Typ zpěvníku:')
    self.brw.combo(model=sbtype.sbtypes,id='sbtype',size=(100,-1),event=self.onchangebasesbtype)
    self.brw.endsizer()
    self.brw.listbox(proportion=1,id='songlist',model=[])
    self.brw.button(text=u'Pokročilé nastavení')
    self.brw.check(text=u'Začít na nové stránce')
    self.brw.grid(rows=2,cols=2,vgap=5,hgap=5)
    self.brw.button(text=u'Dopředu',event=lambda ev:self.brw['songlist'].moveup())
    self.brw.button(text=u'Dozadu',event=lambda ev:self.brw['songlist'].movedown())
    self.brw.button(text=u'Vymazat',event=lambda ev:self.brw['songlist'].eraseact())
    self.brw.endsizer()
    self.infobrw=self.brw.panelbrw()
    self.brw.endsizer()
    
    self.brw.pager(proportion=1)
    self.brw.page(text=u"Text písně")
    self.brw.endparent()

    self.preview=Preview(
      self,
      lambda canvas,page:self.actsb.drawpage(canvas,page),
      lambda curpage,pagecount:u"Strana %d/%d"%(curpage+1,pagecount)
    )
    self.preview.define_brw(self.brw,u"Náhled")
    
    self.logpreview=Preview(
      self,
      lambda canvas,page:self.lpp.drawpage(page,canvas),
      lambda curpage,pagecount:self.lpp.fmtpagenum(curpage)
    )
    self.logpreview.define_brw(self.brw,u"Logické stránky")
    
#     self.brw.page(text=u"Náhled")
#     self.brw.vbox()
#     self.previewbar=self.brw.toolbar()
#     self.preview=self.brw.scrollwin(onpaint=self.OnPaintPreview,proportion=1,color='white').ctrl
#     self.brw.endsizer()
#     self.brw.endparent()
#     
#     self.brw.page(text=u"Logické stránky")
#     self.brw.vbox()
#     self.logpreviewbar=self.brw.toolbar()
#     self.logpreview=self.brw.scrollwin(onpaint=self.OnPaintLogPreview,proportion=1,color='white').ctrl
#     self.brw.endsizer()
#     self.brw.endparent()
    
    self.brw.endparent()
    
    self.brw.endsizer()
    
#     self.previewbar.label(model=lambda: u"Strana %d/%d"%(self.curpage+1,self.pagecount),layoutflags=wx.CENTER)
#     self.previewbar.space((10,10))
#     self.previewbar.button(text='<<',event=lambda ev:self.changepage(-1))
#     self.previewbar.button(text='>>',event=lambda ev:self.changepage(1))
#     self.previewbar.combo(id='zoom',model=['%d%%'%z for z in self._zooms],curmodel=browse.attr(self,'curzoom'),event=self.changezoom)
#     self.previewbar.realize()
    
    self.infobrw.grid(rows=3,cols=2,border=5)
    self.infobrw.label(text=u'Logických stránek:')
    self.infobrw.label(model=lambda:len(self.actsb.logpages.pages),default=u'???')
    self.infobrw.label(text=u'Fyzických stránek:')
    self.infobrw.label(model=lambda:self.actsb.a4d.sheetcnt()*2,default=u'???')
    self.infobrw.label(text=u'Nevyužitých stránek:')
    self.infobrw.label(model=lambda:self.actsb.a4d.freepgcnt(),default=u'???')
    self.infobrw.endsizer()
    
    #print len(self.brw.stack)
    #self.songv=wx.Panel(self.notebook,-1)
    #self.notebook.AddPage(self.songv,u"Text písně")
    
    #self.preview=wx.ScrolledWindow(self.notebook,-1,style=wx.SUNKEN_BORDER)
    #self.preview.SetScrollRate(20,20)
    #self.preview.Bind(wx.EVT_PAINT, self.OnPaintPreview)
    #self.notebook.AddPage(self.preview,u"Náhled")
    
    #self.panel.Hide()
    #self.notebook.Hide()    
    #print self.parent_window.IsShown()
    #self.parent_window.Hide()
    #print self.parent_window.IsShown()
    
    
  def edit_sb_type(self,ev=None):
    sbtype.edit_sb_type(self.actsb.sbtype)
    
  def on_destroy_menu(self):
    pass
    
  def create_toolbar(self,obj):
    toolbar=obj.get_toolbar()
    evtbinder=obj.get_event_binder()

    if desktop.active_content() in ('dbview','songbook'):
      bt=buttons.GenToggleButton(toolbar,-1,u"Zpěvník:")
      evtbinder.Bind(wx.EVT_BUTTON,self.OnPageToggleButton,bt)
      toolbar.AddControl(bt)
      bt.SetToggle(desktop.active_content()=='songbook')
    
      self.sbs=wx.ComboBox(
        toolbar,wx.NewId(),"", 
        size=(150,-1),style=wx.CB_DROPDOWN|wx.CB_READONLY
      )
      self.fillsbs()
      evtbinder.Bind(wx.EVT_COMBOBOX,self.OnChangeSB,self.sbs)
      #self.sbs.Bind(wx.EVT_SET_FOCUS,self.OnFocusSB)
      toolbar.AddControl(self.sbs)

    if desktop.active_content()=='songbook':
      utils.wx_add_art_tool(toolbar,self.newsb,wx.ART_NEW,evtbinder,u'Nový zpěvník',u'Vytvoří nový zpěvník')
      utils.wx_add_art_tool(toolbar,self.loadsb,wx.ART_FILE_OPEN,evtbinder,u'Otevřít zpěvník',u'Načte zpěvník z disku')
      utils.wx_add_art_tool(toolbar,self.savesb,wx.ART_FILE_SAVE,evtbinder,u'Uložit zpěvník',u'Uloží zpěvník na disk')
      utils.wx_add_art_tool(toolbar,self.savesbas,wx.ART_FILE_SAVE_AS,evtbinder,u'Uložit zpěvník na',u'Uloží zpěvník na disk, zobrazí dialog na výběr souboru')

  def newsb(self,event=None):
    self._addsb(songbook.SongBook())
  
  def _addsb(self,sb):
    self.on_deselect_old_sb()
    self.actsb=sb
    self.opensbs.append(self.actsb)
    self.fillsbs()
    self.on_select_new_sb()
    self.reformat()

  def loadsb(self,event=None):
    file=utils.open_dialog(desktop.main_window,u"Zpěvníky (*.zp)|*.zp",u"Otevřít zpěvník")
    if file: 
      desktop.show_content('songbook')
      sb=songbook.SongBook()
      sb.filename=file
      sb.load(open(file,'r'))
      self._addsb(sb)
    self.reformat()

  def savesbas(self,event=None):
    if not self.actsb: return
    file=utils.save_dialog(desktop.main_window,u"Zpěvníky (*.zp)|*.zp",self.actsb.filename,u"Uložit zpěvník")
    if file:
      self.actsb.filename=file
      self.actsb.save(open(file,'w'))

  def savesb(self,event=None):
    if not self.actsb: return
    if not self.actsb.filename: return self.savesbas()
    self.actsb.save(open(self.actsb.filename,'w'))

  def OnPageToggleButton(self,event):
    if event.GetIsDown(): desktop.show_content('songbook')

  def create_menu(self,obj):
    if self.content_visible and self.actsb:
      obj.create_submenu('songbook',u'Zpěvník')
      obj.create_menu_command('songbook/sbtype',u'Upravit typ zpěvníku',self.edit_sb_type,config.hotkey.edit_sb_type)


  def on_destroy_control(self):
    pass
    
  def on_show(self):
    self.content_visible=True
    #self.panel.Show()
    #self.notebook.Show()
    #self.panel.Layout()

  def on_hide(self):
    self.content_visible=False
    #self.parent_window.Hide()
    #self.panel.Hide()
    #self.notebook.Hide()    
    
  def get_name(self):
    return 'songbook'

  def fillsbs(self):
    self.ignore_change_sb=True
    utils.wx_fill_list(self.sbs,self.predefined_sb_list+self.opensbs)
    try:
      self.sbs.SetSelection(len(self.predefined_sb_list)+self.opensbs.index(self.actsb))
      self.OnChangeSB(None)
    except Exception,e:
      self.sbs.SetSelection(0)
    self.ignore_change_sb=False
    
  def on_deselect_old_sb(self):
    if not self.actsb: return
    #self.actsb.sbpanel=None
    #self.actsb.clearhooks()
    #self.brw['songlist'].clearhooks()
    if self.alloccol:
      dbview.remove_song_column(self.alloccol)
      self.alloccol=None
      
  def on_select_new_sb(self):
    if not self.actsb: return
    #self.actsb.sbpanel=self
    self.alloccol=dbview.add_song_column('insongbook',u'Tisk?',self.actsb.hassong,self.actsb.setsongcontain,gridlib.GridCellBoolRenderer,gridlib.GridCellBoolEditor);
    self.brw['songlist'].setmodel(self.actsb.songs)
    #self.actsb.onappend=self.brw['songlist'].append
    #self.actsb.onremove=self.brw['songlist'].remove
    #self.brw['songlist'].onremove=self.actsb.onremovesong
    
  #def OnFocusSB(self,event):
    #desktop.show_content('songbook')
    #event.Skip()
      
  def OnChangeSB(self,event):
    if self.ignore_change_sb: return
    self.on_deselect_old_sb()
    sel=self.sbs.GetSelection()
    if sel==1:
      self.newsb()
    if sel==2:
      self.loadsb()
    if sel>=len(self.predefined_sb_list):
      self.actsb=self.opensbs[sel-len(self.predefined_sb_list)]
      self.on_select_new_sb()

#   def OnPaintPreview(self,event):
#     if not self.actsb:
#       event.Skip()
#       return
#     zoom=self._zooms[self.curzoom]/100.0
#     dc=wx.PaintDC(self.preview)
#     self.preview.PrepareDC(dc)
#     dc.SetUserScale(self.actsb.rbt.pkoefx*zoom,self.actsb.rbt.pkoefy*zoom)
#     self.actsb.drawpage(format.DCCanvas(dc),self.curpage)

  def OnPaintLogPreview(self,event):
    pass

  #def refresh(self):
    #self.preview.Refresh()
    #self.previewbar.loadall()

  #def setvirtsize(self):
    #zoom=self._zooms[self.curzoom]/100.0
    #self.preview.SetVirtualSize((self.actsb.rbt.pw100*zoom,self.actsb.rbt.ph100*zoom))

  def format(self):
    self.lpp=None
    if not self.actsb: return
    self.actsb.format()
    self.lpp=self.actsb.logpagepreview()
    #self.pagecount=self.actsb.a4d.sheetcnt()*2
    self.preview.setvirtsize(self.actsb.rbt.pgwi,self.actsb.rbt.pghi)
    self.preview.update(self.actsb.a4d.sheetcnt()*2)
    self.logpreview.setvirtsize(*self.lpp.getpagesize())
    self.logpreview.update(self.lpp.getcount())
    self.changepage()
    self.infobrw.loadall()
  
  def reformat(self):
    if not self.actsb: return
    self.actsb.clearformat()
    self.format()

  def changepage(self):
    self.preview.changepage()
    self.logpreview.changepage()

#   def changepage(self,d=0):
#     self.curpage+=d
#     if self.curpage<0: self.curpage=0
#     if self.curpage>=self.pagecount: self.curpage=self.pagecount-1
#     self.refresh()
    
  def onchangebasesbtype(self,ev):
    self.actsb.sbtype.changebase(self.brw['sbtype'].getitem())
    self.reformat()  

  def format_songbook(self):
    self.format()
    