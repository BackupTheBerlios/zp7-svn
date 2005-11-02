# -*- coding: UTF-8 -*-

class ISong:
  def getval(self,name) : raise NotImplemented()
  def gettitle(self) : return getval('title')
  def getauthor(self) : raise getval('author')
  def gettext(self) : raise getval('text')
  
  def __getattr__(self,name): 
    if (not name.startswith('__')) and (not name.endswith('__')):
      return self.getval(name)
    return object.__getattr__(self,name)
    
class DataSong(ISong):
  