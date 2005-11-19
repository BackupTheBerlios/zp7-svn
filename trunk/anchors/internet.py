# -*- coding: UTF-8 -*-

import interop

class IServer:
  url=''
  login=''
  password=''
  server_type=None
  def edit(self): raise NotImplemented()
  def download_db(self): 
    """downloads database
    @result: xml
    @rtype: unicode string
    """
    raise NotImplemented()

class IServerType(object):
  def create(self): raise NotImplemented() #creates IServer

interop.anchor.define('servertype',IServerType)