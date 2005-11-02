# -*- coding: UTF-8 -*-

import wx
import re
import canvas as canvasmod

class Pane:
  """One no-splittable pane in output
  
  @ivar ops: list( tuple(op_func,op_args,op_str) )
  """
  h=0
  ops=[]
  canvas=canvasmod.MemoryCanvas()
  
  def __init__(self):
    self.ops=[]
    self.canvas=canvasmod.MemoryCanvas()

class PaneGrp:
  panes=[]
  
  def __init__(self):
    self.ops=[]
    self.panes=[]
  
  def addpane(self):
    res=Pane()
    self.panes.append(res)
    return res

  def draw(self,canvas):
    y=0
    for pane in self.panes:
      pane.canvas.draw(canvasmod.SubCanvas(canvas,0,y))
      y+=pane.h            
  
  def prndraw(self,state):
    for pane in self.panes: state.printpane(pane.h,pane.canvas.draw)
