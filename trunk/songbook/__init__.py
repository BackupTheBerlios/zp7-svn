# -*- coding: UTF-8 -*-

import desktop
import sbpanel
import config
import sbtype

desktop.add_content(sbpanel.SBPanel())

def _edit_sb_types(ev):
  sbtype.edit_sb_types()

def _create_menu(obj):
  obj.create_menu_command('options/sbtypes',u'Typy zpěvníku',_edit_sb_types,config.hotkey.edit_sb_types)

desktop.register_menu(_create_menu)
