# -*- coding: UTF-8 -*-

import wx
import wx.grid as gridlib
import code
import locale
import utils
import searchwinu
import code
import utils.dialogs
import copy
import desktop
import config
import interop
import dbgrid
from database import songdb


class ServerTable(dbgrid.DBTable):
  groupfilter=None
  cond=None
  
  def getbasecolumns(self):
    return [dbgrid.DbColumn('url',u"URL"),dbgrid.DbColumn('login',u"Login"),dbgrid.DbColumn('type',u"Typ")]
     
  def retrieve_data(self,columns):
    return songdb.DBServer.getlist(self.db,columns)


class ServerGrid(dbgrid.DBGrid):
  configxml=config.xml/'dbview'/'servergrid'

  def __init__(self,parent):
    dbgrid.DBGrid.__init__(self,parent,ServerTable)

  def reload(self):
    xxx,curserver=self.getcurdbtuple()
    dbgrid.DBGrid.reload(self)
    if curserver>=0: self.setcurid(curserver)
    
# def addserverdialog(db):   
#   server=browse.var(songdb.EmptyDBObject())
#   name=browse.var('??')
#   url=browse.var('')
#   brw=browse.DialogBrowse(desktop.main_window,u'Přidat server')
#   brw.vbox()
#   brw.grid(rows=2,cols=2,border=5)
#   brw.label(text=u'Server (už nepůjde měnit)')
#   #print [songdb.EmptyDBObject()]+songdb.DBServer.enum(db)
#   brw.combo(model=([songdb.EmptyDBObject()]+songdb.DBServer.enum(db)),valuemodel=server)
#   brw.label(text=u'Jméno')
#   brw.edit(model=name)
#   brw.label(text=u'Web')
#   brw.edit(model=url)
#   brw.endsizer()
#   brw.defokcancel()
#   brw.endsizer()
#   if brw.run()==wx.ID_OK:
#     db.addgroup(name.get(),url=url.get(),server=server.get().id)
#     db.commit()
#     interop.send_flag('reloaddb')
# 
# def editserverdialog(group):
#   brw=browse.DialogBrowse(desktop.main_window,u'Upravit skupinu')
#   brw.vbox()
#   brw.grid(rows=2,cols=2,border=5)
#   brw.label(text=u'Jméno')
#   brw.edit(model=browse.attr(group,'name'))
#   brw.label(text=u'Web')
#   brw.edit(model=browse.attr(group,'url'))
#   brw.endsizer()
#   brw.defokcancel()
#   brw.endsizer()
#   if brw.run()==wx.ID_OK:
#     group.commit()
#     interop.send_flag('reloaddb')
