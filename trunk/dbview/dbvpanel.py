# -*- coding: utf-8 -*- 

import desktop
import desktop.intf
import wx
import database
import utils
import songtool.toolbars
import wx.lib.layoutf  as layoutf
import printing
import interop
import anchors.content
import wx.lib.buttons as buttons
import songlistctrl

# submodules
import songgrid
import songview

class DBVPanel(anchors.content.IContent):
  splitter=None
  #grid=None
  dbs=None
  #songv=None
  predefined_db_list=[u"- Vyberte databázi",u"- Nová databáze",u"- Stáhnout z internetu"]
  content_visible=False
  selected_db=None
  ignore_change_db=False
  cursong=None
  notebook=None
  gridctrl=None

  def __init__(self):
    desktop.register_menu(self.create_toolbar)
    desktop.register_menu(self.create_menu)
    desktop.show_content(self.get_name())
  
  def on_create_control(self,parent,evtbinder):
    self.notebook=wx.Notebook(parent,-1)
    self.notebook.SetConstraints(layoutf.Layoutf('t=t#1;l=l#1;r=r#1;b=b#1',(parent,)))

    self.gridctrl=songlistctrl.SongListCtrlAndSongView(self.notebook,songgrid.SongGrid)
    #self.splitter=wx.SplitterWindow(self.notebook,-1)
    self.notebook.AddPage(self.gridctrl,u'Písně - tabulka')
    #self.splitter.SetConstraints(layoutf.Layoutf('t=t#1;l=l#1;r=r#1;b=b#1',(parent,)))
    self.notebook.Hide()
    
    #self.grid=songgrid.SongGrid(self.splitter)
    #self.songv=songview.SongCtrl(self.splitter)
    
    #self.splitter.SetMinimumPaneSize(200)
    #self.splitter.SplitVertically(self.grid, self.songv)
    
    #self.grid.onsongclick=self.onsongclick
    #self.splitter.Hide()

    #self.transptoolbar=utils.SongToolBarWrap(self.songv)
    #self.transptoolbar.makebuttons(self,tb)    
    
  def on_destroy_menu(self):
    self.dbs=None
    #if self.dbs: self.dbs.Destroy()
    #self.dbs=None
    
  def create_toolbar(self,obj):
    toolbar=obj.get_toolbar()
    evtbinder=obj.get_event_binder()
   
    if desktop.active_content() in ('dbview','songbook'):
      bt=buttons.GenToggleButton(toolbar,-1,u"Databáze:")
      evtbinder.Bind(wx.EVT_BUTTON,self.OnPageToggleButton,bt)
      bt.SetToggle(desktop.active_content()=='dbview')
      toolbar.AddControl(bt)
    
    
      self.dbs=wx.ComboBox(
        toolbar,wx.NewId(),"", 
        size=(150,-1),style=wx.CB_DROPDOWN|wx.CB_READONLY
      )
      self.filldbs()
      evtbinder.Bind(wx.EVT_COMBOBOX, self.OnChangeDb, self.dbs)
      #self.dbs.Bind(wx.EVT_SET_FOCUS,self.OnFocusDb)
      toolbar.AddControl(self.dbs)
    
    #print 'create tyoolbar'
    if self.visible(): 
      self.gridctrl.create_toolbar(obj)
      #print 'visible'
#       songtool.toolbars.make_transp_toolbar(self.songv,evtbinder,toolbar)
#       toolbar.AddSeparator()
#       utils.wx_add_art_tool(toolbar,self.printsong,wx.ART_PRINT,evtbinder,u'Tisk písně',u'Vytiskne aktuální píseň')

  def OnPageToggleButton(self,event):
    if event.GetIsDown(): desktop.show_content('dbview')

  def create_menu(self,obj):
    #obj.create_menu_command('song/trprev5',title,event,hotkey=u'',hint=u''):
    if self.visible():
      obj.create_submenu('song',u'Píseň')
      self.gridctrl.create_menu(obj)
      #songtool.toolbars.make_transp_menu(self.songv,obj)

  def on_destroy_control(self):
    self.dbs=None
    
  def on_show(self):
    self.content_visible=True
    self.showifneeded()

  def on_hide(self):
    self.content_visible=False
    self.showifneeded()
    
  def OnChangeDb(self,event):
    try:
      interop.disable_messaging()
      if self.ignore_change_db: return
      sel=self.dbs.GetSelection()
      if sel==0: self.selected_db=None
      if sel==1 or sel==2:
        dlg=wx.TextEntryDialog(desktop.main_window,u"Zadej jméno nové databáze",u"Zpěvníkátor")
        if dlg.ShowModal()==wx.ID_OK:
          db=database.dbmanager.create_inet_db(dlg.GetValue())
          self.selected_db=db
          self.filldbs()
          self.gridctrl.set_data(db)
          #wx.MessageDialog(self,u"Nová databáze %s" % dlg.GetValue(),u"Zpěvníkátor").ShowModal()
          #desktop.recreate_menu()
          
      if sel>=len(self.predefined_db_list):
        db=database.dbmanager[sel-len(self.predefined_db_list)]
        if db!=self.selected_db:
          self.selected_db=db
          self.gridctrl.set_data(db)
          #desktop.recreate_menu()
  
      self.showifneeded()
      desktop.recreate_menu()
    finally:
      interop.enable_messaging()

  def filldbs(self):
    self.ignore_change_db=True
    dblist=list(iter(database.dbmanager))
    utils.wx_fill_list(self.dbs,self.predefined_db_list+dblist)
    try:
      self.dbs.SetSelection(len(self.predefined_db_list)+dblist.index(self.selected_db))
      self.OnChangeDb(None)
    except Exception,e:
      self.dbs.SetSelection(0)
    self.ignore_change_db=False

  def showifneeded(self):
    if self.notebook: self.notebook.Show(self.visible())
      
#   def onsongclick(self,db,songid):
#     song=db[songid]
#     self.songv.setsong(song)
#     self.cursong=song

  def getcurdb(self):
    sel=self.dbs.GetSelection()
    if sel<len(self.predefined_db_list):return None
    return database.dbmanager[sel-len(self.predefined_db_list)]

  def visible(self):
    if not self.content_visible: return False
    if not self.dbs: return False
    if self.dbs.GetSelection()<len(self.predefined_db_list): return False
    return True
    
  def get_name(self):
    return 'dbview'    
    
#   def printsong(self,event=None):
#     if self.cursong: printing.printsong(self.cursong)
    
  #def OnFocusDb(self,event):
    #desktop.show_content('dbview')
    #event.Skip()

    