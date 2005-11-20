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
import copy
import interop
from utils.xmlnode import XmlNode

class SongDB:
  name=""
  con=None
  cur=None
  song_nametofld={'id':'s.id','title':'s.title','author':'s.author','group':'g.name','text':'t.songtext','groupid':'s.groupid'}
  group_nametofld={'id':'g.id','name':'g.name','serverid':'g.serverid','url':'g.url'}
  server_nametofld={'id':'r.id','url':'r.url','login':'r.login','password':'r.password','type':'r.type'}
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
        "groupid" INT,
        "author" VARCHAR(100),
        "remark" VARCHAR(100),
        "netmodified" INT
      );""")
    self.cur.execute("""
      CREATE UNIQUE INDEX "songs_id" ON "songs" ("id");
      """)
    self.cur.execute("""
      CREATE INDEX "songs_netid" ON "songs" ("netid");
      """)
    self.cur.execute("""
      CREATE TABLE "songtexts" (
        "songid" INTEGER,
        "songtext" TEXT
      );""")
    self.cur.execute("""
      CREATE UNIQUE INDEX "songtexts_id" ON "songtexts" ("songid");
      """)
    self.cur.execute("""
      CREATE TABLE "songsearchtexts" (
        "songid" INTEGER,
        "searchtext" TEXT
      );""")
    self.cur.execute("""
      CREATE UNIQUE INDEX "songsearchtexts_id" ON "songsearchtexts" ("songid");
      """)
    self.cur.execute("""
      CREATE TABLE "groups" (
        "id" INTEGER PRIMARY KEY,
        "netid" INT,
        "name" VARCHAR(100),
        "url" VARCHAR(100),
        "lang" CHAR(2),
        "serverid" INTEGER
      );
      """)
    self.cur.execute("""
      CREATE TABLE "servers" (
        "id" INTEGER PRIMARY KEY,
        "type" VARCHAR(10),
        "url" VARCHAR(100),
        "login" VARCHAR(50),
        "password" VARCHAR(50)
      );
      """)
    self.cur.execute("""
      CREATE UNIQUE INDEX "groups_id" ON "groups" ("id");
      """)
    self.cur.execute("""
      CREATE INDEX "groups_netid" ON "groups" ("netid");
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
  
  def addgroup(self,name,url="",netid=0,server=0):
    self.wantcur()
    self.cur.execute("INSERT INTO groups (id,name,netid,url,serverid) VALUES (NULL,?,?,?,?)",(name.encode("utf-8"),int(netid),url.encode("utf-8"),int(server)))

  def getsongbyid(self,songid,fields):
    """return sequence of song values
    
    @type fields: sequence(str)
    """
    self.wantcur()
    flds=[self.song_nametofld[fld] for fld in fields]
    self.cur.execute("SELECT "+",".join(flds)+" FROM songs s LEFT JOIN groups g ON (s.groupid=g.id) LEFT JOIN songtexts t ON (s.id=t.songid) WHERE s.id=?",(songid,))
    return self.cur.fetchone()

  def getserverbyid(self,serverid,fields):
    """return sequence of server values
    
    @type fields: sequence(str)
    """
    self.wantcur()
    flds=[self.server_nametofld[fld] for fld in fields]
    self.cur.execute("SELECT "+",".join(flds)+" FROM servers r WHERE r.id=?",(serverid,))
    return self.cur.fetchone()

  def getgroupbyid(self,groupid,fields):
    """return sequence of group values
    
    @type fields: sequence(str)
    """
    self.wantcur()
    flds=[self.group_nametofld[fld] for fld in fields]
    self.cur.execute("SELECT "+",".join(flds)+" FROM groups g WHERE g.id=?",(groupid,))
    return self.cur.fetchone()

  def update_search_text(self,songid,title,author,groupid,songtext):
    self.wantgroupnames()
    searchtext=utils.make_search_text(title)+'|'+utils.make_search_text(author)+'|'+utils.make_search_text(self.groupnames[int(groupid)])+'|'+\
               utils.make_search_text(transpmod.deletechords(songtext))
    self.cur.execute("REPLACE INTO songsearchtexts (songid,searchtext) VALUES (?,?)",(songid,searchtext.encode("utf-8").encode("utf-8")))

  def addsong(self,title,groupid,author,songtext,netid=0):
    self.wantcur()
    if songtext==None : songtext=u''
    
    self.cur.execute("INSERT INTO songs (id,title,groupid,author,netid) VALUES (NULL,?,?,?,?)",
                     (title.encode("utf-8"),int(groupid),author.encode("utf-8"),int(netid)))
    songid=self.cur.lastrowid
    self.cur.execute("INSERT INTO songtexts (songid,songtext) VALUES (?,?)",(songid,songtext.encode("utf-8")))
    self.update_search_text(songid,title,author,int(groupid),songtext)

  def __unicode__(self) : return unicode(self.name+"."+self.getext())
  
  def getsongsby(self,order='id',columns=['id'],groupfilter=None,condition='',jointexts=False,sqlarguments={}):
    self.wantcur()
    flds=[self.song_nametofld[fld] for fld in columns]
    where=''
    if groupfilter is not None: where=' WHERE (s.groupid=%d)' % groupfilter
    if condition:
      if where: where=where+' AND '+condition
      else: where=' WHERE '+condition
    if jointexts: jointexts='LEFT JOIN songtexts t ON (t.songid=s.id)'
    else: jointexts=''
    self.cur.execute(
        "SELECT %s FROM songs s LEFT JOIN groups g ON (s.groupid=g.id) %s %s ORDER BY %s" 
      % 
        (",".join(flds),jointexts,where,self.song_nametofld[order]),
      sqlarguments)
    return iter(self.cur)

  def getgroupsby(self,order,fields):
    self.wantcur()
    flds=[self.group_nametofld[fld] for fld in fields]
    self.cur.execute("SELECT %s FROM groups g ORDER BY %s" % (",".join(flds),self.group_nametofld[order]))
    return iter(self.cur)

  def getserversby(self,order,fields):
    self.wantcur()
    flds=[self.server_nametofld[fld] for fld in fields]
    self.cur.execute("SELECT %s FROM servers r ORDER BY %s" % (",".join(flds),self.server_nametofld[order]))
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
        #self.cur.execute("SELECT s.id,s.title,s.author,g.name,s.songtext FROM songs s LEFT JOIN groups g ON (s.groupid=g.id)")
        #for row in self.cur:
        #  text=utils.make_search_text(row[1])+'|'+utils.make_search_text(row[2])+'|'+utils.make_search_text(row[3])+'|'+\
        #       utils.make_search_text(transpmod.deletechords(row[4]))
        #  self.searchtexts[int(row[0])]=text
      finally:
        dlg.Destroy()
            
    return self.searchtexts

  def updatesong(self,songid,changed):
    self.wantcur()
    if 'text' in changed:
      self.cur.execute('REPLACE INTO songtexts (songid,songtext) VALUES (?,?)',(songid,changed['text']))      
    changedattrs=[fld for fld in changed if self.song_nametofld[fld].startswith('s.')]
    fields=[self.song_nametofld[fld][2:] for fld in changedattrs]
    values=[changed[fld] for fld in changedattrs]
    lst=['"%s"=?' % fld for fld in fields]+['netmodified=1']
    query='UPDATE songs SET %s WHERE id=?' % ','.join(lst)
    self.cur.execute(query,values+[songid])

  def enumgroups(self):
    groups=list(self.getgroupsby('id',('id','name','serverid')))
    return [DBGroup(self,g[0],{'name':g[1],'serverid':g[2]}) for g in groups]

  def enumservers(self):
    groups=list(self.getserversby('id',('id','url','login','password','type')))
    return [DBServer(self,g[0],{'url':g[1],'login':g[2],'password':g[3],'type':g[4]}) for g in groups]

  def compile_update_xml(self,serverid):
    xml=XmlNode('update')
    self.wantcur()
    self.cur.execute("""SELECT s.id,s.netid,s.title,s.author,s.groupid,t.songtext FROM songs s
                        LEFT JOIN songtexts t ON (s.id=t.songid) LEFT JOIN groups g ON (s.groupid=g.id)
                        WHERE s.netmodified=1 AND g.serverid=%s""" % serverid)
    for id,netid,title,author,groupid,text in self.cur:
      if netid: 
        x=xml.add('updatesong')
        x['id']=netid
      else: 
        x=xml.add('addsong')
      x['localid']=id      
      x['title']=title
      x['author']=author
      x['group_id']=groupid
      x.text=text
    return xml
                     

class InetSongDB(SongDB):
  def getext(self) : return "idb"

  def _download_from_inet(self,dlg,server,serverid):
    dlg.Update(10, u"Stahuji databázi ze serveru %s" % server)
    s=server.download_db()
          
    dlg.Update(30, u"Analyzuji databázi ze serveru %s" % server)
    doc=etree.parse(StringIO.StringIO(s))
    
    dlg.Update(40, u"Vkládám skupiny do databáze")
    for node in doc.xpath('//database/groups/group'):
      attr=node.attrib
      self.addgroup(attr['name'],attr['url'],attr['id'],serverid)
    
    self.cur.execute("SELECT id,netid FROM groups")
    netidtoid={}
    for (id,netid) in self.cur : netidtoid[int(netid)]=int(id)
    netidtoid[0]=0
      
    dlg.Update(50, u"Vkládám písně do databáze")
    for node in doc.xpath('//database/songs/song'):
      attr=node.attrib
      self.addsong(attr['title'],netidtoid[int(attr['group_id'])],attr['author'],node.text,attr['id'])

    dlg.Update(90, u"Vkládám písně do databáze")
    self.cur.execute('VACUUM')
    
    self.commit()          

  def _insert_server(self,server):
    self.wantcur()
    self.cur.execute('INSERT INTO servers (type,url,login,password) VALUES (?,?,?,?)',
      (server.server_type.name,server.url,server.login,server.password)
    )
    return self.cur.lastrowid

class LocalSongDB(SongDB):
  def getext(self) : return "ldb"

class DBGroup:
  db=None
  groupid=0
  vals={}
  attrnames=('name','serverid')
  
  def __init__(self,db,groupid,vals=None):
    self.db=db
    self.groupid=groupid
    if vals:
      self.vals=vals
    else:
      self.vals=dict(zip(self.attrnames,db.getgroupbyid(groupid,self.attrnames)))

  def __getattr__(self,name): 
    if (not name.startswith('__')) and (not name.endswith('__')) and self.vals.has_key(name):
      return self.vals[name]
    return object.__getattr__(self,name)

  def __unicode__(self): return self.name
  def __cmp__(self,other): return cmp(self.groupid,other.groupid)
  def __hash__(self): return hash(self.groupid)

class DBSong(object):
  db=None
  songid=0
  vals={}
  dbvals={} # original values stored in database
  attrnames=('title','author','group','text','groupid')
  
  def __init__(self,db,songid):
    self.db=db
    self.songid=songid
    self.vals=dict(zip(self.attrnames,db.getsongbyid(songid,self.attrnames)))
    self.dbvals=copy.copy(self.vals)

  def __getattr__(self,name): 
    if (not name.startswith('__')) and (not name.endswith('__')) and self.vals.has_key(name):
      return self.vals[name]
    return object.__getattr__(self,name)

  def __setattr__(self,name,value): 
    if name in self.attrnames:
      self.vals[name]=value
    else:
      object.__setattr__(self,name,value)

  def dbidtuple(self):
    return (str(self.db.name),int(self.songid))

  def commit(self,commitdb=True):
    """writes changes to database"""
    changed={}
    for v in self.vals:
      if self.vals[v]!=self.dbvals[v]:
        changed[v]=self.vals[v]
    if changed:
      self.db.updatesong(self.songid,changed)
      self.db.update_search_text(self.songid,self.title,self.author,int(self.groupid),self.text)
      self.dbvals=copy.copy(self.vals)
      if commitdb: self.db.commit()

  def setgroupobj(self,value): self.groupid=value.groupid
  groupobj=property(lambda self:DBGroup(self.db,self.groupid),setgroupobj)
  
class DBObject(object):
  db=None
  id=0
  vals={}
  attrnames=() # to be overwrite
  getfuncname=''

  def __init__(self,db,id,vals=None):
    self.db=db
    self.id=id
    if vals:
      self.vals=vals
    else:
      self.vals=dict(zip(self.attrnames,getattr(db,self.getserverbyid)(id,self.attrnames)))

  def __cmp__(self,other): return cmp(self.groupid,other.groupid)
  def __hash__(self): return hash(self.groupid)
  
  def __getattr__(self,name): 
    if name in self.attrnames:
      return self.vals[name]
    else:
      return object.__getattr__(self,name)

  def __setattr__(self,name,value): 
    if name in self.attrnames:
      self.vals[name]=value
    else:
      object.__setattr__(self,name,value)
  
  
class DBServer(DBObject):
  attrnames=('url','login','password','type')
  getfuncname='getserverbyid'

  def __unicode__(self): return self.name
  def iserver(self):
    res=interop.anchor['servertype'].find(self.type).create()
    res.url=self.url
    res.login=self.login
    res.password=self.password
    return res
