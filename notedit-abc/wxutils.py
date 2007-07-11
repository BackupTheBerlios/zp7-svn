import wx, os, os.path

def open_dialog(parent, wildcard, message = u"Open"):
    while True:
        try:
            dlg = wx.FileDialog(
                parent, message = message, defaultDir = os.getcwd(),
                defaultFile = "", wildcard = wildcard, style = wx.OPEN | wx.CHANGE_DIR
            )
            if dlg.ShowModal() == wx.ID_OK:
                file = dlg.GetPath()
                if os.path.isfile(file): return file
                wx.MessageDialog(parent, u'File %s was not found' % file, u'Error').ShowModal()
            else:
                return None
        finally:
            pass
            #dlg.Destroy()

def save_dialog(parent, wildcard, deffile = '', message = u"Save"):
    try:
        if deffile:
            dir = os.path.dirname(deffile)
            file = os.path.basename(deffile)
        else:
            dir = os.getcwd()
            file = ''
        dlg = wx.FileDialog(
            parent, message = message, defaultDir = dir,
            defaultFile = file, wildcard = wildcard, style = wx.SAVE | wx.CHANGE_DIR | wx.OVERWRITE_PROMPT
        )
        if dlg.ShowModal() == wx.ID_OK: return dlg.GetPath()
        return None
    finally:
        dlg.Destroy()

