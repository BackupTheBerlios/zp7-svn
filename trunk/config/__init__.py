# -*- coding: UTF-8 -*-

import os
import os.path
import sys
import atexit

import desktop

import hotkeys
import fonts
import utils.xmlnode

import cfgpanel

def _reload_cfg(event):
  reload(keyboard)
  desktop.recreate_menu()

def _edit_cfg_scripts(event):
  desktop.show_content('cfg-scripts')

def get_hotkey(name):
  try:
    return getattr(hotkeys,name)
  except:
    return ''

class HotKeyAcc:
  def __getattr__(self,name):
    return get_hotkey(name)

hotkey=HotKeyAcc()

def get_font(name):
  try:
    return getattr(fonts,name)
  except:
    return {}

class FontAcc:
  def __getattr__(self,name):
    return get_font(name)

font=FontAcc()

def _create_menu(obj):
  obj.create_submenu('options',u'Nastavení')
  obj.create_menu_command('options/keyboard',u'Upravit konfigurační skripty',_edit_cfg_scripts,hotkey.edit_cfg_scripts)
  obj.create_menu_command('options/relaod',u'Znovu načíst konfigurační skripty',_reload_cfg,hotkey.reload_cfg)

desktop.register_menu(_create_menu)
desktop.add_content(cfgpanel.CfgPanel())

cfgfilename=os.path.join(os.path.dirname(os.path.dirname(sys.argv[0])),"options.xml")

try:
  xml=utils.xmlnode.XmlNode.load(open(cfgfilename,"r"))
except:
  xml=utils.xmlnode.XmlNode('options')
  
def save_xml():
  xml.save(open(cfgfilename,"w"))
  
atexit.register(save_xml)
