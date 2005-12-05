# -*- coding: UTF-8 -*-

import wx
import utils

class SearchWindow(wx.MiniFrame):
  text=None
  ctrl=None
  
  def __init__(self,ev_to_emulate,ctrl,pos=wx.DefaultPosition):
    self.ctrl=ctrl
    
    style=wx.DEFAULT_FRAME_STYLE & (~wx.RESIZE_BORDER) | wx.STAY_ON_TOP
    wx.MiniFrame.__init__(self,utils.main_window,-1,u"Hledání",pos,wx.DefaultSize,style)
    self.text=wx.TextCtrl(self,-1,'',size=(125, -1)) 
    self.text.EmulateKeyPress(ev_to_emulate)
    self.Bind(wx.EVT_TEXT,self.OnText,self.text)
    self.text.Bind(wx.EVT_KEY_DOWN,self.OnKeyDown)

    prev=wx.Button(self,-1,"<<",size=(50, -1))
    self.Bind(wx.EVT_BUTTON,self.OnPrev,prev)
    next = wx.Button(self,-1,">>",size=(50, -1))
    self.Bind(wx.EVT_BUTTON,self.OnNext,next)
    self.Bind(wx.EVT_CLOSE, self.OnCloseWindow)
    
    sizer=wx.BoxSizer(wx.HORIZONTAL)
    sizer.Add(self.text,0,wx.EXPAND)
    sizer.Add(prev,0,wx.EXPAND)
    sizer.Add(next,0,wx.EXPAND)
    self.SetSizer(sizer)
    sizer.Fit(self)

  def OnPrev(self,ev):
    self.ctrl.qsearch(self.text.GetValue(),-1)
            
  def OnNext(self,ev):
    self.ctrl.qsearch(self.text.GetValue(),1)
  
  def OnCloseWindow(self,ev):
    self.Destroy()
    self.ctrl.destroysearchwin()
  
  def OnText(self,ev):
    ev.Skip()
    self.OnNext(ev)

  def OnKeyDown(self,ev):
    keycode=ev.GetKeyCode()
    if keycode==wx.WXK_UP:
      self.OnPrev(ev)
    elif keycode==wx.WXK_DOWN or keycode==wx.WXK_RETURN:
      self.OnNext(ev)
    elif keycode==wx.WXK_ESCAPE:
      self.Close(True)
    else:
      ev.Skip()
  