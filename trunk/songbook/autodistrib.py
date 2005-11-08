# -*- coding: UTF-8 -*-

import copy
import format

# class DistribAlg:
#   def run(panes,logpages):
#     """ runs distrib alg
#     
#     @type panes: list of list of pane
#     @type logpages: L{paging.LogPages}
#     """
    
class Pane:
  """one Pane, can be original pane objects, must follow this interface"""
  hi=0
  delim=False # if this pane should be first or last on page, it is not added to logpages
  def draw(self,canvas): pass

def countextrasheets(hi,hi0,maxhi,panegrp):
  res=0
  for pane in panegrp:
    if hi+pane.hi>maxhi:
      res+=1
      hi=0
      hi0=0
      
    hi+=pane.hi
    if not pane.delim: hi0=hi

  return res
    

class PaneGrp:
  panes=[]
  
  def __init__(self,panes=[]):
    self.panes=panes
    
  def __iter__(self):
    return iter(self.panes)

  def sheetcnt(self,maxhi):
    return countextrasheets(maxhi,maxhi,maxhi,self)

class LogPage:
  panes=[]
  hi=0 # actual height with last delim
  hi0=0 # actual height without last delim
  maxhi=0
  
  def __init__(self,maxhi):
    self.panes=[]
    self.maxhi=maxhi

  def __iter__(self):
    return iter(self.panes)

  def extrapagecount(self,panegrp):
    """returns pages count, which will be added, if adding panegrp, doesn't change anythink"""
    return countextrasheets(self.hi,self.hi0,self.maxhi,panegrp)

  def canaddpane(self,pane):
    if pane.delim: return True
    return self.hi+pane.hi<=self.maxhi
  
  def addpane(self,pane):
    self.panes.append(pane)
    self.hi+=pane.hi
    if not pane.delim: self.hi0=self.hi    
  
  def islast(self,pane): return self.panes and self.panes[-1]==pane
  def isfirst(self,pane): return self.panes and self.panes[0]==pane
    
class DistribAlg:
  panegrps=[]
  pages=[]
  maxhi=0
  printer=None
  firstleft=True

  def __init__(self,printer,panegrps):
    self.panegrps=panegrps
    self.pages=[]
    self.printer=printer
    self.maxhi=printer.getpagesize()[1]
    
  def addpage(self):
    self.pages.append(LogPage(self.maxhi))

  def isleftpage(self):
    index=len(self.pages)-1
    if not self.firstleft: index+=1
    return (index&1)==0
  
  def addpage(self):
    self.pages.append(LogPage(self.maxhi))
    
  def addpanegrp(self,panegrp):
    for pane in panegrp:
      if not self.pages[-1].canaddpane(pane):
        self.addpage()
      self.pages[-1].addpane(pane)
    
  def run(self):
    def addpanegrp(index):
      self.addpanegrp(panegrpstack[index])
      del panegrpstack[index]
  
    panegrpstack=copy.copy(self.panegrps)
    self.addpage() # first page
    wasaddedonleftpage=False
    
    while panegrpstack:
      rest=self.pages[-1].extrapagecount(panegrpstack[0])
      if self.isleftpage():
        if rest<=1:
          addpanegrp(0)
          wasaddedonleftpage=True
        else:
          if not wasaddedonleftpage:
            # smula,pisen se nevejde ani na dvojstranu, je potreba rozdelit
            addpanegrp(0)
          else:
            if panegrpstack[0].sheetcnt(self.maxhi)<=2:
              self.addpage()
              self.addpage()
              addpanegrp(0)
            else:
              addpanegrp(0)
      else: # right page
        if rest==0:
          addpanegrp(0)
          wasaddedonleftpage=True
        else:
          self.addpage()
          addpanegrp(0)

  def printpages(self):
    for page in self.pages:
      canvas=self.printer.beginpage()
      acty=0
      for pane in page:
        denydraw=False
        if pane.delim:
          if page.isfirst(pane) or page.islast(pane):
            denydraw=True
        if not denydraw:
          pane.draw(format.SubCanvas(canvas,0,acty))
          acty+=pane.hi
      self.printer.endpage()