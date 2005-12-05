# -*- coding: UTF-8 -*-

import wx
import wx.grid as gridlib
import locale
import utils
import searchwinu
import utils.dialogs
import copy
import desktop
import config
import interop
import dbgrid
from database import songdb
import interop
import anchors.internet
import browse

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
    
def addserverdialog(db):
  server_type=browse.var(interop.anchor['servertype'].default)
  brw=browse.DialogBrowse(desktop.main_window,u'Konfigurace serverů')
  brw.vbox(border=5)
  brw.label(text=u'Typ serveru:')
  brw.combo(model=list(interop.anchor['servertype']),valuemodel=server_type,autosave=True)
  brw.defokcancel()
  brw.endsizer()
  if brw.run()==wx.ID_OK:
    songdb.DBServer.insertobject(db,{'type':server_type.get().name})
    db.commit()
    interop.send_flag('reloaddb')

def editserverdialog(server):
  server_object=server.iserver()
  if server_object.edit():
    server.assign(server_object)
    server.commit()
    interop.send_flag('reloaddb')

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