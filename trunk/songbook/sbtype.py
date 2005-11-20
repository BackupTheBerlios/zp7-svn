# -*- coding: utf-8 -*- 

import utils.xmltools as xmltools
import utils.xmlnode as xmlnode
import utils
import desktop
import browse
import config
import wx
import copy
import realsb
import interop

class SBType(object):
  __xml_int__=('hcnt','vcnt','leftsp','topsp','rightsp','bottomsp','content_cols','a4distribtype')
  __xml_str__=('header_text','footer_text')
  features=( ('header','songheader'),('distribalg','distribalg'),('songdelimiter','songdelimiter') )
  hcnt=1
  vcnt=1
  leftsp=0
  rightsp=0
  content_cols=1
  topsp=0
  bottomsp=0
  a4distribtype=0
  name=u''
  basetype=u''
  saveonlydiff=False
  fontnames=('title','author','chord','text','label','content','header','footer')
  fonttitles={
    'title':u'Název',
    'author':u'Autor',
    'chord':u'Akord',
    'text':u'Text',
    'label':u'Návěští',
    'content':u'Obsah',
    'header':u'Záhlaví',
    'footer':u'Zápatí'
  }
  #fontnames=('default','chord','text','label')
  #fonttitles={'default':u'Implicitní','chord':u'Akord','text':u'Text','label':u'Návěští'}
  #fontnames=['chord','text','label']
  #fonttitles={'chord':u'Akord','text':u'Text','label':u'Návěští'}
  fonts={}
  header=None
  distribalg=None
  songdelimiter=None
  header_text=u''
  footer_text=u''

  
  def __init__(self):
    self.fonts={}
    for f in self.fontnames: self.fonts[f]=utils.emptyfont()
    for attr,anchor in self.features: setattr(self,attr,interop.anchor[anchor].default)
    #self.header=interop.anchor['songheader'].default
    #self.distribalg=interop.anchor['distribalg'].default
    #self.songdelimiter=interop.anchor['songdelimiter'].default

  def copyfrom(self,src):
    #oldname=self.name
    xml=xmlnode.XmlNode()
    src.xmlsavedata(xml)
    self.xmlloaddata(xml)
    #self.name=oldname

  def xmlsavediff(self,xml,implicitbtype=False):
    xml.clear()
    full=xmlnode.XmlNode()
    self.xmlsavedata(full)
    btype=searchsbtype(self.basetype)
    if not btype and implicitbtype: btype=SBType()
    if btype:
      bxml=xmlnode.XmlNode()
      btype.xmlsavedata(bxml)
      xmltools.xmldiff(full,bxml,xml)
    else:
      xml.assign(full)
 
  def changebase(self,newbase):
    #oldname=self.name
    xml=xmlnode.XmlNode()
    self.xmlsavediff(xml,True)
    bxml=xmlnode.XmlNode()
    newbase.xmlsavedata(bxml)
    xmltools.xmlmerge(bxml,xml)
    self.xmlloaddata(bxml)
    #self.name=oldname
    self.basetype=newbase.name
 
#   def _xml_load_generic_attrs(self,xml):
#     for a in self.iattrs: setattr(self,a,int(xml.attrs.get(a,0)))
#     for a in self.sattrs: setattr(self,a,xml[a])

  def xmlloaddata(self,xml):
    #self._xml_load_generic_attrs(xml)
    utils.xml_generic_load_attrs(self,xml)
    #self.header=interop.anchor['songheader'].find(xml['header'])
    #self.distribalg=interop.anchor['distribalg'].find(xml['distribalg'])
    for attr,anchor in self.features: setattr(self,attr,interop.anchor[anchor].find(xml[attr]))
    if self.hcnt<1: self.hcnt=1
    if self.vcnt<1: self.vcnt=1
    if self.content_cols<1: self.content_cols=1
    #self.name=xml.attrs.get('name',u'')
    #self.saveonlydiff=bool(xml.attrs.get('saveonlydiff',0))
    #self.basetype=xml.attrs.get('basetype','')
    for f in self.fontnames: utils.fontxmltodict(xml/'fonts'/f,self.fonts[f])
 
  def xmlload(self,xml):
    myxml=xml
    self.name=xml.attrs.get('name',u'')
    self.saveonlydiff=xml['saveonlydiff']=='1'
    self.basetype=xml['basetype']
    if self.saveonlydiff and self.basetype:
      btype=searchsbtype(xml['basetype'])
      if btype:
        myxml=xmlnode.XmlNode()
        self.basetype.xmlsave(myxml)
        xmltools.xmlmerge(myxml,xml)
        myxml['saveonlydiff']=1
    self.xmlloaddata(myxml)        
    
  def xmlsave(self,xml):
    if self.saveonlydiff:
      self.xmlsavediff(xml)
    else:
      self.xmlsavedata(xml)
    xml['name']=self.name
    xml['saveonlydiff']=self.saveonlydiff
    xml['basetype']=self.basetype

  def _xml_save_generic_attrs(self,xml):
    for a in self.iattrs: xml[a]=getattr(self,a)
    for a in self.sattrs: xml[a]=getattr(self,a)
    
  def xmlsavedata(self,xml):
    xml.clear()
    utils.xml_generic_save_attrs(self,xml)
    for f in self.fontnames: utils.fontdicttoxml(self.fonts[f],xml/'fonts'/f)
    for attr,anchor in self.features: xml[attr]=getattr(self,attr).name
    #xml['header']=self.header.name
    #xml['distribalg']=self.distribalg.name
    
  @staticmethod
  def fromxml(xml):
    res=SBType()
    res.xmlload(xml)
    return res

  def __unicode__(self):  return self.name
  
  def getreal(self,dc):
    return realsb.RealSBType(self,dc)

  def get_basetype_obj(self): return searchsbtype(self.basetype)
  def set_basetype_obj(self,value): 
    if value: self.basetype=value.name
    else: self.basetype=u''
  
  basetype_obj=property(get_basetype_obj,set_basetype_obj)

sbtypes=utils.xmlloadarray_constructor(config.xml/'sbtypes',{'sbtype':SBType.fromxml})

def savesbtypes():
  #ve must save in right order
  ordered=[]
  rest=copy.copy(sbtypes)
  while len(rest)>0:
    ok=False
    for x in rest:
      if (not x.basetype) or searchsbtype(x.basetype,ordered):
        ordered.append(x)
        rest.remove(x)
        ok=True
        break
    if not ok: rest[0].basetype='' #we must break cycle
    
  sbtypes[:]=ordered
  (config.xml/'sbtypes').childs[:]=[]
  utils.xmlsavearray(sbtypes,config.xml/'sbtypes','sbtype')
   
   
def edit_sb_type(sbtype):
  def _copy_from_type(ev):
    src=brw['basesbtype'].getitem()
    if src:
      sbtype.basetype=src.name
      sbtype.copyfrom(src)
      brw.loadall()    

  if not sbtype: return
  brw=browse.DialogBrowse(desktop.main_window,u'Typ zpěvníku')
  brw.vbox(proportion=1)
  brw.pager()
  brw.page(text=u'Obecné')
  
  brw.vbox()
  brw.grid(border=5,cols=3,rows=2,flags=wx.ALL)
  
  if sbtype.name:
    brw.label(text=u'Jméno')
    brw.label(text=sbtype.name)
    brw.space()
  
  brw.label(text=u'Bázový typ zpěvníku',size=(100,-1))
  brw.combo(model=sbtypes,id='basesbtype',valuemodel=browse.attr(sbtype,'basetype_obj'))
  brw.button(text=u'Zkopírovat',event=_copy_from_type)
  
  brw.label(text=u'Rozdělení na stránku')
  brw.combo(model=list(interop.anchor['distribalg']),valuemodel=browse.attr(sbtype,'distribalg'))
  brw.label()
  
  brw.label(text=u'Záhlaví písně')
  brw.combo(model=list(interop.anchor['songheader']),valuemodel=browse.attr(sbtype,'header'))
  brw.label()

  brw.label(text=u'Oddělovač písní')
  brw.combo(model=list(interop.anchor['songdelimiter']),valuemodel=browse.attr(sbtype,'songdelimiter'))
  brw.label()

  brw.label(text=u'Rozdělení malých stránek na A4')
  brw.combo(model=[u'Sešit',u'Do řádků'],curmodel=browse.attr(sbtype,'a4distribtype'))
  brw.label()
  
  brw.endsizer()
  brw.label(proportion=1)
  brw.endsizer()
  
  brw.endparent()
  
  brw.page(text=u'Fonty')
  brw.grid(border=10,flags=wx.ALL,cols=2,rows=3)
  for f in sbtype.fontnames:
    brw.font(text=sbtype.fonttitles[f],font=sbtype.fonts[f],size=(150,50))
  brw.endsizer()
  brw.endparent()

  brw.page(text=u'Vzhled stránky')
  brw.vbox()
  brw.grid(border=5,flags=wx.ALL,cols=2,rows=3)
  brw.label(text=u'Počet malých stránek horizontálně')
  brw.spin(model=browse.attr(sbtype,'hcnt'))
  brw.label(text=u'Počet malých stránek vertikálně')
  brw.spin(model=browse.attr(sbtype,'vcnt'))
  brw.label(text=u'Osazení zleva (mm)')
  brw.spin(model=browse.attr(sbtype,'leftsp'))
  brw.label(text=u'Osazení zprava (mm)')
  brw.spin(model=browse.attr(sbtype,'rightsp'))
  brw.label(text=u'Osazení zezhora (mm)')
  brw.spin(model=browse.attr(sbtype,'topsp'))
  brw.label(text=u'Osazení zdola (mm)')
  brw.spin(model=browse.attr(sbtype,'bottomsp'))
  brw.label(text=u'Počet sloupců v obsahu')
  brw.spin(model=browse.attr(sbtype,'content_cols'))
  brw.label(text=u'Záhlaví (%c-číslo stránky)')
  brw.edit(model=browse.attr(sbtype,'header_text'))
  brw.label(text=u'Zápatí (%c-číslo stránky)')
  brw.edit(model=browse.attr(sbtype,'footer_text'))
  brw.endsizer()
  brw.label(proportion=1)
  brw.endsizer()
  brw.endparent()
  
  brw.endparent()

  brw.defokcancel()
    
  brw.endsizer()
  brw.run()
   
def searchsbtype(name,array=sbtypes):
  for item in array: 
    if item.name==name: 
      return item
  return None

def _newsbtype(brw):
  name=brw['newsbtype'].getvalue()
  if not name:
    utils.showerror(u'Zpěvník musí mít zadané jméno')
    return
    
  if searchsbtype(name): 
    utils.showerror(u'Zpěvník %s už existuje'%name)
    return
  
  res=SBType()
  res.name=name
  brw['sbtypes'].append(res)
  
def edit_sb_types():
  brw=browse.DialogBrowse(desktop.main_window,u'Typy zpěvníku')
  brw.hbox()
  brw.vbox(border=10)
  brw.label(text=u'Dostupné typy zpěvníku')
  brw.listbox(id='sbtypes',size=(100,200),model=sbtypes)
  brw.endsizer()
  brw.vbox(border=3)
  brw.button(text=u'Upravit',event=lambda ev:edit_sb_type(brw['sbtypes'].getitem()))
  brw.button(text=u'Vymazat',event=lambda ev:utils.confirm_call(u'Opravdu vymazat zpěvník',lambda: brw['sbtypes'].eraseact()))
  brw.space((1,20))
  brw.label(text=u'Jméno nového zpěvníku')
  brw.edit(id='newsbtype')
  brw.button(text=u'Nový',event=lambda ev:_newsbtype(brw))
  brw.space((1,20))
  brw.button(text=u'Zavřít',event=lambda ev:brw.ok())
  brw.endsizer()
  brw.endsizer()
  brw.run()
  savesbtypes()