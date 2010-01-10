# -*- coding: UTF-8 -*-

import pysqlite2.dbapi2 as sqlite 
import StringIO
import os
import os.path
import sys
import wx
import internet
import utils
import database
import desktop
import copy

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
            data=self.getdatabyid(db,id,self.attrnames)
            if data is None: self.vals=self.emptyvals()
            else: self.vals=dict(zip(self.attrnames,data))
        else: # new inserted object
            self.vals=self.emptyvals()
        self.dbvals=copy.copy(self.vals)

    @classmethod
    def emptyvals(self):
        return dict(zip(self.attrnames,[None]*len(self.attrnames)))

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


