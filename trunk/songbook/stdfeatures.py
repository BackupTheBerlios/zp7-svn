# -*- coding: UTF-8 -*-

import interop
import anchors.songformat as songformat

def printtext(text,font,panegrp,realsb):
  pane=panegrp.addpane()
  realsb.dc.SetFont(realsb.getfont(font).getwxfont())
  w,h=realsb.dc.GetTextExtent(text)
  pane.hi=h
  pane.canvas.font(realsb.getfont(font))
  pane.canvas.text(0,0,text)

class StdHeader(songformat.IHeader):
  name='std'
  
  def get_title(self): return u'Název+Autor'

  def printheader(self,song,panegrp,realsb):
    printtext(song.title,'title',panegrp,realsb)
    printtext(song.author,'author',panegrp,realsb)

class OnlyTitleHeader(songformat.IHeader):
  name='title'
  
  def get_title(self): return u'Název'

  def printheader(self,song,panegrp,realsb):
    printtext(song.title,'title',panegrp,realsb)


stdhdr=StdHeader()
interop.anchor['songheader'].add_feature(stdhdr)
interop.anchor['songheader'].set_default(stdhdr)

interop.anchor['songheader'].add_feature(OnlyTitleHeader())


class StdDistribAlg(songformat.IDistribAlg):
  name='normal'  
  def get_title(self): return u'Normální rozdělení'
  def creator(self,logpages,panegrps):
    import autodistrib
    return autodistrib.AutoDistribAlg(logpages,panegrps)

class SimpleDistribAlg(songformat.IDistribAlg):
  name='simple'
  def get_title(self): return u'Jednoduché rozdělení'
  def creator(self,logpages,panegrps):
    import autodistrib
    return autodistrib.SimpleDistribAlg(logpages,panegrps)

stddistribalg=StdDistribAlg()
interop.anchor['distribalg'].add_feature(stddistribalg)
interop.anchor['distribalg'].set_default(stddistribalg)

interop.anchor['distribalg'].add_feature(SimpleDistribAlg())


class StdDelimiter(songformat.ISongDelimiter):
  name='std'
  
  def get_title(self): return u'Jednoduchá čára'

  def printdelimiter(self,panegrp,realsb):
    pane=panegrp.addpane()
    realsb.dc.SetFont(realsb.getfont('text').getwxfont())
    w,h=realsb.dc.GetTextExtent('M')
    pane.hi=h*2
    pane.canvas.line(0,h,realsb.pgwi,h)


stddelim=StdDelimiter()
interop.anchor['songdelimiter'].add_feature(stddelim)
interop.anchor['songdelimiter'].set_default(stddelim)

