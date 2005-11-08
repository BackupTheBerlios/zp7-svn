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

class SBType:
  iattrs=('hcnt','vcnt','leftsp','topsp','rightsp','bottomsp')
  hcnt=1
  vcnt=1
  leftsp=0
  rightsp=0
  topsp=0
  bottomsp=0
  name=u''
  basetype=u''
  saveonlydiff=False
  fontnames=('title','author','chord','text','label')
  fonttitles={'title':u'Název','author':u'Autor','chord':u'Akord','text':u'Text','label':u'Návěští'}
  #fontnames=('default','chord','text','label')
  #fonttitles={'default':u'Implicitní','chord':u'Akord','text':u'Text','label':u'Návěští'}
  #fontnames=['chord','text','label']
  #fonttitles={'chord':u'Akord','text':u'Text','label':u'Návěští'}
  fonts={}
  header=None
  
  def __init__(self):
    self.fonts={}
    for f in self.fontnames: self.fonts[f]=utils.emptyfont()
    self.header=interop.anchor['songheader'].default

  def copyfrom(self,src):
    xml=xmlnode.XmlNode()
    src.xmlsavedata(xml)
    self.xmlloaddata(xml)

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
    xml=xmlnode.XmlNode()
    self.xmlsavediff(xml,True)
    bxml=xmlnode.XmlNode()
    newbase.xmlsavedata(bxml)
    xmltools.xmlmerge(bxml,xml)
    self.xmlloaddata(bxml)
 
  def _xml_load_iattrs(self,xml):
    for a in self.iattrs: setattr(self,a,int(xml.attrs.get(a,0)))

  def xmlloaddata(self,xml):
    self._xml_load_iattrs(xml)
    self.name=xml.attrs.get('name',u'')
    self.saveonlydiff=bool(xml.attrs.get('saveonlydiff',0))
    self.basetype=xml.attrs.get('basetype','')
    for f in self.fontnames: utils.fontxmltodict(xml/'fonts'/f,self.fonts[f])
 
  def xmlload(self,xml):
    myxml=xml
    if xml['saveonlydiff'] and xml['basetype']:
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

  def _xml_save_iattrs(self,xml):
    for a in self.iattrs: xml[a]=getattr(self,a)
    
  def xmlsavedata(self,xml):
    xml.clear()
    self._xml_save_iattrs(xml)
    xml['name']=self.name
    xml['saveonlydiff']=self.saveonlydiff
    xml['basetype']=self.basetype
    for f in self.fontnames: utils.fontdicttoxml(self.fonts[f],xml/'fonts'/f)
    
  @staticmethod
  def fromxml(xml):
    res=SBType()
    res.xmlload(xml)
    return res

  def __unicode__(self):  return self.name
  
  def getreal(self,dc):
    return realsb.RealSBType(self,dc)

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
  if not sbtype: return
  brw=browse.DialogBrowse(desktop.main_window,u'Typ zpěvníku')
  brw.vbox(proportion=1)
  brw.pager()
  brw.page(text=u'Obecné')
  
  brw.vbox()
  brw.grid(border=5,cols=3,rows=2,flags=wx.ALL)
  
  brw.label(text=u'Jméno')
  brw.label(text=sbtype.name)
  brw.space()
  
  brw.label(text=u'Bázový typ zpěvníku',size=(100,-1))
  brw.combo(model=sbtypes,id='basesbtype')
  brw.button(text=u'Zkopírovat',event=lambda ev:sbtype.copyfrom(brw['basesbtype'].getitem()))
  
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
  brw.endsizer()
  brw.label(proportion=1)
  brw.endsizer()
  brw.endparent()
  
  brw.endparent()

  brw.hbox(border=5,layoutflags=wx.CENTER)
  brw.button(text='OK',event=lambda ev:brw.ok())
  brw.button(text='Storno',event=lambda ev:brw.cancel())
  brw.endsizer()
  
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