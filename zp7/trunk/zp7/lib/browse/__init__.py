import wx
import utils

class attr:
    obj=None
    name=''

    def __init__(self,obj,name):
        self.obj=obj
        self.name=name

    def get(self):
        return getattr(self.obj,self.name)

    def set(self,value):
        setattr(self.obj,self.name,value)

class var:
    obj=None

    def __init__(self,obj=None):
        self.obj=obj

    def get(self):
        return self.obj

    def set(self,value):
        self.obj=value

from ext import Browse
from dialog import DialogBrowse
from toolbar import ToolbarBrowse
