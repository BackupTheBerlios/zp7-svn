import wx
from wx.lib.layoutf import Layoutf
import traceback, sys, os

import abcedit, viewpanel, worker, wxutils, convert, htmldialog

ID_NEW = wx.NewId()
ID_OPEN = wx.NewId()
ID_SAVE = wx.NewId()
ID_SAVEAS = wx.NewId()
ID_EXPORT = wx.NewId()
ID_EXIT = wx.NewId()

mainicon = None

class NotEditFrame(wx.Frame):
    editor = None
    view = None
    worker = None
    filename = None
    modified = False
    log = None
    oldstdout = None
    
    def __init__(self):
        wx.Frame.__init__(self, None, -1, u'NotoEditor for ABC', size=(640, 480))

        menuBar = wx.MenuBar()
        menu = wx.Menu()
        
        menu.Append(ID_NEW, "&New\tCtrl+N", "Creates new empty ABC file")
        menu.Append(ID_OPEN, "&Open\tCtrl+O", "Opens existing ABC file")
        menu.Append(ID_SAVE, "&Save\tCtrl+S", "Saveas current file to new location")
        menu.Append(ID_SAVEAS, "Save &as\tCtrl+Shift+S", "Saveas current file")
        menu.Append(ID_EXPORT, "&Export\tCtrl+E", "Exports to image or music file")
        menu.Append(ID_EXIT, "&Close", "Closes this NotoEditor window")
        
        self.Bind(wx.EVT_MENU, self.OnNew,  id=ID_NEW)
        self.Bind(wx.EVT_MENU, self.OnOpen,  id=ID_OPEN)
        self.Bind(wx.EVT_MENU, self.OnSave,  id=ID_SAVE)
        self.Bind(wx.EVT_MENU, self.OnSaveAs,  id=ID_SAVEAS)
        self.Bind(wx.EVT_MENU, self.OnExport,  id=ID_EXPORT)
        self.Bind(wx.EVT_MENU, self.OnQuit,  id=ID_EXIT)
        self.Bind(wx.EVT_SIZE, self.OnSize)
        self.Bind(wx.EVT_IDLE, self.OnIdle)
        self.Bind(wx.EVT_CLOSE, self.OnClose)

        menuBar.Append(menu, "&File")
        self.SetMenuBar(menuBar)

        self.CreateStatusBar()

        self.log = wx.TextCtrl(self, -1, style=wx.TE_MULTILINE | wx.TE_DONTWRAP)
        self.log.SetConstraints(Layoutf('l!0;h!100;w=w#1;b=b#1', (self,)))

        self.editor = abcedit.SynEdit(self)
        self.editor.SetConstraints(Layoutf('l!0;t!0;w%w50#1;b^#2', (self, self.log)))
        self.editor.on_change = self.OnChange
        #self.editor.SetConstraints(Layoutf('t=t#1;l=r10#1;r!100;h%h50#1', (self,)))
        
        self.view = viewpanel.ViewPanel(self)
        #self.view.SetConstraints(Layoutf('l%w50#1;t!0;w%w50#1;b=b#1', (self,)))
        self.view.SetConstraints(Layoutf('l%w50#1;t!0;w%w50#1;b^#2', (self, self.log)))
        
        self.worker = worker.WorkerThread()
        self.worker.start()

        if len(sys.argv) >= 2:
            self.filename = sys.argv[1]
            self.loadfromfile()
            self.updatetitle()
            
        if mainicon is not None:
            self.SetIcon(mainicon)
        
        self.oldstdout = sys.stdout
        sys.stdout = self
        self.Maximize()
        self.editor.SetFocus()
        
    def write(self, msg):
        self.oldstdout.write(msg)
        self.log.AppendText(msg)        
        
    def canclearcontent(self):
        if not self.modified: return True
        dlg = wx.MessageDialog(self, u'File modified, save?', 'Warning', wx.YES_NO | wx.CANCEL)
        res = dlg.ShowModal()
        dlg.Destroy()
        if res == wx.ID_YES: return self.save()
        if res == wx.ID_NO: return True
        return False

    def save(self):
        if self.filename is None: return self.saveas()
        self.savetofile()
        return True
        
    def loadfromfile(self):
        self.editor.SetText(unicode(open(self.filename, 'rb').read(), 'utf-8'))
        self.modified = False
    
    def savetofile(self):
        open(self.filename, 'wb').write(self.editor.GetText().encode('utf-8'))
        self.modified = False

    def saveas(self):
        fn = wxutils.save_dialog(self, 'ABC Files (*.abc)|*.abc', self.filename)
        if fn is not None:
            self.filename = fn
            self.updatetitle()
            self.savetofile()
            return True
        else:
            return False

    def OnOpen(self, evt):
        if self.canclearcontent():
            fn = wxutils.open_dialog(self, 'ABC Files (*.abc)|*.abc')
            if fn is not None:
                self.filename = fn
                self.updatetitle()
                self.loadfromfile()

    def OnNew(self, evt):
        if self.canclearcontent():
            htmldialog.run_html_file(self, 'new_wizard.html', {'mainwin' : self})
    
    def create_new_file(self, contents=u''):
        self.filename = None
        self.updatetitle()
        self.view.set_bitmap(None)
        self.editor.SetText(contents)
        self.modified = False

    def gsexport(self, fn, gsdevice):
        abcfile = convert.tmp_file('abc')
        psfile = convert.tmp_file('ps')
        open(abcfile, 'wb').write(self.editor.GetText().encode(convert.abc_encoding))
        convert.run_abc2ps(abcfile, psfile)
        convert.run_gsexe(psfile, fn, gsdevice)
        convert.remove_files(abcfile, psfile)

    def midiexport(self, fn):
        abcfile = convert.tmp_file('abc')
        open(abcfile, 'wb').write(self.editor.GetText().encode('utf-8'))
        convert.run_abc2midi(abcfile, fn)
        convert.remove_files(abcfile)

    def OnExport(self, evt):
        flt = 'PNG Images (*.png)|*.png|BMP Images (*.bmp)|*.bmp|JPEG Images (*.jpg)|*.jpg'
        flt += '|MIDI music (*.mid)|*.mid|PDF Files (*.pdf)|*.pdf|PostScript files (*.ps)|*.ps'
        flt += '|TIFF Images (*.tif)|*.tif'
        fn = wxutils.save_dialog(self, flt, os.path.splitext(self.filename)[0])
        if fn is not None:
            ext = os.path.splitext(fn)[1].lower()
            if ext == '.png': self.gsexport(fn, 'pngmono')
            if ext == '.ps': self.gsexport(fn, 'pswrite')
            if ext == '.pdf': self.gsexport(fn, 'pdfwrite')
            if ext == '.tif': self.gsexport(fn, 'tiffcrle')
            if ext == '.jpg': self.gsexport(fn, 'jpeg')
            if ext == '.bmp': self.gsexport(fn, 'bmpmono')
            if ext == '.mid': self.midiexport(fn)

    def updatetitle(self):
        if self.filename is not None:
            self.SetTitle(u'NotoEditor for ABC [%s]' % self.filename)
        else:
            self.SetTitle(u'NotoEditor for ABC')

    def OnSave(self, evt):
        self.save()

    def OnSaveAs(self, evt):
        self.saveas()

    def OnClose(self, evt):
        if self.canclearcontent():
            self.worker.quit()
            evt.Skip()

    def OnQuit(self, evt):
        self.Close()

    def OnSize(self, evt):
        self.Layout()

    def OnIdle(self, evt):
        try:
            bmp = self.worker.get_output()
            if bmp is not None:
                self.view.set_bitmap(bmp)
        except:
            traceback.print_exc()
        
    def OnChange(self, evt):
        self.modified = True
        self.worker.set_input(self.editor.GetText().encode(convert.abc_encoding))

class NotEditApp(wx.App):
    def OnInit(self):
        try:
            global mainicon
            import win32api 
            exeName = win32api.GetModuleFileName(win32api.GetModuleHandle(None)) 
            mainicon = wx.Icon(exeName, wx.BITMAP_TYPE_ICO) 
        except:
            pass # we are not under windows
    
        frame = NotEditFrame()
        self.SetTopWindow(frame)
        frame.Show(True)
        return True

        
app = NotEditApp(redirect=False)
app.MainLoop()
