# -*- coding: UTF-8 -*-

import images
import wx
import songtool.transp as transpmod
import config

_transp_bitmaps={}

_transp_titles={
  'prev':u'Nižší',
  'next':u'Vyšší',
  'prev5':u'Nižší o kvintu',
  'next5':u'Vyšší o kvintu',
  'simple':u'Nejjednodušší',
  'easy':u'Nejlehčí',
  'orig':u'Původní'
}

_transp_hints={
  'prev':u'Sníží akordy o půl tónu',
  'next':u'Zvýší akordy o půl tónu',
  'prev5':u'Sníží akordy o kvintu',
  'next5':u'Zvýší akordy o kvintu',
  'simple':u'Ukáže nejjednodušší akordy',
  'easy':u'Ukáže nejlehčí akordy',
  'orig':u'Obnoví původní akordy'
}

class SongEventCall:
  def __init__(self,songv,cmd):
    self.songv=songv
    self.cmd=cmd
    
  def __call__(self,event):
    self.songv.transp(self.cmd)

def _want_transp_bitmaps():
  if _transp_bitmaps: return
  for cmd in transpmod.commands:
    _transp_bitmaps[cmd]=getattr(images,'gettr'+cmd+'Bitmap')()
  
def make_transp_toolbar(songv,evthandler,toolbar):
  _want_transp_bitmaps()
  for cmd in transpmod.commands:
    toolid=wx.NewId()
    toolbar.AddSimpleTool(toolid,_transp_bitmaps[cmd],_transp_titles[cmd],_transp_hints[cmd])
    evthandler.Bind(wx.EVT_TOOL, SongEventCall(songv,cmd), id=toolid)

def make_transp_menu(songv,obj):
  _want_transp_bitmaps()
  for cmd in transpmod.commands:
    obj.create_menu_command('song/'+cmd,_transp_titles[cmd],SongEventCall(songv,cmd),config.get_hotkey('transp_'+cmd),_transp_hints[cmd],_transp_bitmaps[cmd])
