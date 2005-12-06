# -*- coding: UTF-8 -*-

import wx
import os,os.path,sys
import zipfile

def wantnewversion(libvername,version):
  if os.path.isdir('.svn'): return False
  try: libversion=open(libvername,'r').read()
  except: return True
  msg=u'Nainstalovat novou verzi zpěvnikátoru %s (aktuální je %s)' % (version,libversion)
  app=wx.App()
  dlg=wx.MessageDialog(None,msg,u'Zpěvníkátor',wx.YES|wx.NO|wx.CENTRE|wx.ICON_QUESTION)
  return dlg.ShowModal()==wx.ID_YES

def update():
  basedir=os.path.normpath('%s/..' % sys.argv[0])
  zipname=os.path.join(basedir,'auto-install','lib.zip')
  libvername=os.path.join(basedir,'lib','version.txt')
  
  if os.path.isfile(zipname):
    zip=zipfile.ZipFile(zipname)
    version=zip.read('lib/version.txt')
    
    if wantnewversion(libvername,version):
      open(libvername,'w').write(version)
      for f in zip.namelist():
        if f.endswith('/'):
          try: os.mkdir(os.path.join(basedir,f))
          except: pass
        else:
          content=zip.read(f)
          open(os.path.join(basedir,f),'wb').write(content)
      zip.close()
      zip=None
      os.remove(zipname)
