import wx

class ViewPanel(wx.Panel):
    #bg = wx.Brush('white')
    bmp = None
    
    def __init__(self, parent):
        wx.Panel.__init__(self, parent, -1)
        self.Bind(wx.EVT_PAINT, self.OnPaint)

    def OnPaint(self, evt):
        dc = wx.PaintDC(self)
        #dc.BeginPaint()
        dc.Clear()
        if self.bmp is not None:
            dc.DrawBitmap(self.bmp, 0, 0, False)
        
    def set_bitmap(self, bmp):
#         if self.bmp is not None:
#             self.bmp.Destroy()
        self.bmp = bmp
        self.Refresh()