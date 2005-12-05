# -*- coding: UTF-8 -*-

import wx
import re
import utils
    
class SongFont:
  bold=False
  italic=False
  underline=False
  face=''
  size=10
  color='black'
  
  wxfont=None
  
  def __init__(self,**args):
    if args.has_key('bold') : self.bold=args['bold']
    if args.has_key('italic') : self.italic=args['italic']
    if args.has_key('underline') : self.underline=args['underline']
    if args.has_key('face') : self.face=args['face']
    if args.has_key('size') : self.size=args['size']
    if args.has_key('color') : self.color=args['color']

  def getwxfont(self):
    if not self.wxfont:
      self.wxfont=utils.wxfontfromdict(self.__dict__)
    return self.wxfont  

  def transform(self,dc):
    px,py=dc.GetPPI()
    sx,sy=wx.ScreenDC().GetPPI()
    self.size*=int(float(px+py)/float(sx+sy))