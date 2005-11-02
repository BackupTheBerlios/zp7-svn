# -*- coding: UTF-8 -*-

import wx
import re

import panes
import canvas as canvasmod

class SongFormatPars:
  """
  @ivar fonts: dict(str:SongFont), keys=title|text|label|chord
  """
  fonts={}
  
  def __init__(self):
    self.fonts={}

  def transformfonts(self,dc):
    for f in self.fonts:
      self.fonts[f].transform(dc)


class SongFormatter:
  """Algorithm, which formats song text
  
  @ivar pars: SongFormatPars
  """
  panegrp=None
  #lastwaslabel=False
  actpane=None
  actx=0
  askdc=None
  left=0
  spwi=0
  pars=None
  text=""
  labwi=0
  pagewi=0
  pagehi=0
  chord_word_re=re.compile(r'(\[[^]]*\])?([^[]*)')
  ahi=0
  athi=0
  thi=0
  labhi=0
  labelcache=None

  def setaskfont(self,font):
    self.askdc.SetFont(self.pars.fonts[font].getwxfont())
  
  def wantpane(self,hi):
    if not self.actpane:
      self.actpane=self.panegrp.addpane()
    if hi>self.actpane.h : self.actpane.h=hi
  
  def writetextword(self,text):
    self.setaskfont('text')
    if self.actx>self.labwi : self.actx+=self.spwi
    self.wantpane(self.thi)
    w,h=self.askdc.GetTextExtent(text)
    self.actpane.canvas.font(self.pars.fonts['text'])
    self.actpane.canvas.text(self.actx,0,text)
    self.actx+=w
    
  def linefeed(self):
    self.actpane=None
    self.actx=self.labwi
  
  def writelabel(self,label):
    self.flushlabel(self.labhi)
    self.labwi=0
    #if (self.lastwaslabel) : self.linefeed()
    self.labelcache=label
    self.setaskfont('label')
    self.wantpane(self.labhi)
    self.labwi=self.askdc.GetTextExtent(label)[0]+self.spwi
    #w,h=self.write(label,'label')
    #self.lastwaslabel=True
    #self.labwi=w+self.spwi
    #self.actx=self.labwi

  def gettextwi(self,text,font):
    self.setaskfont(font)
    return self.askdc.GetTextExtent(text)[0]
    

  def fmtacword(self,wrd):
    canvas=canvasmod.MemoryCanvas()
    parts=self.chord_word_re.findall(wrd) # ((chord1,part1),(chord2,part2)...)
    apos=0
    tpos=0
    for chord,part in parts:
      chord=chord[1:-1] #odstranit hranate zavorky okolo akordu
      if chord!="":
        if tpos<apos: tpos=apos
        canvas.font(self.pars.fonts['chord'])
        canvas.text(apos,0,chord)
        apos+=self.gettextwi(chord+' ','chord')
      if part!="":
        canvas.font(self.pars.fonts['text'])
        canvas.text(tpos,self.ahi,part)
        tpos+=self.gettextwi(part,'text')
    wi=max(apos,tpos)
    return (canvas,wi)

  def fmtacline(self,line):
    self.flushlabel(self.athi)
    words=line.split()
    for wrd in words:
      cnv,w=self.fmtacword(wrd)
      if self.actx>self.labwi and self.actx+w>self.pagewi : self.linefeed()
      self.wantpane(self.athi)
      cnv.draw(canvasmod.SubCanvas(self.actpane.canvas,self.actx,0))
      self.actx+=w+self.spwi
    
  def fmttextline(self,line):
    self.flushlabel(self.thi)
    self.setaskfont('text')
    words=line.split()
    for wrd in words:
      w,h=self.askdc.GetTextExtent(wrd)
      if self.actx>self.labwi and self.actx+w>self.pagewi : self.linefeed()
      self.writetextword(wrd)
  
  def flushlabel(self,linehi):
    if self.labelcache:
      self.wantpane(self.labhi)
      self.actpane.canvas.font(self.pars.fonts['label'])
      self.actpane.canvas.text(0,linehi-self.labhi,self.labelcache)
      self.labelcache=None
      
      
  def __init__(self,askdc,pars,text,pagewi):
    """
  
    @rtype: L{PaneGrp}
    """
    self.askdc=askdc
    self.text=text
    self.pars=pars
    self.pagewi=pagewi
    self.panegrp=panes.PaneGrp()
    #self.lastwaslabel=False
    self.actpane=None
    self.actx=0
    self.left=0
    self.setaskfont('text')
    self.spwi=self.askdc.GetTextExtent(' ')[0]
    self.thi=self.askdc.GetTextExtent('M')[1]
    self.setaskfont('chord')
    self.ahi=self.askdc.GetTextExtent('M')[1]
    self.athi=self.ahi+self.thi
    self.setaskfont('label')
    self.labhi=self.askdc.GetTextExtent('M')[1]
    
  def run(self):
    lines=self.text.split("\n")
    for line in lines:
      if len(line)>0 and line[0]=='.':
        self.writelabel(line[1:])
        continue
      self.actx=self.labwi
      if line.find('[')>=0:
        self.fmtacline(line)
      else:
        self.fmttextline(line)
      self.linefeed()
      
    self.flushlabel(self.labhi)
      
    for pane in self.panegrp.panes : self.pagehi+=pane.h