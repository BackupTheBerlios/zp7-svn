# -*- coding: UTF-8 -*-

import pysqlite2.dbapi2 as sqlite 
import StringIO
import os
import os.path
import sys
import wx
import lxml.etree as etree
import internet
import utils
import songtool.transp as transpmod
import database
import traceback
import desktop

class SongDB:
  name=""
  con=None
  cur=None
  song_nametofld={'id':'s.id','title':'s.title','author':'s.author','group':'g.name','text':'t.songtext','groupid':'g.id'}
  group_nametofld={'id':'g.id','name':'g.name'}
  searchtexts=None
  groupnames=None
  
  def __init__(self,name):
    self.name=name
    
  def __del__(self):
    if self.cur : self.con.rollback()
    if self.con : self.con.close()
    
  def commit(self):
    if not self.cur : return
    self.con.commit()
    self.cur=None

  def wantcur(self):
    if self.cur : return
    self.wantcon()
    self.cur=self.con.cursor()

  def wantcon(self):
    self.con=sqlite.connect(os.path.join(database.dbmanager.path,"%s.%s" % (self.name,self.getext()) ))

  def getext(self) : raise NotImplementedError()

  def wantgroupnames(self):
    if not self.groupnames:
      self.groupnames={0:""}
      self.wantcur()
      self.cur.execute("SELECT id,name FROM groups")
      for row in self.cur : self.groupnames[int(row[0])]=row[1]
    
  def _create(self):
    if self.cur : raise Exception("internal error")
    self.wantcur()
    self.cur.execute("""
      CREATE TABLE "songs" (
        "id" INTEGER PRIMARY KEY,
        "netid" INT,
        "title" VARCHAR(100),
        "group_id" INT,
        "author" VARCHAR(100),
        "remark" VARCHAR(100),
        "transp" INT,
        "marked" INT,
        "netmodified" INT,
        "nonet" INT,
        "special" VARCHAR(100)
      );""")
    self.cur.execute("""
      CREATE INDEX "songs_id" ON "songs" ("id");
      """)
    self.cur.execute("""
      CREATE INDEX "songs_netid" ON "songs" ("netid","id");
      """)
    self.cur.execute("""
      CREATE TABLE "songtexts" (
        "songid" INTEGER,
        "songtext" TEXT
      );""")
    self.cur.execute("""
      CREATE INDEX "songtexts_id" ON "songtexts" ("songid");
      """)
    self.cur.execute("""
      CREATE TABLE "songsearchtexts" (
        "songid" INTEGER,
        "searchtext" TEXT
      );""")
    self.cur.execute("""
      CREATE INDEX "songsearchtexts_id" ON "songsearchtexts" ("songid");
      """)
    self.cur.execute("""
      CREATE TABLE "groups" (
        "id" INTEGER PRIMARY KEY,
        "netid" INT,
        "name" VARCHAR(100),
        "url" VARCHAR(100),
        "lang" CHAR(2),
        "netmodified" INT,
        "nonet" INT,
        "special" VARCHAR(100)
      );
      """)
    self.cur.execute("""
      CREATE INDEX "groups_id" ON "groups" ("id");
      """)
    self.cur.execute("""
      CREATE INDEX "groups_netid" ON "groups" ("netid","id");
      """)
    self.cur.execute("""
      CREATE TABLE "info" (
        "id" INTEGER PRIMARY KEY,
        "name" VARCHAR(100),
        "value" TEXT
      );
      """)
    self.cur.execute("""
      CREATE INDEX "info_id" ON "info" ("id");  
        """)
    self.commit()
  
  def addgroup(self,name,url="",netid=0):
    self.wantcur()
    self.cur.execute("INSERT INTO groups (id,name,netid,url) VALUES (NULL,?,?,?)",(name.encode("utf-8"),int(netid),url.encode("utf-8")))

  def getsongbyid(self,songid,fields):
    """return sequence of song values
    
    @type fields: sequence(str)
    """
    self.wantcur()
    flds=[self.song_nametofld[fld] for fld in fields]
    self.cur.execute("SELECT "+",".join(flds)+" FROM songs s LEFT JOIN groups g ON (s.group_id=g.id) LEFT JOIN songtexts t ON (s.id=t.songid) WHERE s.id=?",(songid,))
    return self.cur.fetchone()

  def getgroupbyid(self,groupid,fields):
    """return sequence of group values
    
    @type fields: sequence(str)
    """
    self.wantcur()
    flds=[self.group_nametofld[fld] for fld in fields]
    self.cur.execute("SELECT "+",".join(flds)+" FROM groups g WHERE g.id=?",(groupid,))
    return self.cur.fetchone()

  def addsong(self,title,group_id,author,songtext,netid=0):
    self.wantgroupnames()
    self.wantcur()
    if songtext==None : songtext=u''
    searchtext=utils.make_search_text(title)+'|'+utils.make_search_text(author)+'|'+utils.make_search_text(self.groupnames[int(group_id)])+'|'+\
               utils.make_search_text(transpmod.deletechords(songtext))
    
    self.cur.execute("INSERT INTO songs (id,title,group_id,author,netid) VALUES (NULL,?,?,?,?)",
                     (title.encode("utf-8"),int(group_id),author.encode("utf-8"),int(netid)))
    songid=self.cur.lastrowid
    self.cur.execute("INSERT INTO songtexts (songid,songtext) VALUES (?,?)",(songid,songtext.encode("utf-8")))
    self.cur.execute("INSERT INTO songsearchtexts (songid,searchtext) VALUES (?,?)",(songid,searchtext.encode("utf-8").encode("utf-8")))

  def __unicode__(self) : return unicode(self.name+"."+self.getext())
  
  def getsongsby(self,order,fields,groupfilter=None):
    self.wantcur()
    flds=[self.song_nametofld[fld] for fld in fields]
    wheregroup=''
    if groupfilter is not None: wheregroup=' WHERE s.group_id=%d' % groupfilter
    self.cur.execute("SELECT %s FROM songs s LEFT JOIN groups g ON (s.group_id=g.id) %s ORDER BY %s" % (",".join(flds),wheregroup,self.song_nametofld[order]))
    return iter(self.cur)

  def getgroupsby(self,order,fields):
    self.wantcur()
    flds=[self.group_nametofld[fld] for fld in fields]
    self.cur.execute("SELECT %s FROM groups g ORDER BY %s" % (",".join(flds),self.group_nametofld[order]))
    return iter(self.cur)

  def __getitem__(self,songid):
    return DBSong(self,songid)    

  def group(self,groupid):
    return DBGroup(self,groupid)

  def getsearchtexts(self):
    if not self.searchtexts:
      try:
        dlg=wx.ProgressDialog(u"Zpěvníkátor",u"Indexuju písničky pro vyhledávání...",maximum=0,parent=utils.main_window)
        self.searchtexts={}
        self.wantcur()
        self.cur.execute("SELECT songid,searchtext FROM songsearchtexts")
        for row in self.cur : self.searchtexts[int(row[0])]=row[1]
        #self.cur.execute("SELECT s.id,s.title,s.author,g.name,s.songtext FROM songs s LEFT JOIN groups g ON (s.group_id=g.id)")
        #for row in self.cur:
        #  text=utils.make_search_text(row[1])+'|'+utils.make_search_text(row[2])+'|'+utils.make_search_text(row[3])+'|'+\
        #       utils.make_search_text(transpmod.deletechords(row[4]))
        #  self.searchtexts[int(row[0])]=text
      finally:
        dlg.Destroy()
            
    return self.searchtexts


class InetSongDB(SongDB):
  def getext(self) : return "idb"

  def _download_from_inet(self,dlg):
    dlg.Update(10, u"Stahuji databázi")
    s=internet.download_inet_db()
          
    dlg.Update(30, u"Analyzuji databázi")
    doc=etree.parse(StringIO.StringIO(s))
    
    dlg.Update(40, u"Vkládám skupiny do databáze")
    for node in doc.xpath('//database/groups/group'):
      attr=node.attrib
      self.addgroup(attr['name'],attr['href'],attr['id'])
    
    self.cur.execute("SELECT id,netid FROM groups")
    netidtoid={}
    for (id,netid) in self.cur : netidtoid[int(netid)]=int(id)
    netidtoid[0]=0
      
    dlg.Update(50, u"Vkládám písně do databáze")
    for node in doc.xpath('//database/songs/song'):
      attr=node.attrib
      self.addsong(attr['title'],netidtoid[int(attr['group_id'])],attr['author'],node.getchildren()[0].text,attr['id'])

    dlg.Update(90, u"Vkládám písně do databáze")
    self.cur.execute('VACUUM')
    
    self.commit()          

class LocalSongDB(SongDB):
  def getext(self) : return "ldb"

class DBGroup:
  db=None
  groupid=0
  vals={}
  attrnames=('name',)
  
  def __init__(self,db,groupid):
    self.db=db
    self.groupid=groupid
    self.vals=dict(zip(self.attrnames,db.getgroupbyid(groupid,self.attrnames)))

  def __getattr__(self,name): 
    if (not name.startswith('__')) and (not name.endswith('__')) and self.vals.has_key(name):
      return self.vals[name]
    return object.__getattr__(self,name)

class DBSong:
  db=None
  songid=0
  vals={}
  attrnames=('title','author','group','text','groupid')
  
  def __init__(self,db,songid):
    self.db=db
    self.songid=songid
    self.vals=dict(zip(self.attrnames,db.getsongbyid(songid,self.attrnames)))

  def __getattr__(self,name): 
    if (not name.startswith('__')) and (not name.endswith('__')) and self.vals.has_key(name):
      return self.vals[name]
    return object.__getattr__(self,name)

  def dbidtuple(self):
    return (str(self.db.name),int(self.songid))
