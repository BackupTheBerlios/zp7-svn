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