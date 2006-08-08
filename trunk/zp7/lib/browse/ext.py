import base
import utils
import wx

class _FontEditor:
    fontdict={}
    def __init__(self,fontdict): self.fontdict=fontdict
    def __call__(self,ev): _editfont(ev,self.fontdict)

def _editfont(ev,font):
    if utils.editfont(font):
        ev.GetEventObject().SetFont(utils.wxfontfromdict(font))
        ev.GetEventObject().SetForegroundColour(font['color'])


class FontExtension:
    def font(self,**kw):
        kw,mypars=self._split_args(kw,base._ctrlpars({'font':{}}))
        kw['event']=_FontEditor(mypars['font'])
        res=self.button(**kw)
        res.ctrl.SetFont(utils.wxfontfromdict(mypars['font']))
        res.ctrl.SetForegroundColour(mypars['font']['color'])

class ToolbarExtension:
    def toolbar(self,**kw):
        """returns toolbar browse

        @rtype: L{toolbar.ToolbarBrowse}
        """
        import toolbar
        panel=self.panel(size=(-1,25))
        self.endparent()
        panel.ctrl.Bind(wx.EVT_SIZE,lambda ev: panel.ctrl.Layout())
        #panel.load=lambda: panel.ctrl.Layout()
        return toolbar.ToolbarBrowse(panel.ctrl)


class PanelBrwExtension:
    def panelbrw(self):
        panel=self.panel()
        self.endparent()
        panel.ctrl.Bind(wx.EVT_SIZE,lambda ev: panel.ctrl.Layout())
        return Browse(panel.ctrl)


class Browse(base.BrowseBase,FontExtension,ToolbarExtension,PanelBrwExtension):
    pass
