# -*- coding: UTF-8 -*-

import wx
import utils
import browse
import copy
import desktop
import anchors.internet
import interop

# server_types={
#   'api':APIServerType(),
#   'xml':XMLServerType()
# }

# class ServerCfg:
#   url=''
#   login=''
#   password=''
#   server_type=server_types['api']
#   
#   def __init__(self,url=''):
#     self.url=url
#   
#   def __unicode__(self):
#     if self.url: return unicode(self.url)
#     return u'Nezadán'
# 
#   def edit(self):
#     brw=browse.DialogBrowse(desktop.main_window,u'Server')
#     brw.grid(rows=2,cols=2,border=5)
#     brw.label(text='URL:')
#     brw.edit(model=browse.attr(self,'url'),size=(200,-1))
#     brw.label(text='Login:')
#     brw.edit(model=browse.attr(self,'login'))
#     brw.label(text='Heslo:')
#     brw.edit(model=browse.attr(self,'password'))
#     brw.label(text='Typ:')
#     brw.combo(model=sorted(server_types.values()),valuemodel=browse.attr(self,'server_type'))
#     brw.button(text='OK',event=lambda ev:brw.ok())
#     brw.button(text='Storno',event=lambda ev:brw.cancel())
#     brw.endsizer()
#     brw.run()

def ask_servers():
  servers=[]
  server_type=browse.var(interop.anchor['servertype'].default)
  
  brw=browse.DialogBrowse(desktop.main_window,u'Konfigurace serverů')
  brw.vbox(border=5)
  brw.label(text=u'Servery:')
  brw.listbox(model=servers,id='servers',size=(200,200))
  brw.grid(cols=2,rows=2,border=5)
  brw.button(text=u'Přidat server typu:',event=lambda ev:brw['servers'].append(server_type.get().create()))
  brw.combo(model=list(interop.anchor['servertype']),valuemodel=server_type,autosave=True)
  brw.button(text=u'Ubrat',event=lambda ev:brw['servers'].eraseact())
  brw.button(text=u'Upravit',event=lambda ev:(brw['servers'].getitem().edit(),brw['servers'].reloadact()))
  brw.button(text=u'<<',event=lambda ev:brw['servers'].moveup())
  brw.button(text=u'>>',event=lambda ev:brw['servers'].movedown())
  brw.button(text='OK',event=lambda ev:brw.ok())
  brw.button(text='Storno',event=lambda ev:brw.cancel())
  brw.endsizer()
  brw.endsizer()
  if brw.run()==wx.ID_OK: return servers
  return None
