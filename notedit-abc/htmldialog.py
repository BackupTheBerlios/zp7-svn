import wx, os, os.path, sys, copy
import  wx.html as  html
from wx.lib.layoutf import Layoutf

basedir = os.path.dirname(sys.argv[0])
htmldir = os.path.join(basedir, 'html')
sys.path.append(htmldir)

class HtmlPanel(html.HtmlWindow):
    dialog = None
    vars_dict = None
    def __init__(self, parent):
        html.HtmlWindow.__init__(self, parent, -1, style=wx.NO_FULL_REPAINT_ON_RESIZE)
        self.dialog = parent
        if "gtk2" in wx.PlatformInfo:
            self.SetStandardFonts()

    def OnLinkClicked(self, linkinfo):
        href = linkinfo.GetHref()
        if href.startswith('python:'):
            cmd = href[len('python:'):]
            dct = copy.copy(self.vars_dict)
            dct['close'] = self.dialog.Close
            code = compile(cmd.replace('\r\n','\n'), self.GetOpenedPage(), 'exec')        
            exec code in dct
        else:
            self.base_OnLinkClicked(linkinfo)

    def OnSetTitle(self, title):
        self.dialog.SetTitle(title)
        self.base_OnSetTitle(title)


class HtmlDialog(wx.Dialog):
    panel = None
    def __init__(self, parent):
        wx.Dialog.__init__(self, parent, -1, size=(600, 400))
        self.panel = HtmlPanel(self)
        self.panel.SetConstraints(Layoutf('t=t#1;l=l#1;b=b#1;r=r#1', (self,)))
        self.Bind(wx.EVT_SIZE, self.OnSize)

    def OnSize(self, evt):
        self.Layout()


def run_html_file(parent, fn, vars_dict):
    path = os.path.join(htmldir, fn)
    dlg = HtmlDialog(parent)
    dlg.panel.vars_dict = vars_dict
    dlg.panel.LoadPage(path)
    dlg.ShowModal()

