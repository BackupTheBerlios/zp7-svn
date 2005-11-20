# -*- coding: UTF-8 -*-

import urllib
import gzip
import StringIO
import wx
import utils
import browse
import copy
import desktop
import anchors.internet
import interop

class APIServerType(anchors.internet.IServerType):
  name='api'
  def __unicode__(self): return 'API'
  def create(self): return APIServer()

class APIServer(anchors.internet.IServer):
  url=''
  login=''
  password=''
  #server_type=server_types['api']
  
  def __init__(self):
    self.server_type=interop.anchor['servertype'].find('api')

  def getbaseurl(self): return self.url

  def createurl(self,command,params={}):
    #http://test2.zpevnik.net/xmlapi/index.php
    return "%s?command=%s&login=%s&password=%s" % (self.getbaseurl(),command,urllib.quote(self.login),urllib.quote(self.password))
  
  def download_db(self):
    fr=urllib.urlopen(self.createurl('getdatabase'))
    s=fr.read()
    fr.close()
          
    try:
      fr=gzip.GzipFile(None,"rb",0,StringIO.StringIO(s))
      s=fr.read()
      fr.close()
    except IOError:
      pass
    
    return s

  def send_update(self,xml):
    raise NotImplemented()
  
  def __unicode__(self):
    if self.url: return unicode(self.url)
    return u'Nezadán'

  def edit(self):
    brw=browse.DialogBrowse(desktop.main_window,u'API Server')
    brw.grid(rows=2,cols=2,border=5)
    brw.label(text='URL:')
    brw.edit(model=browse.attr(self,'url'),size=(200,-1))
    brw.label(text='Login:')
    brw.edit(model=browse.attr(self,'login'))
    brw.label(text='Heslo:')
    brw.edit(model=browse.attr(self,'password'))
    #brw.label(text='Typ:')
    #brw.combo(model=sorted(server_types.values()),valuemodel=browse.attr(self,'server_type'))
    brw.button(text='OK',event=lambda ev:brw.ok())
    brw.button(text='Storno',event=lambda ev:brw.cancel())
    brw.endsizer()
    brw.run()


interop.anchor['servertype'].add_feature(APIServerType())

class ZPNETAPIServer(APIServer):
  def getbaseurl(self): return "http://test2.zpevnik.net/xmlapi/index.php"
  def __unicode__(self): return u'zpevnik.net'
  def edit(self):
    utils.showinfo(u'Tento typ serveru nemá žádné konfigurovatelné položky')


class ZPNETAPIServerType(anchors.internet.IServerType):
  name='zpnet'
  def __unicode__(self): return 'zpevnik.net'
  def create(self): return ZPNETAPIServer()

interop.anchor['servertype'].add_default(ZPNETAPIServerType())