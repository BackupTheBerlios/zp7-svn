# -*- coding: UTF-8 -*-

import wx
import re

class SongCanvas:
  def text(self,x,y,text) : pass
  def font(self,font) : pass
  def line(self,x1,y1,x2,y2): pass


class DCCanvas(SongCanvas):
  dc=None
  def __init__(self,dc) : self.dc=dc
  def text(self,x,y,text) : self.dc.DrawText(text,x,y)
  def font(self,font) : self.dc.SetFont(font.getwxfont());self.dc.SetTextForeground(font.color)
  def line(self,x1,y1,x2,y2): print (x1,y1,x2,y2);self.dc.DrawLine(x1,y1,x2,y2)


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

  def draw(self,canvas):
    for fn,args in self.items : getattr(canvas,fn)(*args)


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
  #def __getattr__(self,name) : return getattr(self.canvas,name)