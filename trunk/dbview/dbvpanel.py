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
import groupview
import code
import config
from songtool import songedit
from internet import serverconfig
import serverview
import browse
from database import songdb
from lxml import etree
from StringIO import StringIO

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
  groupsongctrl=None
  groupctrl=None
  serverctrl=None

  def __init__(self):
    desktop.register_menu(self.create_toolbar)
    desktop.register_menu(self.create_menu)
    desktop.show_content(self.get_name())
    interop.define_flag('reloaddb',self.reloaddb)
  
  def on_create_control(self,parent,evtbinder):
    self.notebook=wx.Notebook(parent,-1)
    self.notebook.SetConstraints(layoutf.Layoutf('t=t#1;l=l#1;r=r#1;b=b#1',(parent,)))

    self.gridctrl=songlistctrl.SongListCtrlAndSongView(self.notebook,songgrid.SongGrid)
    self.groupsongctrl=songlistctrl.SongListCtrlAndSongView(self.notebook,groupview.GroupSongGrid)
    self.groupctrl=groupview.GroupGrid(self.notebook)
    self.serverctrl=serverview.ServerGrid(self.notebook)
    #self.splitter=wx.SplitterWindow(self.notebook,-1)
    self.notebook.AddPage(self.gridctrl,u'Písně - tabulka')
    self.notebook.AddPage(self.groupsongctrl,u'Písně po skupinách')
    self.notebook.AddPage(self.groupctrl,u'Skupiny')
    self.notebook.AddPage(self.serverctrl,u'Servery')
    self.notebook.Bind(wx.EVT_NOTEBOOK_PAGE_CHANGING,self.OnNotebookChanging)
    self.notebook.Bind(wx.EVT_NOTEBOOK_PAGE_CHANGED,self.OnNotebookChanged)
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

  def cursonglist(self,index=None):
    if index is None: index=self.notebook.GetSelection()
    if index==0: return self.gridctrl
    if index==1: return self.groupsongctrl
    return None
    
  def selectedsong(self):
    try:
      db,id=self.cursonglist().get_cur_song()
      return db[id]
    except:
      return None

  def OnNotebookChanging(self,ev):
    sl=self.cursonglist()
    if sl:
      xxx,self.tmp_cursong=sl.get_cur_song()
    ev.Skip()

  def OnNotebookChanged(self,ev):
    ev.Skip()
    if hasattr(self,'tmp_cursong'):
      sl=self.cursonglist(ev.GetSelection())
      if sl:
        sl.set_cur_song(self.tmp_cursong)
      del self.tmp_cursong
    desktop.recreate_menu()

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
      try:
        self.cursonglist().create_toolbar(obj)
      except:
        pass
      #self.gridctrl.create_toolbar(obj)
      #print 'visible'
#       songtool.toolbars.make_transp_toolbar(self.songv,evtbinder,toolbar)
#       toolbar.AddSeparator()
#       utils.wx_add_art_tool(toolbar,self.printsong,wx.ART_PRINT,evtbinder,u'Tisk písně',u'Vytiskne aktuální píseň')

  def OnPageToggleButton(self,event):
    if event.GetIsDown(): desktop.show_content('dbview')

  def dbfind(self,ev):
    from database.filterdialog import FilterDialog
    dlg=FilterDialog()
    if dlg.run():
      self.gridctrl.setcond(dlg.gensql())

  def cancelfind(self,ev):
    self.gridctrl.setcond(None)

  def editsong(self,ev):
    song=self.selectedsong()
    if song: songedit.editsong(song)
  
  def reloaddb(self):
    self.gridctrl.reload()
    self.groupsongctrl.reload()
    self.groupctrl.reload()
    self.serverctrl.reload()
  
  def create_menu(self,obj):
    #obj.create_menu_command('song/trprev5',title,event,hotkey=u'',hint=u''):
    if self.visible():
      obj.create_submenu('database',u'Databáze')
      if self.notebook.GetSelection()==0: # tabulka pisni
        obj.create_menu_command('database/find',u'Hledat',self.dbfind,config.hotkey.db_find)
        obj.create_menu_command('database/cancelfind',u'Zrušit filtr',self.cancelfind,config.hotkey.db_cancelfind)
        #obj.create_menu_command('database/sendupdate',u'Aktualizovat internetovou databázi',self.sendupdate,config.hotkey.sendupdate)
        obj.create_menu_command('database/addsong',u'Přidat píseň',self.addsong,config.hotkey.addsong)
      if self.notebook.GetSelection()==2: # tabulka skupin
        obj.create_menu_command('database/addgroup',u'Přidat skupinu',self.addgroup,config.hotkey.addgroup)
        obj.create_menu_command('database/editgroup',u'Upravit skupinu',self.editgroup,config.hotkey.editgroup)
      if self.notebook.GetSelection()==3: # tabulka serveru
        obj.create_menu_command('database/addserver',u'Přidat server',self.addserver,config.hotkey.addserver)
        obj.create_menu_command('database/clearserver',u'Vyčistit záznamy serveru',self.clearserver,config.hotkey.clearserver)
        obj.create_menu_command('database/editserver',u'Upravit server',self.editserver,config.hotkey.editserver)
        obj.create_menu_command('database/updateserver',u'Aktualizovat ze serveru',self.updateserver,config.hotkey.updateserver)

      obj.create_menu_command('database/exporttoshare',u'Exportovat pro sdílení',self.exporttoshare,config.hotkey.dbexporttoshare)
      obj.create_menu_command('database/updateall',u'Aktualizovat databázi',self.updateall,config.hotkey.updateall)
        
      obj.create_submenu('song',u'Píseň')
      obj.create_menu_command('song/edit',u'Upravit',self.editsong,config.hotkey.editsong)
      try:
        self.cursonglist().create_menu(obj)
      except:
        pass
      #songtool.toolbars.make_transp_menu(self.songv,obj)

  def addsong(self,ev):
    pass

  def addgroup(self,ev):
    groupview.addgroupdialog(self.getcurdb())

  def editgroup(self,ev):
    db,id=self.groupctrl.getcurdbtuple()
    groupview.editgroupdialog(db.group(id))
  
  def addserver(self,ev):
    serverview.addserverdialog(self.getcurdb())

  def editserver(self,ev):
    db,id=self.serverctrl.getcurdbtuple()
    serverview.editserverdialog(db.server(id))

  def clearserver(self,ev):
    self.getcurdb().clearserver(self.serverctrl.getcurdbtuple()[1])
    interop.send_flag('reloaddb')

  def updateserver(self,ev):
    try:
      dlg=wx.ProgressDialog(u"Aktualizace ze serveru",u"Aktualizace ze serveru",parent=desktop.main_window)
      db, serverid = self.serverctrl.getcurdbtuple()
      server = db.server(serverid).iserver()
      self.getcurdb().clearserver(serverid)
      s = server.download_db()
      xml = etree.parse(StringIO(s))
      self.getcurdb().importxml(xml, serverid)
      interop.send_flag('reloaddb')
    finally:
      dlg.Destroy()

  def updateall(self,ev):
    pass
  
  def exporttoshare(self,ev):
    file_name=utils.save_dialog(desktop.main_window,'XML soubory|*.xml')
    if file_name:
      self.getcurdb().exporttoshare(file_name)
    
  def sendupdate(self,ev):
    db=self.getcurdb()
    for server in songdb.DBServer.enum(db):
      xml=db.compile_update_xml(server.id)
      print xml.tostr()
    raise NotImplemented()

  def on_destroy_control(self):
    self.dbs=None
    
  def on_show(self):
    self.content_visible=True
    self.showifneeded()

  def on_hide(self):
    self.content_visible=False
    self.showifneeded()
    
  def set_data(self,db):
    self.gridctrl.set_data(db)
    self.groupsongctrl.set_data(db)
    self.groupctrl.set_data(db)
    self.serverctrl.set_data(db)
    
  def OnChangeDb(self,event):
    try:
      interop.disable_messaging()
      if self.ignore_change_db: return
      sel=self.dbs.GetSelection()
      if sel==0: self.selected_db=None
      if sel==1 or sel==2:
        dlg=wx.TextEntryDialog(desktop.main_window,u"Zadej jméno nové databáze",u"Zpěvníkátor")
        if dlg.ShowModal()==wx.ID_OK:
          if sel==2: servers=serverconfig.ask_servers()
          else: servers=[]
          if servers or sel==1:
            db=database.dbmanager.create_database(dlg.GetValue(),servers)
            self.selected_db=db
            self.filldbs()
            self.set_data(db)
          #wx.MessageDialog(self,u"Nová databáze %s" % dlg.GetValue(),u"Zpěvníkátor").ShowModal()
          #desktop.recreate_menu()
          
      if sel>=len(self.predefined_db_list):
        db=database.dbmanager[sel-len(self.predefined_db_list)]
        if db!=self.selected_db:
          self.selected_db=db
          self.set_data(db)
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
    
  def add_song_column(self,col):
    self.gridctrl.add_song_column(col)
    self.groupsongctrl.add_song_column(col)
  
  def remove_song_column(self,col):
    self.gridctrl.remove_song_column(col)
    self.groupsongctrl.remove_song_column(col)
    
    
#   def printsong(self,event=None):
#     if self.cursong: printing.printsong(self.cursong)
    
  #def OnFocusDb(self,event):
    #desktop.show_content('dbview')
    #event.Skip()