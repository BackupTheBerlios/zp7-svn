# -*- coding: UTF-8 -*-

import wx
import locale
import startup
import desktop
import desktop.mainwin
import interop
import os.path
import sys
import utils
import traceback
import internet.downloader

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

def excepthook(type,value,tb):
#print type
    s=''.join(traceback.format_exception(type,value,tb))
    utils.showerror(s)

def main():
    locale.setlocale(locale.LC_ALL,'cz')
    sys.excepthook=excepthook
    app=ZpApp()
    internet.downloader.checknewversion()
    app.MainLoop()

if __name__=='__main__':
    main()

