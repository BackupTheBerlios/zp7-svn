# -*- coding: UTF-8 -*-

class IMenuCreator:
  def create_menu_command(self,path,title,event,hotkey=u'',hint=u'',bitmap=None):
    """adds menu command to submenu indicated by path (eg. file/open)"""
    pass
    
  def create_submenu(self,path,title):
    """adds submenu, no command is assocated with this menu, but it can have children"""
    pass
    
  def get_toolbar(self):
    """@rtype: L{wx.ToolBar}"""
    pass
    
  def get_event_binder(self):
    """returns wx object, which has method Bind to bind events"""

    