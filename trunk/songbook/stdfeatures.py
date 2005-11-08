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


stdhdr=StdHeader()
interop.anchor['songheader'].add_feature(stdhdr)
interop.anchor['songheader'].set_default(stdhdr)
