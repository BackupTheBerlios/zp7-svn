# -*- coding: UTF-8 -*-

import wx
import locale
import startup
import desktop
import desktop.mainwin
import interop
import os.path
import sys

class ZpApp(wx.App):
  """ Application object, responsible for the Splash screen, applying command
    line switches, optional logging and creation of the main frames. """

  def __init__(self):
    wx.App.__init__(self, False)

  def OnInit(self):
    startup.run_startup(self)
    
    desktop.mainwin.create_main_window()
    self.SetTopWindow(desktop.main_window)
    
    desktop.recreate_menu()
    desktop.main_window.create_controls()
    desktop.main_window.Show(True)
    return True

if __name__=='__main__':
  interop.library_directory=os.path.dirname(sys.argv[0])
  interop.initialize()
  locale.setlocale(locale.LC_ALL,'cz')
  app=ZpApp()
  app.MainLoop()
  