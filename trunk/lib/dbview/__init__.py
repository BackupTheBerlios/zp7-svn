# -*- coding: utf-8 -*- 

import desktop
import dbvpanel

#desktop.register_menu(lambda c:c.create_menu_command('file/open',u'Otevřít',None,'',u'Otevře soubor'))

_panel=dbvpanel.DBVPanel()

desktop.add_content(_panel)

#add_song_column=_panel.grid.add_song_column
#remove_song_column=_panel.grid.remove_song_column

class DBColumnDef:
  fget=None
  fset=None
  renderer=None
  editor=None
  title=u''
  name=''
  
  def __init__(self,name,title,fget,fset,renderer,editor):
    """
    @type fget: lambda id,id:unicode
    @type fset: lambda id,id,unicode:pass
    """
    self.name=name
    self.title=title
    self.fget=fget
    self.fset=fset
    self.renderer=renderer
    self.editor=editor


def add_song_column(col):
  """adds song column
  
  @type col: L{dbgrid.DBColumnDef}
  """
  return _panel.add_song_column(col)
  
def remove_song_column(col):
  """removes song column
  
  @type col: L{dbgrid.DBColumnDef}
  """
  _panel.remove_song_column(col)
