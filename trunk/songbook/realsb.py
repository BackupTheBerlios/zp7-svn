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


class RealSBType:
  sbtype=None
  dc=None # wx.DC; ask dc
  pgwi=0
  pghi=0
  pars=None
  bigw=0 #size of big page
  bigh=0
  pw100=0 #size of preview 100%
  ph100=0
  pkoefx=1.0 #koeficient
  pkoefy=1.0
  mmkx=1.0
  mmky=1.0
  
  dleft=0
  dtop=0
  dright=0
  dbottom=0
  
  _pgwi=0 #without substracted margins
  _pghi=0
  
  def __init__(self,sbtype,dc):
    self.dc=dc
    self.sbtype=sbtype
    self.pars=format.SongFormatPars()
    self.count()
  
  def count(self):
    self.bigw,self.bigh=self.dc.GetSize()

    px,py=self.dc.GetPPI()
    sx,sy=wx.ScreenDC().GetPPI()

    self._pgwi=self.bigw/self.sbtype.hcnt
    self._pghi=self.bigh/self.sbtype.vcnt

    self.mmkx=px/25.4
    self.mmky=py/25.4
    
    self.dtop=int(self.sbtype.topsp*self.mmkx)
    self.dleft=int(self.sbtype.leftsp*self.mmkx)
    self.dright=int(self.sbtype.rightsp*self.mmkx)
    self.dbottom=int(self.sbtype.bottomsp*self.mmkx)
    
    self.pgwi=self._pgwi-self.dleft-self.dright
    self.pghi=self._pghi-self.dtop-self.dbottom
    
    for f in sbtype.SBType.fontnames:
      self.pars.fonts[f]=format.SongFont(**self.sbtype.fonts[f])
      self.pars.fonts[f].transform(self.dc)
     
    self.pkoefx=float(sx)/float(px)
    self.pkoefy=float(sy)/float(py)
    self.pw100=int(self.bigw*self.pkoefx)
    self.ph100=int(self.bigh*self.pkoefy)
  
  def pageofs(self,x,y):
    return x*self._pgwi+self.dleft,y*self._pghi+self.dtop
    #res=x*self._pgwi+self.dleft,y*self._pghi+self.dtop
    #print res
    #return res
 
  def getfont(self,name):
    try:
      return self.pars.fonts[name]
    except: 
      return self.pars.fonts['default']