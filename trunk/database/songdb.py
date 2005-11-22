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
    self.con=sqlite.connect(os.path.join(database.dbmanager.path,"%s.db" % self.name))

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
  
  def __unicode__(self) : return unicode(self.name+".db")

  def __getitem__(self,songid):
    return DBSong(self,songid)    

  def group(self,groupid):
    return DBGroup(self,groupid)

  def _download_from_inet(self,dlg,server,serverid):
    dlg.Update(10, u"Stahuji databázi ze serveru %s" % server)
    s=server.download_db()
          
    dlg.Update(30, u"Analyzuji databázi ze serveru %s" % server)
    doc=etree.parse(StringIO.StringIO(s))
    
    dlg.Update(40, u"Vkládám skupiny do databáze")
    for node in doc.xpath('//database/groups/group'):
      attr=node.attrib
      DBGroup.insertobject(self,{
        'name':attr['name'],
        'url':attr['url'],
        'netid':attr['id'],
        'serverid':serverid
      })
    
    self.cur.execute("SELECT id,netid FROM groups")
    netidtoid={}
    for (id,netid) in self.cur : netidtoid[int(netid)]=int(id)
    netidtoid[0]=0
      
    dlg.Update(50, u"Vkládám písně do databáze")
    for node in doc.xpath('//database/songs/song'):
      attr=node.attrib
      DBSong.insertobject(self,{
        'title':attr['title'],
        'groupid':netidtoid[int(attr['group_id'])],
        'author':attr['author'],
        'text':node.text,
        'netid':attr['id']
      })

    dlg.Update(90, u"Vkládám písně do databáze")
    self.cur.execute('VACUUM')
    
    self.commit()          

  def exporttoshare(self,file_name):
    self.wantcur()
    xml=XmlNode('database')
    grpxml=xml.add('groups')
    for id,name,url in DBGroup.getlist(self,('id','name','url')):
      g=grpxml.add('group')
      g['id']=id
      g['name']=name
      g['url']=url
    sngxml=xml.add('groups')
    for id,title,author,groupid,text in DBSong.getlist(self,('id','title','author','groupid','text')):
      s=sngxml.add('song')
      s['id']=id
      s['title']=title
      s['author']=author
      s['groupid']=groupid
      s.add('text').text=text
    xml.save(open(file_name,'w'))
    

class DBObject(object):
  db=None
  id=None # int
  vals={}
  dbvals={} # original values stored in database
  attrnames=() # to be overwrite
  tablename='' # to be overwrite
  tablechar='' # to be overwrite
  name_to_fld={} # to be overwrite, dict(field_name:field with selector)
  joins=[]
  querycache={}

  def __init__(self,db,id=None,vals=None):
    self.db=db
    self.id=id
    if vals:
      self.vals=vals
    elif id is not None:
      self.vals=dict(zip(self.attrnames,self.getdatabyid(db,id,self.attrnames)))
    else: # new inserted object
      self.vals=dict(zip(self.attrnames,[None]*len(self.attrnames)))
    self.dbvals=copy.copy(self.vals)

  def __cmp__(self,other): return cmp(self.id,other.id)
  def __hash__(self): return hash(self.id)
  
  def __getattr__(self,name): 
    if name in self.attrnames:
      return self.vals[name]
    else:
      return object.__getattribute__(self,name)

  def __setattr__(self,name,value): 
    if name in self.attrnames:
      self.vals[name]=value
    else:
      object.__setattr__(self,name,value)

  def dbidtuple(self):
    return (str(self.db.name),int(self.id))

  def commit(self,commitdb=True):
    """writes changes to database"""
    changed={}
    for v in self.vals:
      if self.vals[v]!=self.dbvals[v]:
        changed[v]=self.vals[v]
    if changed:
      self.updateobject(self.db,self.id,changed)
      self.dbvals=copy.copy(self.vals)
      if commitdb: self.db.commit()

  def insert(self):
    assert self.id is None
    changed=[v for v in self.vals if self.vals[v] is not None]
    self.id=self.insertobject(self.db,dict(zip(changed,[self.vals[v] for v in changed])))
    return self.id
  
  def assign(self,src):
    for a in self.attrnames:
      if hasattr(src,a):
        self.vals[a]=getattr(src,a)
  
  @classmethod
  def buildquery(self,fields,cond=None):
    querykey=(self,)+tuple(fields)
    try:
      return self.querycache[querykey]
    except:
      flds=[self.name_to_fld[fld] for fld in fields]
      query="SELECT "+",".join(flds)+" FROM %s %s " % (self.tablename,self.tablechar)
      for table,char,cond in self.joins: # add joins
        if len(filter(lambda f:f[0]==char,flds))>0 or (cond and char+'.' in cond): # only joins from which fields are required
          query+=(" LEFT JOIN %s %s ON (%s) " % (table,char,cond))
      self.querycache[querykey]=query # next time it would be quicker
      return query

  @classmethod
  def getdatabyid(self,db,id,fields):
    """return sequence of object values
    
    @type fields: sequence(str)
    """
    db.wantcur()
    query=self.buildquery(fields)
    query+="WHERE %s.id=?" % self.tablechar
    db.cur.execute(query,(id,))
    return db.cur.fetchone()

  @classmethod
  def updateobject(self,db,id,changed):
    """@param changed: dict of changed values"""
    db.wantcur()
    changedattrs=[fld for fld in changed if self.name_to_fld[fld].startswith(self.tablechar)]
    fields=[self.name_to_fld[fld][2:] for fld in changedattrs]
    values=[changed[fld] for fld in changedattrs]
    lst=['"%s"=?' % fld for fld in fields]
    if not lst: return
    query='UPDATE %s SET %s WHERE id=?' % (self.tablename,','.join(lst))
    db.cur.execute(query,values+[id])

  @classmethod
  def insertobject(self,db,values):
    """inserts new object into database
    
    @type value: dict(str:str)
    @return: id of new object
    """
    db.wantcur()
    attrs=[fld for fld in values if self.name_to_fld[fld].startswith(self.tablechar)]
    fields=[self.name_to_fld[fld][2:] for fld in attrs]
    values=[values[fld] for fld in attrs]
    
    query='INSERT INTO %s (id,%s) VALUES (NULL,%s)' % (self.tablename,','.join(fields),','.join(['?']*len(fields)))
    db.cur.execute(query,values)
    return db.cur.lastrowid

  @classmethod
  def getlist(self,db,fields,cond='',queryparams=[]):
    db.wantcur()
    query=self.buildquery(fields,cond)
    if cond.strip(): query+='WHERE '+cond
    db.cur.execute(query,queryparams)
    return iter(db.cur)
    
  @classmethod
  def enum(self,db,cond='',queryparams=[]):
    """@rtype: list of db objects"""
    lst=self.getlist(db,('id',)+self.attrnames,cond,queryparams)
    return [self(db,rec[0],dict(zip(self.attrnames,rec[1:]))) for rec in lst]
    
class EmptyDBObject(DBObject):
  def __unicode__(self): return u'Nic'

  def __init__(self):
    self.vals={}

class DBGroup(DBObject):
  attrnames=('name','serverid','url','netid')
  tablename='groups'
  tablechar='g'
  name_to_fld={'id':'g.id','name':'g.name','serverid':'g.serverid','url':'g.url','netid':'g.netid'}
  joins=[]

  def __unicode__(self): return self.name

class DBSong(DBObject):
  attrnames=('title','author','group','text','groupid','netid')
  tablename='songs'
  tablechar='s'
  name_to_fld={'id':'s.id','title':'s.title','author':'s.author','group':'g.name',
               'text':'t.songtext','groupid':'s.groupid','netid':'s.netid'}
  joins=[('groups','g','s.groupid=g.id'),('songtexts','t','s.id=t.songid')]


  @classmethod
  def insertobject(self,db,values):
    res=DBObject.insertobject.im_func(self,db,values)
    if values.has_key('text'):
      db.cur.execute('INSERT INTO songtexts (songid,songtext) VALUES (?,?)',(res,values['text']))
    return res

  @classmethod
  def updateobject(self,db,id,changed):
    DBObject.updateobject.im_func(self,db,id,changed)
    if changed.has_key('text'):
      db.cur.execute('REPLACE INTO songtexts (songid,songtext) VALUES (?,?)',(id,changed['text']))

  def setgroupobj(self,value): self.groupid=value.id
  groupobj=property(lambda self:DBGroup(self.db,self.groupid),setgroupobj)


class DBServer(DBObject):
  attrnames=('url','login','password','type')
  tablename='servers'
  tablechar='r'
  name_to_fld={'id':'r.id','url':'r.url','login':'r.login','password':'r.password','type':'r.type'}
  joins=[]

  def __unicode__(self): 
    if self.url:
      return self.url
    if self.type: return self.type
    return u'???'
    
  def iserver(self):
    res=interop.anchor['servertype'].find(self.type).create()
    res.url=self.url
    res.login=self.login
    res.password=self.password
    return res

