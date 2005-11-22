# -*- coding: UTF-8 -*-

import os
import os.path
import sys
import desktop
import wx

import intf
import songdb

# ext_to_db={
#   ".idb":songdb.InetSongDB,
#   ".ldb":songdb.LocalSongDB
# }

class SongDBManager(intf.IDBManager):
  dbs={}
  path=os.path.join(os.path.dirname(os.path.dirname(sys.argv[0])),"db")
  
  def __init__(self):
    self.dbs={}
    self.refresh()
  
  def refresh(self):
    for f in os.listdir(self.path):
      name,ext=os.path.splitext(f)
      name=name.lower()
      ext=ext.lower()
      if self.dbs.has_key(name) : continue
      if ext=='.db':
        self.dbs[name]=songdb.SongDB(name)
#       if ext_to_db.has_key(ext):
#         self.dbs[name]=ext_to_db[ext](name)
  
  def create_inet_db(self,name,servers):
    try:
      dlg=wx.ProgressDialog(u"Stahování databáze",u"Vytvářím databází",maximum=100,parent=desktop.main_window)
      if self.dbs.has_key(name) : raise Exception("Duplicate database name")
      db=songdb.SongDB(name)
      self.dbs[name]=db
      db._create()
      for server in servers:
        srv=songdb.DBServer(db)
        srv.assign(server)
        serverid=srv.insert()        
        #serverid=db._insert_server(server)
        db._download_from_inet(dlg,server,serverid)
      return db
    finally:
      dlg.Destroy()
  
  def sortedkeys(self):
    keys=self.dbs.keys()
    keys.sort()
    return keys
    
  
  def __iter__(self):
    for key in self.sortedkeys() : yield self.dbs[key]
  
  def __getitem__(self,index) : return self.dbs[self.sortedkeys()[index]]