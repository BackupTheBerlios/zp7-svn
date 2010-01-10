# -*- coding: UTF-8 -*-

class IDBManager:
    """manager of available databases"""
    def __init__(self): pass
    def refresh(self): 
        """Reloads contents of db directory"""
        pass

    def create_database(self,name,servers):
        """creates database downloaded from internet"""
        pass

    def __iter__(self):
        """iterates over dbs in alphabetic order"""
        pass

    def __getitem__(self,index):
        """get db object, index is integer"""
        pass

class ISongDB:
    """song database"""
    def commit(self):
        """commits changes to file"""
        pass

    def addgroup(self,name,url="",netid=0):
        """adds new group to database"""
        pass

    def getsongbyid(self,songid,fields):
        """return sequence of song values

        @type fields: sequence(str)
        """
        pass

    def addsong(self,title,groupid,author,songtext,netid=0):
        """adds new song to database"""
        pass

    def getsongsby(self,order,fields):
        """return generator of tuples of fields of songs

        @rtype: ( (title1,group1...),(title2,group2,...),... )
        """
        pass

    def __getitem__(self,songid):
        """return song object identigied by id

        @type: IDBSong
        """
        pass

    def getsearchtexts(self):
        """loads all search texts

        @rtype:dict(int(songid):str(searchtext))
        """
        pass
