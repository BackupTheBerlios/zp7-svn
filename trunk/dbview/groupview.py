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
import songgrid
import browse
from database import songdb

class GroupTable(dbgrid.DBTable):
  def getbasecolumns(self):
    return [dbgrid.DbColumn('name',u"Jméno"),dbgrid.DbColumn('serverid',u"Server"),dbgrid.DbColumn('url',u"URL")]
     
  def retrieve_data(self,columns):
    return songdb.DBGroup.getlist(self.db,columns)


class GroupGrid(dbgrid.DBGrid):
  configxml=config.xml/'dbview'/'groupgrid'

  def __init__(self,parent):
    dbgrid.DBGrid.__init__(self,parent,GroupTable)

class GroupSongGrid(wx.SplitterWindow): #navenek se chova jako songgrid
  groups=None
  songs=None
  cursong=None
  curgroup=None
  onrowclick=None
  db=None
  
  def __init__(self,parent):
    wx.SplitterWindow.__init__(self,parent,-1)
    
    self.groups=GroupGrid(self)
    self.groups.configxml=config.xml/'dbview'/'gv_groupgrid'
    self.songs=songgrid.SongGrid(self)
    self.songs.configxml=config.xml/'dbview'/'gv_songgrid'
    
    self.SetMinimumPaneSize(100)
    self.SplitVertically(self.groups,self.songs)
    
    self.groups.onrowclick=self.ongroupclick
    self.songs.onrowclick=self.onsongclick
    
  def set_data(self,db):
    self.groups.set_data(db)
    self.songs.set_data(db)
    self.db=db

  def ongroupclick(self,db,groupid):
    group=db.group(groupid)
    self.songs.setgroupfilter(groupid)
    self.curgroup=group
    
  def onsongclick(self,db,songid):
    if self.onrowclick: self.onrowclick(db,songid)
    self.cursong=db[songid]

  def add_column(self,col):
    return self.songs.add_column(col)
  
  def remove_column(self,col):
    self.songs.remove_column(col)

  def getcurdbtuple(self):
    return self.songs.getcurdbtuple()

  def setcurid(self,songid):
    song=self.db[songid]
    self.curgroup=self.db.group(song.groupid)
    self.groups.setcurid(song.groupid)
    self.songs.setgroupfilter(song.groupid,immediately=True)
    self.songs.setcurid(songid)
    self.cursong=song
  
  def reload(self):
    xxx,cursong=self.getcurdbtuple()
    self.songs.reload()
    self.groups.reload()
    if cursong>=0: self.setcurid(cursong)
   

def addgroupdialog(db):   
  server=browse.var(songdb.EmptyDBObject())
  name=browse.var('??')
  url=browse.var('')
  brw=browse.DialogBrowse(desktop.main_window,u'Přidat skupinu')
  brw.vbox()
  brw.grid(rows=2,cols=2,border=5)
  brw.label(text=u'Server (už nepůjde měnit)')
  brw.combo(model=([songdb.EmptyDBObject()]+db.enumservers()),valuemodel=server)
  brw.label(text=u'Jméno')
  brw.edit(model=name)
  brw.label(text=u'Web')
  brw.edit(model=url)
  brw.endsizer()
  brw.defokcancel()
  brw.endsizer()
  if brw.run()==wx.ID_OK:
    db.addgroup(name.get(),url=url.get(),server=server.get().id)
    db.commit()
    interop.send_flag('reloaddb')

def editgroupdialog(group):
  brw=browse.DialogBrowse(desktop.main_window,u'Upravit skupinu')
  brw.vbox()
  brw.grid(rows=2,cols=2,border=5)
  brw.label(text=u'Jméno')
  brw.edit(model=browse.attr(group,'name'))
  brw.label(text=u'Web')
  brw.edit(model=browse.attr(group,'url'))
  brw.endsizer()
  brw.defokcancel()
  brw.endsizer()
  if brw.run()==wx.ID_OK:
    group.commit()
    interop.send_flag('reloaddb')
 