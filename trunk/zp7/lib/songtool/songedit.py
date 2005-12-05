# -*- coding: UTF-8 -*-

import wx
import songtool.transp as transpmod
import format
import config
import desktop
import browse
import interop
from database import songdb


def editsong(song):
  brw=browse.DialogBrowse(desktop.main_window,u'Úprava písně')
  brw.vbox()
  brw.hbox(border=5)
  brw.label(text=u'Název:')
  brw.edit(model=browse.attr(song,'title'))
  brw.label(text=u'Autor:')
  brw.edit(model=browse.attr(song,'author'))
  brw.label(text=u'Skupina:')
  brw.combo(model=songdb.DBGroup.enum(song.db),valuemodel=browse.attr(song,'groupobj'))
  brw.endsizer()
  brw.memo(proportion=1,size=(-1,300),model=browse.attr(song,'text'))
  brw.hbox(border=5,layoutflags=wx.CENTER)
  brw.button(text='OK',event=lambda ev:brw.ok())
  brw.button(text='Storno',event=lambda ev:brw.cancel())
  brw.endsizer()
  brw.endsizer()
  if brw.run()==wx.ID_OK:
    song.commit()
    interop.send_flag('reloaddb')