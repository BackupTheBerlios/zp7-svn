# -*- coding: utf-8 -*- 

import desktop
import dbvpanel

#desktop.register_menu(lambda c:c.create_menu_command('file/open',u'Otevřít',None,'',u'Otevře soubor'))

_panel=dbvpanel.DBVPanel()

desktop.add_content(_panel)

#add_song_column=_panel.grid.add_song_column
#remove_song_column=_panel.grid.remove_song_column

def add_song_column(*args):
  return _panel.grid.add_song_column(*args)
  
def remove_song_column(*args):
  _panel.grid.remove_song_column(*args)