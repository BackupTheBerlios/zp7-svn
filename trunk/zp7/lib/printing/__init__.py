# -*- coding: UTF-8 -*-

import wx
import config
import desktop

printData = wx.PrintData()
printData.SetPaperId(wx.PAPER_LETTER)
printData.SetPrintMode(wx.PRINT_MODE_PRINTER)

def print_setup():
    global printData
    data=wx.PrintDialogData(printData)
    dlg=wx.PrintDialog(desktop.main_window,data)
    dlg.GetPrintDialogData().SetSetupDialog(True)
    dlg.ShowModal();
    printData=wx.PrintData(dlg.GetPrintDialogData().GetPrintData())
    dlg.Destroy()


def printsong(song):
    import format

    pars=format.SongFormatPars()
    pars.fonts['label']=format.SongFont(**config.font.printsong_label)
    pars.fonts['text']=format.SongFont(**config.font.printsong_text)
    pars.fonts['chord']=format.SongFont(**config.font.printsong_chord)

    dc=wx.PrinterDC(printData)
    pars.transformfonts(dc)

    w,h=dc.GetSize()

    fmt=format.SongFormatter(dc,pars,song.text,w)
    fmt.run()

    dc.StartDoc(song.title)
    fmt.panegrp.draw(format.DCCanvas(dc))
    dc.EndDoc()

def _create_menu(obj):
    obj.create_menu_command('options/printer_settings',u'Nastavení tiskárny',lambda ev:print_setup(),config.hotkey.printer_settings)

desktop.register_menu(_create_menu)

