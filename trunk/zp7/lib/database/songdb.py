# -*- coding: UTF-8 -*-

import pysqlite2.dbapi2 as sqlite 
from StringIO import StringIO
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
import dbobject

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

    def server(self,serverid):
        return DBServer(self,serverid)

    def importxml(self,xmldoc,serverid):
        """type xmldoc: etree.ElementTree"""
        for node in xmldoc.xpath('//database/groups/group'):
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

        for node in xmldoc.xpath('//database/songs/song'):
            attr=node.attrib
            DBSong.insertobject(self,{
                'title':attr['title'],
                'groupid':netidtoid[int(attr['groupid'])],
                'author':attr['author'],
                'text':node.find('text').text,
                'netid':attr.get('id','0')
            })

        self.cur.execute('VACUUM')
        self.commit()

    def clearserver(self,serverid):
        self.wantcur()
        self.cur.execute("""delete from songs where 
            (?==(select r.id from groups g left join servers r on (g.serverid=r.id) 
                where g.id=songs.groupid))""",[serverid])
        self.cur.execute('delete from groups where serverid=?',[serverid])
        self.delete_noused_texts()
        self.commit()

    def delete_noused_texts(self):
        self.wantcur()
        self.cur.execute('DELETE FROM songtexts WHERE 0=(SELECT COUNT(id) FROM songs WHERE songtexts.songid=songs.id)')

    def download_from_server(self,dlg,server,serverid):
        dlg.Update(10, u"Stahuji databázi ze serveru %s" % server)
        s=server.download_db()

        dlg.Update(30, u"Analyzuji databázi ze serveru %s" % server)
        doc=etree.parse(StringIO(s))

        dlg.Update(40, u"Vkládám skupiny a písně do databáze")
        self.importxml(doc,serverid)
        dlg.Update(90, u"Vkládám písně do databáze")

    def exporttoshare(self,file_name):
        self.wantcur()
        xml=XmlNode('database')
        grpxml=xml.add('groups')
        for id,name,url in DBGroup.getlist(self,('id','name','url')):
            g=grpxml.add('group')
            g['id']=id
            g['name']=name
            g['url']=url
        sngxml=xml.add('songs')
        for id,title,author,groupid,text in DBSong.getlist(self,('id','title','author','groupid','text')):
            s=sngxml.add('song')
            s['id']=id
            s['title']=title
            s['author']=author
            s['groupid']=groupid
            s.add('text').text=text
        xml.save(open(file_name,'w'))


class DBGroup(dbobject.DBObject):
    attrnames=('name','serverid','url','netid')
    tablename='groups'
    tablechar='g'
    name_to_fld={'id':'g.id','name':'g.name','serverid':'g.serverid','url':'g.url','netid':'g.netid'}
    joins=[]

    def __unicode__(self): return self.name

class DBSong(dbobject.DBObject):
    attrnames=('title','author','group','text','groupid','netid')
    tablename='songs'
    tablechar='s'
    name_to_fld={'id':'s.id','title':'s.title','author':'s.author','group':'g.name',
        'text':'t.songtext','groupid':'s.groupid','netid':'s.netid'}
    joins=[('groups','g','s.groupid=g.id'),('songtexts','t','s.id=t.songid')]


    @classmethod
    def insertobject(self,db,values):
        res=dbobject.DBObject.insertobject.im_func(self,db,values)
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


class DBServer(dbobject.DBObject):
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
