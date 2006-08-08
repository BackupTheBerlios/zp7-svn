import os,os.path,sys

# import all external modules
import wx
import wx.lib.layoutf
import wx.lib.stattext
import wx.lib.buttons 
import wx.grid
import wx.stc
import pysqlite2.dbapi2
import lxml.etree, lxml._elementpath
import urllib
import gzip
import xml.sax.saxutils
import shutil
import zipfile
import threading
import wx.py

sys.path.append(os.path.normpath('%s/../lib' % sys.argv[0]))

if __name__=='__main__':
    import update
    update.update()

    import main
    main.main()

