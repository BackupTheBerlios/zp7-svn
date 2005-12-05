# -*- coding: UTF-8 -*-

import wx
import os,os.path,sys
import zipfile

def update():
  basedir=os.path.normpath('%s/..' % sys.argv[0])
  zipname=os.path.join(basedir,'auto-install','lib.zip')
  vername=os.path.join(basedir,'auto-install','version.txt')
  if os.path.isfile(zipname) and os.path.isfile(vername):
    zip=zipfile.ZipFile(zipname)
    version=open(vername,'r').read()
    open(os.path.join(basedir,'lib','version.txt'),'w').write(version)
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
    os.remove(vername)
