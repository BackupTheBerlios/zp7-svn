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

class SongTable(dbgrid.DBTable):
  groupfilter=None
  cond=None
  
  def getbasecolumns(self):
    return [dbgrid.DbColumn('title',u"Název"),dbgrid.DbColumn('author',u"Autor"),dbgrid.DbColumn('group',u"Skupina")]
     
  def retrieve_data(self,columns):
    if self.cond:
      sql,sqlpars,args=self.cond
      print sql
      print sqlpars
      print args
      return self.db.getsongsby(columns=columns,groupfilter=self.groupfilter,condition=sql,sqlarguments=sqlpars,**args)
    else:
      return self.db.getsongsby(columns=columns,groupfilter=self.groupfilter)
    
  def setgroupfilter(self,groupid,immediately=False):
    self.groupfilter=groupid
    self.fill_data(immediately)


class SongGrid(dbgrid.DBGrid):
  configxml=config.xml/'dbview'/'songgrid'

  def __init__(self,parent):
    dbgrid.DBGrid.__init__(self,parent,SongTable)

  def setgroupfilter(self,groupid,immediately=False):
    self.table.setgroupfilter(groupid,immediately)
    
  def setcond(self,cond):
    """@type cond: (sql, {sqlpars}, {getsongsby-keyword-params}"""
    self.table.cond=cond
    self.table.fill_data()

  def reload(self):
    xxx,cursong=self.getcurdbtuple()
    dbgrid.DBGrid.reload(self)
    if cursong>=0: self.setcurid(cursong)
