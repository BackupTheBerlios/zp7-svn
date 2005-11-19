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


class XMLServerType(anchors.internet.IServerType):
  name='xml'
  def __unicode__(self): return 'XML'
  def create(self): return XMLServer()

interop.anchor['servertype'].add_feature(XMLServerType())

class XMLServer(anchors.internet.IServer):
  url=''
  #server_type=server_types['api']
  
  def __init__(self):
    self.server_type=interop.anchor['servertype'].find('xml')
  
  def __unicode__(self):
    if self.url: return unicode(self.url)
    return u'Nezadán'

  def edit(self):
    brw=browse.DialogBrowse(desktop.main_window,u'XML Server')
    brw.grid(rows=2,cols=2,border=5)
    brw.label(text='URL:')
    brw.edit(model=browse.attr(self,'url'),size=(200,-1))
    #brw.label(text='Typ:')
    #brw.combo(model=sorted(server_types.values()),valuemodel=browse.attr(self,'server_type'))
    brw.button(text='OK',event=lambda ev:brw.ok())
    brw.button(text='Storno',event=lambda ev:brw.cancel())
    brw.endsizer()
    brw.run()
