# -*- coding: UTF-8 -*-

import wx
import utils
import browse
import copy
import desktop
import anchors.internet
import interop

class LocalServerType(anchors.internet.IServerType):
  name='local'
  def __unicode__(self): return u'(lokální)'
  def create(self): return LocalServer()

interop.anchor['servertype'].add_feature(LocalServerType())

class LocalServer(anchors.internet.IServer):
  server_type=None
  
  def __init__(self):
    self.server_type=interop.anchor['servertype'].find('local')
  
  def __unicode__(self):
    return u'(lokální)'

  def edit(self):
    utils.showinfo(u'Tento typ serveru nemá žádné konfigurovatelné položky')

  def download_db(self):
    raise NotImplemented()

