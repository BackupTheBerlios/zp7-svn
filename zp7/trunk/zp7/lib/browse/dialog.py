import wx
from ext import Browse

class DialogBrowse(Browse):
    dlg=None
    def __init__(self,parent=None,title=u'Dialog'):
        self.dlg=wx.Dialog(parent,-1,title)
        Browse.__init__(self,self.dlg)

    def run(self):
        """shows modal dialog"""
        self.dlg.CenterOnScreen()
        return self.dlg.ShowModal() 

    def ok(self,save=True):
        """closes dialog with ok result"""
        if (save): self.saveall()
        self.dlg.EndModal(wx.ID_OK)

    def cancel(self):
        """closes dialog with cancel result"""
        self.dlg.EndModal(wx.ID_CANCEL)

    def onendfinalsizer(self,sizer):
        sizer.Fit(self._getparent())

    def defokcancel(self):
        self.hbox(border=5,layoutflags=wx.CENTER)
        self.button(text='OK',event=lambda ev:self.ok())
        self.button(text='Storno',event=lambda ev:self.cancel())
        self.endsizer()

        #def buttons(self,flags,border=0):
        #self._sizer(self.dlg.CreateStdDialogButtonSizer(flags),border)
        #self.endsizer()
