# -*- coding: utf-8 -*- 

import config
import desktop.intf
import wx.stc
import wx.lib.layoutf  as layoutf
import utils
import os.path
import sys
import shutil
import anchors.content

import config
#import config.keyboard
#import config.fonts

class CfgPanel(anchors.content.IContent):
  stc=None
  content_visible=False
  
  files=('hotkeys','fonts')
  modules=[getattr(config,f) for f in files]
  
  basedir=os.path.join(os.path.dirname(sys.argv[0]),"config")
  loadedidx=0
  
  def on_create_control(self,parobj,evtbinder):
    self.stc=wx.stc.StyledTextCtrl(id=-1,name='stc',
              parent=parobj,style=wx.SUNKEN_BORDER)
    self.stc.SetConstraints(layoutf.Layoutf('t=t#1;l=l#1;r=r#1;b=b#1',(parobj,)))
    self.stc.Hide()
    desktop.register_menu(self.create_toolbar)

  def create_toolbar(self,obj):
    toolbar=obj.get_toolbar()
    evtbinder=obj.get_event_binder()
   
    if self.content_visible:
      self.cfgfiles=wx.ComboBox(
        toolbar,wx.NewId(),"", 
        size=(150,-1),style=wx.CB_DROPDOWN|wx.CB_READONLY
      )
      utils.wx_fill_list(self.cfgfiles,self.files)
      evtbinder.Bind(wx.EVT_COMBOBOX, self.OnChangeFile, self.cfgfiles)
      toolbar.AddControl(self.cfgfiles)
      self.cfgfiles.SetSelection(self.loadedidx)
      self.OnChangeFile(None)

      toolbar.AddSeparator()

      utils.wx_add_art_tool(toolbar,self.save_act,wx.ART_FILE_SAVE,evtbinder)
      utils.wx_add_art_tool(toolbar,lambda ev:desktop.show_content('dbview'),wx.ART_QUIT,evtbinder)
  
  def make_fn(self,index):
    return os.path.join(self.basedir,self.files[index])+".py"

  def save_act(self,event=None):
    if self.loadedidx!=None:
      fn=self.make_fn(self.loadedidx)
      shutil.copyfile(fn,fn+".bak")
      f=open(fn,"w")
      f.write(self.stc.GetText())
      f.close()
      try:
        reload(self.modules[self.loadedidx])
        desktop.recreate_menu()
      except Exception,e:
        shutil.copyfile(fn+".bak",fn)
        wx.MessageDialog(desktop.main_window,u'Chyba při zpracování konfiguračního souboru:%s'%unicode(e),u'Zpěvníkátor',).ShowModal()
    
  def OnChangeFile(self,event):
    self.loadedidx=self.cfgfiles.GetSelection()
    f=open(self.make_fn(self.loadedidx))
    self.stc.SetText(f.read())
    f.close()
  
  def on_destroy_control(self):
    pass
    
  def on_show(self):
    self.stc.Show()
    self.content_visible=True

  def on_hide(self):
    self.stc.Hide()
    self.content_visible=False
  
  def get_name(self):
    return 'cfg-scripts'
    