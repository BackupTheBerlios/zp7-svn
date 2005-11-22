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

class SongTable(dbgrid.DBTable):
  groupfilter=None
  cond=None
  
  def getbasecolumns(self):
    return [dbgrid.DbColumn('title',u"Název"),dbgrid.DbColumn('author',u"Autor"),dbgrid.DbColumn('group',u"Skupina")]
     
  def retrieve_data(self,columns):
    conds=[]
    sqlpars={}
    if self.cond:
      sql,pars=self.cond
      sqlpars.update(pars)
      conds.append(' ('+sql+') ')
    if self.groupfilter:
      conds.append(' (groupid=:group) ')
      sqlpars['group']=self.groupfilter
#       print sql
#       print sqlpars
#       print args
      #return songdb.DBSong.getlist(self.db,columns,'s.groupid=?', groupfilter=self.groupfilter,condition=sql,sqlarguments=sqlpars,**args)
      #return songdb.DBSong.getlist(self.db,columns)
    if conds:
      return songdb.DBSong.getlist(self.db,columns,' AND '.join(conds),sqlpars)
    else:
      return songdb.DBSong.getlist(self.db,columns)
      #return songdb.DBSong.getlist(self.db,columns,groupfilter=self.groupfilter)
    
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
