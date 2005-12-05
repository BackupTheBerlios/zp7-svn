import threading
import urllib
import os.path,sys

class VersionChecker(threading.Thread):
  def run(self):
    #try:
      self.dorun()
    #except:
      #pass
  
  def dorun(self):
    newversion=map(int,urllib.urlopen('http://zpevnik.net/auto-install/version.txt').read().split('.'))
    oldversion=map(int(open(os.path.normpath('%s/../lib/version.txt'%sys.argv[0]),'r')))
    if newversion>oldversion:
      version=urllib.urlopen('http://zpevnik.net/auto-install/version.txt').read()
      content=urllib.urlopen('http://zpevnik.net/auto-install/lib.zip').read()
      open(os.path.normpath('%s/../auto-install/lib.zip'%sys.argv[0]),'wb').write(content)
      open(os.path.normpath('%s/../auto-install/version.txt'%sys.argv[0]),'w').write(version)

def checknewversion():
  VersionChecker().start()
