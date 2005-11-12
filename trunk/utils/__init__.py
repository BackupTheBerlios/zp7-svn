# -*- coding: UTF-8 -*-

import images
import wx
import songtool.transp as transpmod
import unicodedata
import psyco
import os
import os.path

main_window=None # can be referenced from all modules

def wx_fill_list(list,generator,fn=lambda x:x):
  list.Clear()
  for item in generator:
    list.Append(unicode(fn(item)))

def wx_add_art_tool(toolbar,event,icon_id,evtbinder,short_hint=u'',long_hint=u''):
  """adds toolbar button with icon from wx art provider"""
  bmp=wx.ArtProvider.GetBitmap(icon_id,wx.ART_TOOLBAR,(16,16))
  toolid=wx.NewId()
  toolbar.AddSimpleTool(toolid,bmp,short_hint,long_hint)
  evtbinder.Bind(wx.EVT_TOOL,event,id=toolid)

def _showinfoerror(msg,flags):
  import desktop
  dlg=wx.MessageDialog(desktop.main_window,msg,u'Zpěvníkátor',flags|wx.OK|wx.CENTRE)
  dlg.ShowModal()
  dlg.Destroy()

def showerror(msg):
  _showinfoerror(msg,wx.ICON_ERROR)

def showinfo(msg):
  _showinfoerror(msg,wx.ICON_INFORMATION)

def confirm(msg):
  import desktop
  dlg=wx.MessageDialog(desktop.main_window,msg,u'Zpěvníkátor',wx.YES|wx.NO|wx.CENTRE|wx.ICON_QUESTION)
  res=dlg.ShowModal()
  dlg.Destroy()
  return res==wx.ID_YES

def confirm_call(msg,call):
  if confirm(msg): call()

def open_dialog(parent,wildcard,message=u"Otevřít"):
  """shows open dialog
  
  @param wildcard: srt like 'all files|*.*|text files|*.txt'"""
  while True:
    try:
      dlg=wx.FileDialog(
        parent,message=message,defaultDir=os.getcwd(),
        defaultFile="",wildcard=wildcard,style=wx.OPEN|wx.CHANGE_DIR
      )
      if dlg.ShowModal() == wx.ID_OK:
        file=dlg.GetPath()
        if os.path.isfile(file): return file
        wx.MessageDialog(parent,u'Soubor nebyl %s nalezen' % file,u'Zpěvníkátor').ShowModal()
      else:
        return None
    finally:
      dlg.Destroy()

def save_dialog(parent,wildcard,deffile='',message=u"Uložit"):
  """shows save dialog
  
  @param wildcard: srt like 'all files|*.*|text files|*.txt'"""
  try:
    if deffile:
      dir=os.path.dirname(deffile)
      file=os.path.basename(deffile)
    else:
      dir=os.getcwd()
      file=''
    dlg=wx.FileDialog(
      parent,message=message,defaultDir=dir,
      defaultFile=file,wildcard=wildcard,style=wx.SAVE|wx.CHANGE_DIR|wx.OVERWRITE_PROMPT
    )
    if dlg.ShowModal() == wx.ID_OK: return dlg.GetPath()
    return None
  finally:
    dlg.Destroy()
    
def make_search_text(text):
  """makes search text
  
  removes diacritics, converts to uppercase and omits all nonalphanum characters""" 
  from unicodedata import combining,normalize
  text=unicode(text)
  text=normalize('NFKD',text)
  res=''
  for c in text:
    if not combining(c):
      res+=c
  res=res.upper()
  nres=''
  for c in res:
    if u"ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".find(c)>=0 : nres+=c
    
  return nres
   
def xmlsavearray(array,xml,name='item'):
  """saves array to xml
  
  @type array: iterable
  @type xml: L{utils.xmlnode.XmlNode}
  @type name: str
  """
  for item in array: item.xmlsave(xml.add(name))

def xmlloadarray_constructor(xml,creators={}):
  """saves array to xml
  
  @type xml: L{utils.xmlnode.XmlNode}
  @type creators: doct(xmlname:class)
  @rtype: list
  """
  res=[]
  for item in xml: res.append(creators[item.name](item))
  return res

def xmlsavedict(xml,dic,args=None):
  if not args: args=dic.keys()
  for arg in args: xml.attrs[arg]=dic[arg]

def xmlloaddict(xml,dic,args=None):
  if not args: args=xml.attrs.keys()
  for arg in args: dic[arg]=xml.attrs[arg]

def fontxmltodict(xml,d):
  d['bold']=int(xml.attrs.get('bold',0))
  d['italic']=int(xml.attrs.get('italic',0))
  d['size']=int(xml.attrs.get('size',10))
  d['face']=xml.attrs.get('face','Arial')
  d['underline']=int(xml.attrs.get('underline',0))
  d['color']=xml.attrs.get('color','black')

def fontdicttoxml(d,xml):
  xml['bold']=int(d['bold'])
  xml['italic']=int(d['italic'])
  xml['size']=int(d['size'])
  xml['face']=d['face']
  xml['underline']=int(d['underline'])
  if isinstance(d['color'],wx.Colour):
    t=d['color'].Get()
    xml['color']='#'+str(chr(t[0])+chr(t[1])+chr(t[2])).encode('hex')
  else:
    xml['color']=d['color']

def emptyfont():
  return {'bold':False,'italic':False,'size':10,'face':'Arial','underline':False,'color':'black'}

def wxfontfromdict(d):
  bold=wx.NORMAL
  if d.get('bold',False): bold=wx.FONTWEIGHT_BOLD
  style=wx.NORMAL
  if d.get('italic',False) : style=wx.FONTSTYLE_ITALIC
  return wx.Font(d.get('size',10),wx.DEFAULT,style,bold,d.get('underline',False),d.get('face','Arial'))

def wxfonttodict(font,d):
  d['bold']=font.GetWeight()==wx.FONTWEIGHT_BOLD
  d['italic']=font.GetStyle()==wx.FONTSTYLE_ITALIC
  d['size']=font.GetPointSize()
  d['face']=font.GetFaceName()
  d['underline']=font.GetUnderlined()

def editfont(font):
  import desktop
  data=wx.FontData()
  data.EnableEffects(True)
  data.SetColour(font.get('color','black'))
  data.SetInitialFont(wxfontfromdict(font))

  dlg=wx.FontDialog(desktop.main_window, data)
    
  if dlg.ShowModal() == wx.ID_OK:
    data=dlg.GetFontData()
    wxfonttodict(data.GetChosenFont(),font)
    font['color']=data.GetColour()
    return True
  
  return False
   
def xml_generic_load_attrs(self,xml):
  if hasattr(self,'__xml_int__'):
    for a in self.__xml_int__: 
      setattr(self,a,int(xml.attrs.get(a,0)))
  if hasattr(self,'__xml_str__'):
    for a in self.__xml_str__:
      setattr(self,a,xml[a])
  if hasattr(self,'__xml_bool__'):
    for a in self.__xml_bool__: 
      setattr(self,a,bool(int(xml.attrs.get(a,0))))

def xml_generic_save_attrs(self,xml):
  if hasattr(self,'__xml_int__'):
    for a in self.__xml_int__: 
      xml[a]=getattr(self,a)
  if hasattr(self,'__xml_str__'):
    for a in self.__xml_str__:
      xml[a]=getattr(self,a)
  if hasattr(self,'__xml_bool__'):
    for a in self.__xml_bool__: 
      xml[a]=int(getattr(self,a))

   
psyco.bind(make_search_text)