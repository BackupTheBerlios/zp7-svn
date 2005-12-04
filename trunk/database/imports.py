# -*- coding: UTF-8 -*-

import desktop
import browse
import wx
import interop
import anchors.dbimport
from lxml import etree
import utils
from database import songdb, dbobject

def dbimportdialog(db):
  def addfile(ev):
    file=utils.open_dialog(desktop.main_window,'*.*')
    if file:
      brw['files'].append(file)

  files=[]
  filter=browse.var(interop.anchor['importfilter'].default)
  server=browse.var(dbobject.EmptyDBObject())
  
  brw=browse.DialogBrowse(desktop.main_window,u'Import')
  brw.vbox(border=5)
  brw.label(text=u'Soubory:')
  brw.listbox(model=files,id='files')
  brw.button(text=u'Přidat',event=addfile)
  
  brw.hbox(border=5)
  brw.label(text=u'Filtry:')
  brw.combo(model=list(interop.anchor['importfilter']),valuemodel=filter)
  brw.endsizer()

  brw.hbox(border=5)
  brw.label(text=u'Server:')
  brw.combo(model=[dbobject.EmptyDBObject()]+songdb.DBServer.enum(db),valuemodel=server)
  brw.endsizer()
  
  brw.defokcancel()
  brw.endsizer()
  
  if brw.run()==wx.ID_OK:
    try:
      dlg=wx.ProgressDialog(u"Import písní",u"Import písní",parent=desktop.main_window)
      xml=filter.get().getsongxml(files)
      db.importxml(xml,server.get().id)
    finally:
      dlg.Destroy()
    interop.send_flag('reloaddb')
    

class Zp6ImportFilter(anchors.dbimport.IImportFilter):
  name='zp6'
  def getsongxml(self,files):
    root=etree.Element('database')
    resxml=etree.ElementTree(root)
    resgroups=etree.SubElement(root,'groups')
    ressongs=etree.SubElement(root,'songs')
    groups={}
    for file in files:
      xml=etree.parse(open(file))
      for song in xml.xpath('//database/song'):
        groupname = song.attrib['group']
        if groupname not in groups:
          groups[groupname]=len(groups)+1
        group=groups[groupname]
        ressong=etree.SubElement(ressongs,'song')
        ressong.attrib['title']=song.attrib['title']
        ressong.attrib['author']=song.attrib.get('author',u'')
        ressong.attrib['groupid']=unicode(group)
        etree.SubElement(ressong,'text').text=song.find('text').text
    
    for g in groups:
      grp=etree.SubElement(resgroups,'group')
      grp.attrib['name']=g
      grp.attrib['id']=unicode(groups[g])
      grp.attrib['url']=u''
    
    return resxml
  def __unicode__(self): return u'Zpěvníkátor 6.0'

interop.anchor['importfilter'].add_default(Zp6ImportFilter())
