# -*- coding: utf-8 -*- 

import utils.xmltools as xmltools
import utils.xmlnode as xmlnode
import utils
import desktop
import browse
import config
import wx
import copy
import realsb
import format
import sbtype

class PagePrinter:
#canvas=None
    def getpagesize(self): return 0,0
    def beginpage(self): pass
    def endpage(self): pass
    def addpane_hint(self,pane): pass
    #def nextpage(self):
    #if self.canvas: self.endpage()
    #self.beginpage()

class LogPage:
    panes=[]
    canvas=format.MemoryCanvas()
    pagenum=0

    def __init__(self):
        self.panes=[]
        self.canvas=format.MemoryCanvas()

    def draw(self,canvas):
    #acty=0
    #for pane in self.pane: 
    #pane.canvas.draw(format.SubCanvas(canvas,0,acty))
    #acty+=pane.hi
        self.canvas.draw(canvas)

class LogPages(PagePrinter):
    pages=[]
    actpage=None
    pagesize=(0,0)  

    def __init__(self,pagesize):
        self.pages=[]
        self.actpage=None
        self.pagesize=pagesize

    def beginpage(self):
        self.actpage=LogPage()
        self.pages.append(self.actpage)
        self.actpage.pagenum=len(self.pages)
        return self.actpage.canvas

    def addpane_hint(self,pane):
        self.actpage.panes.append(pane)

    def endpage(self): 
        self.actpage=None

    def getpagesize(self): return self.pagesize

    def __len__(self): return len(self.pages)
    def __iter__(self): return iter(self.pages)
    def __getitem__(self,item): return self.pages[item]

# class DistribAlg:
#   """abstract class, which have to transform """
#   pass

# class DistribState:
#   acty=0
#   printer=None
#   canvas=None
#   pgwi=0
#   pghi=0
#   
#   def __init__(self,printer):
#     self.acty=0
#     self.printer=printer
#     self.pgwi,self.pghi=self.printer.getpagesize()
# 
#   def printpane(self,hi,callback):
#     """prints one pane
#     
#     @type callback: lambda canvas
#     """
#     if not self.canvas: self.beginpage()
#     elif self.acty+hi>self.pghi: self.nextpage()
#     callback(format.SubCanvas(self.canvas,0,self.acty))
#     self.acty+=hi
# 
#   def beginpage(self):
#     if self.canvas: return
#     self.acty=0
#     self.canvas=self.printer.beginpage()
# 
#   def endpage(self):
#     if not self.canvas: return
#     self.printer.endpage()
#     self.canvas=None
# 
#   def nextpage(self):
#     self.endpage()
#     self.beginpage()
# 
#   def close(self):
#     self.endpage()
#     self.__dict__.clear()
