# -*- coding: UTF-8 -*-

import wx
import songview
import songtool.toolbars
import utils
import printing

class ISongListCtrl:
  """interface common to songgrid, songgroupview..., describes selecting songs"""
  pass
  

class SongListCtrlAndSongView(wx.SplitterWindow):
  grid=None
  songv=None
  db=None
  
  def __init__(self,parent,grid_class):
    wx.SplitterWindow.__init__(self,parent,-1)
    
    self.grid=grid_class(self)
    self.songv=songview.SongCtrl(self)
    
    self.SetMinimumPaneSize(200)
    self.SplitVertically(self.grid, self.songv)
    
    self.grid.onrowclick=self.onsongclick
    
  def onsongclick(self,db,songid):
    song=db[songid]
    self.songv.setsong(song)
    self.cursong=song

  def set_data(self,db):
    self.db=db
    self.grid.set_data(db)
    
  def create_toolbar(self,obj):
    toolbar=obj.get_toolbar()
    evtbinder=obj.get_event_binder()
    songtool.toolbars.make_transp_toolbar(self.songv,evtbinder,toolbar)
    toolbar.AddSeparator()
    utils.wx_add_art_tool(toolbar,self.printsong,wx.ART_PRINT,evtbinder,u'Tisk písně',u'Vytiskne aktuální píseň')

  def create_menu(self,obj):
    songtool.toolbars.make_transp_menu(self.songv,obj)

  def printsong(self,event=None):
    if self.cursong: printing.printsong(self.cursong)

  def add_song_column(self,col):
    self.grid.add_column(col)
  
  def remove_song_column(self,col):
    self.grid.remove_column(col)

  def get_cur_song(self):
    """@rtype: (database,songid)"""
    return self.grid.getcurdbtuple()

  def set_cur_song(self,songid):
    """type songid: int"""
    self.grid.setcurid(songid)
 
  def setcond(self,cond):
    self.grid.setcond(cond)   
    