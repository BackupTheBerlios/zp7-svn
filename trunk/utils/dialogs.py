# -*- coding: UTF-8 -*-

import wx
import utils
import browse
import copy

def _addtolist(dst,src):
  item=src.getitem()
  if item:
    if item not in dst:
      dst.append(item)  

def edit_ordered_set(parent,subset,mainset,titlefunc,subsettitle,mainsettitle,title):
  mysubset=copy.copy(subset)
  brw=browse.DialogBrowse(parent,title)
  brw.vbox()
  
  brw.hbox()
  brw.vbox(border=5)
  brw.label(text=subsettitle)
  brw.listbox(model=mysubset,id='sub',size=(100,200))
  brw.hbox(layoutflags=wx.CENTER)
  brw.button(text='<<',event=lambda ev:brw['sub'].moveup(),size=(30,-1))
  brw.button(text='>>',event=lambda ev:brw['sub'].movedown(),size=(30,-1))
  brw.endsizer()
  brw.endsizer()

  brw.vbox(border=5,layoutflags=wx.CENTER)
  brw.button(text='<',event=lambda ev:_addtolist(brw['sub'],brw['main']),size=(30,-1))
  brw.button(text='>',event=lambda ev:brw['sub'].eraseact(),size=(30,-1))
  brw.button(text='<<',event=lambda ev:brw['sub'].fill(mainset),size=(30,-1))
  brw.button(text='>>',event=lambda ev:brw['sub'].clear(),size=(30,-1))
  brw.endsizer()

  brw.vbox(border=5)
  brw.label(text=mainsettitle)
  brw.listbox(model=mainset,id='main',size=(100,200))
  brw.endsizer()
  
  brw.endsizer()
  #brw.buttons(wx.OK|wx.CANCEL,5)
  brw.hbox(border=5,layoutflags=wx.CENTER)
  brw.button(text='OK',event=lambda ev:brw.ok())
  brw.button(text='Storno',event=lambda ev:brw.cancel())
  brw.endsizer()
  brw.endsizer()
  
  res=brw.run()
  if res==wx.ID_OK:
    subset[:]=mysubset
    return True
    
  return False  