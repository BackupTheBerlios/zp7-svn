# -*- coding: UTF-8 -*-

import wx
import desktop
import intf
import interop
import wx.lib.layoutf  as layoutf


class MenuTree:
  childs=[]
  name=''
  wxmenu=None
  
  def __init__(self,name=''):
    self.childs=[]
    self.name=name

  def create_submenu(self,name,title):
    child=MenuTree(name)
    child.wxmenu=wx.Menu()
    self.wxmenu.Append(child.wxmenu,title)
    self.childs.append(child)

  def create_menu_command(self,name,title,event,hotkey,hint,wxwindow,bitmap):
    child=MenuTree(name)
    s=title
    if hotkey: s+=u'\t'+hotkey
    
    child.wxmenu=wx.MenuItem(self.wxmenu,-1,s,hint)
    if bitmap: child.wxmenu.SetBitmap(bitmap)
    self.wxmenu.AppendItem(child.wxmenu)
    
    #child.wxmenu=self.wxmenu.Append(-1,s,hint)
    #if bitmap: child.wxmenu.SetBitmap(bitmap)
    self.childs.append(child)
    wxwindow.Bind(wx.EVT_MENU,event,child.wxmenu)

  def _addxxx(self,subpath,fn,args):
    if len(subpath)==0: return self
    subs=[sub for sub in self.childs if sub.name==subpath[0]]
    if len(subs)>0: return subs[0]._addxxx(subpath[1:],fn,args)
    assert len(subpath)==1
    getattr(self,fn)(subpath[0],*args)
    

class MainWindow(wx.Frame,intf.IMenuCreator):
  menutree=None
  toolbar=None
  should_show_content=None
  visible_content=None
  created_controls=False
  shell=None
  #want_recreate_menu=False

  def __init__(self, parent):
    wx.Frame.__init__(self, parent, -1, u'Zpěvníkátor',size=(800,600))
    self.CreateStatusBar()
    self.toolbar=self.CreateToolBar(wx.TB_HORIZONTAL|wx.NO_BORDER|wx.TB_FLAT|wx.TB_TEXT)
    self.SetAutoLayout(True)
    self.should_show_content=desktop._should_show_content
    interop.define_flag('recreate_menu',self.recreate_menu)
    self.Bind(wx.EVT_IDLE,self.OnIdle)

  def create_menu_command(self,path,title,event,hotkey=u'',hint=u'',bitmap=None):
    self.menutree._addxxx(path.split('/'),'create_menu_command',(title,event,str(hotkey),hint,self,bitmap))

  def create_submenu(self,path,title):
    self.menutree._addxxx(path.split('/'),'create_submenu',(title,))

  def OnIdle(self,event):
    interop.process_messages()
#    if self.want_recreate_menu: self.do_recreate_menu()

    #self.want_recreate_menu=True
  
  #def do_recreate_menu(self): 
  def recreate_menu(self): 
    import config
  
    try:  
      self.Freeze()

      #print "recreating"
      #self.want_recreate_menu=False
      oldmenubar=None
      if self.menutree:
        for item in interop.anchor['content']: item.on_destroy_menu()
        oldmenubar=self.menutree.wxmenu
      if self.toolbar: self.toolbar.Destroy()
      
      self.toolbar=self.CreateToolBar(wx.TB_HORIZONTAL|wx.NO_BORDER|wx.TB_FLAT|wx.TB_TEXT)
        
      #self.toolbar.ClearTools()
      self.menutree=MenuTree()
      self.menutree.wxmenu=wx.MenuBar()
      
      self.create_submenu('file','&Soubor')
      for item in desktop._registered_items: item(self)
      self.create_menu_command('file/shell','&Shell',self.OnShell,config.hotkey.shell,u"Spustí shell Pythonu")
      self.create_menu_command('file/exit','&Konec',self.OnExit,config.hotkey.quit,u"Ukončí program Zpěvníkátor")
      
  
      self.SetMenuBar(self.menutree.wxmenu)
      if oldmenubar: oldmenubar.Destroy()
      self.toolbar.Realize()
    finally:
      self.Thaw()

  def get_toolbar(self): return self.toolbar
  def get_event_binder(self): return self

  def create_controls(self):
    for item in interop.anchor['content']:
      item.parent_window=wx.Window(self,-1)
      item.parent_window.SetConstraints(layoutf.Layoutf('t=t#1;l=l#1;r=r#1;b=b#1',(self,)))
      item.parent_window.Bind(wx.EVT_SIZE,lambda ev:item.parent_window.Layout(),None)
      item.parent_window.Hide()
      item.on_create_control(item.parent_window,self)
      #item.on_create_control(self,win)
      #win.Hide()
      #item.parent_window=win
      
    self.created_controls=True
    if self.should_show_content: self.show_content(self.should_show_content)
  
  def OnExit(self,event):
    self.Close(True)
    
  def show_content(self,name):
    try:
      self.Freeze()
      if self.created_controls:
        if self.visible_content: 
          self.visible_content.on_hide()
          self.visible_content.parent_window.Hide()
        self.visible_content=None
        for cnt in interop.anchor['content']:
          if cnt.get_name()==name:
            self.visible_content=cnt
            cnt.parent_window.Show()
            cnt.on_show()
            self.recreate_menu()
            return           
      else:
        self.should_show_content=name
    finally:
      self.Thaw()

  def active_content(self):
    if self.visible_content: return self.visible_content.get_name()
    return ''

  def OnShell(self,event):
    if self.shell:
      # if it already exists then just make sure it's visible
      s = self.shell
      if s.IsIconized():
        s.Iconize(False)
      s.Raise()
    else:
      # Make a PyShell window
      from wx import py
      import database
      
      namespace = { 'wx' : wx,
                    'app' : wx.GetApp(),
                    'desktop' : desktop,
                    'dbmanager' : database.dbmanager
                    }
      #self.shell = py.shell.ShellFrame(None, locals=namespace)
      self.shell = py.crust.CrustFrame(None, locals=namespace)
      self.shell.SetSize((640,480))
      self.shell.Show()



def create_main_window():
  desktop.main_window=MainWindow(None)