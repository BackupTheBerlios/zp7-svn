# -*- coding: UTF-8 -*-

import mainwin
import intf
import interop

_registered_items=[]
#_contents=[]
_should_show_content=None
main_window=None

def register_menu(generator):
  """adds generator function to register menu list
  
  @type generator: def(L{intf.IMenuCreator})
  """
  _registered_items.append(generator)
  
def recreate_menu():
  interop.send_flag('recreate_menu')
  #main_window.recreate_menu()
  
def add_content(content):
  """adds content pane (dbview, songlistview, ...)
  
  @type content: L{intf.IContent}"""
  interop.anchor['content'].add_feature(content)
  #_contents.append(content)
  
def show_content(name):
  global _should_show_content
  if main_window: 
    if active_content()!=name: interop.send_message(lambda:main_window.show_content(name))
  else: _should_show_content=name
  
def active_content(): return main_window.active_content()