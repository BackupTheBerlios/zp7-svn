import wx
from ext import Browse

class ToolbarBrowse(Browse):
    def __init__(self,parent=None):
        Browse.__init__(self,parent)
        self.hbox()

    def realize(self):
        self.endsizer()
