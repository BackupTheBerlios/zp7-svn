# -*- coding: UTF-8 -*-

import wx
import re

class SongCanvas:
  def text(self,x,y,text) : raise NotImplemented
  def font(self,font) : raise NotImplemented
  def line(self,x1,y1,x2,y2): raise NotImplemented
  def dynamic_text(self,x,y,fn): pass


class DCCanvas(SongCanvas):
  dc=None
  def __init__(self,dc) : self.dc=dc
  def text(self,x,y,text) : self.dc.DrawText(text,x,y)
  def font(self,font) : self.dc.SetFont(font.getwxfont());self.dc.SetTextForeground(font.color)
  def line(self,x1,y1,x2,y2): self.dc.DrawLine(x1,y1,x2,y2)
  def dynamic_text(self,x,y,fn): 
    try:
      self.dc.DrawText(unicode(fn()),x,y)
    except:
      pass

class MemoryCanvas(SongCanvas):
  items=[]
  
  def __init__(self):
    self.items=[]    
    
  def text(self,x,y,text):
    self.items.append(('text',(x,y,text))) 

  def font(self,font): 
    self.items.append(('font',(font,)))

  def line(self,x1,y1,x2,y2):
    self.items.append(('line',(x1,y1,x2,y2)))

  def dynamic_text(self,x,y,fn):
    self.items.append(('dynamic_text',(x,y,fn)))

  def draw(self,canvas):
    for fn,args in self.items : getattr(canvas,fn)(*args)


# class _SubCanvasProc:
#   name=''
#   subcanvas=None
#   def __init__(self,subcanvas,name):
#     self.name=name
#     self.subcanvas=subcanvas
# 
#   def __call__(self,*args):
#     getattr(subcanvas.canvas,self.name)(*args)

class SubCanvas(SongCanvas):
  canvas=None
  x=0
  y=0
  def __init__(self,canvas,x,y):
    self.canvas=canvas
    self.x=x
    self.y=y

  def text(self,x,y,text) : self.canvas.text(x+self.x,y+self.y,text)
  def font(self,font) : self.canvas.font(font)
  def line(self,x1,y1,x2,y2): self.canvas.line(x1+self.x,y1+self.y,x2+self.x,y2+self.y)
  def dynamic_text(self,x,y,fn) : self.canvas.dynamic_text(x+self.x,y+self.y,fn)
  
  #def __getattr__(self,name) : return _SubCanvasProc(self,name)
  