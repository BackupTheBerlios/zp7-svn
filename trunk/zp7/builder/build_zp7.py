import os,stat,zipfile,shutil,ftplib,string,time,sys,os.path
from StringIO import StringIO

def emptydir(dirname):
  if os.path.isdir(dirname):
    print "Removing directory "+dirname
    for root, dirs, files in os.walk(dirname,topdown=False):
      for name in files:
        os.chmod(os.path.join(root, name),stat.S_IWRITE);
        os.remove(os.path.join(root, name))
      for name in dirs:
        os.rmdir(os.path.join(root, name))
    os.rmdir(dirname);

noinc=False

for arg in sys.argv[1:]:
  if arg=='--noinc': noinc=True
  else: raise Exception('Unknown argument: %s' % arg)

emptydir('build')
os.mkdir('build')
os.chdir('build')

os.mkdir("zp7")

os.system("svn checkout https://jena@svn.berlios.de/svnroot/repos/zp7/trunk/zp7")
os.system("svn checkout https://jena@svn.berlios.de/svnroot/repos/zp7/trunk/winlib")
#os.system("svn co svn://svn.berlios.de/zp7/trunk/zp7")

print 'creating version'
version=map(int,open('zp7/lib/version.txt','r').readline().split('.'))
version[-1]+=1
version='.'.join(map(str,version))
open('zp7/lib/version.txt','w').write(version)

print 'creating lib.zip'
zip=zipfile.ZipFile('lib.zip',"w",zipfile.ZIP_DEFLATED)
for root, dirs, files in os.walk("zp7\\lib"):
  if '.svn' in dirs:
    dirs.remove('.svn')
  for name in dirs:
    zip.writestr(os.path.join(root,name).replace("zp7\\","")+'/','')
  for name in files:
    if os.path.splitext(name)[1].lower()=='.py' or name.lower()=='version.txt':
      zip.write(os.path.join(root,name),os.path.join(root,name).replace("zp7\\",""))
zip.close()

print 'creating zpevnikator.exe'
os.chdir('zp7')
os.system('makeexe.cmd')
os.chdir('..')
print 'creating zp7.zip'
zip=zipfile.ZipFile('zp7.zip',"w",zipfile.ZIP_DEFLATED)
zip.writestr('zp7/','')
zip.writestr('zp7/auto-install/','')
zip.writestr('zp7/db/','')
zip.writestr('zp7/lib/','')
zip.writestr('zp7/lib/update.py',open('zp7/lib/update.py').read())
zip.writestr('zp7/library.zip',open('zp7/dist/library.zip','rb').read())
zip.writestr('zp7/MSVCR71.dll',open('zp7/dist/MSVCR71.dll','rb').read())
zip.writestr('zp7/w9xpopen.exe',open('zp7/dist/w9xpopen.exe','rb').read())
zip.writestr('zp7/zpevnikator.exe',open('zp7/dist/zpevnikator.exe','rb').read())
zip.writestr('zp7/unicows.dll',open('winlib/unicows.dll','rb').read())
zip.writestr('zp7/options.xml',open('zp7/options.xml').read())
zip.writestr('zp7/auto-install/lib.zip',open('lib.zip','rb').read())
zip.close()

print 'logging into FTP autoinstall'
ftp=ftplib.FTP("ftp.zpevnik.net")
ftp.login(user="autoinstall.zpevnik.net",passwd="python")
print 'uploading lib.zip'
ftp.storbinary("STOR /lib.zip",open('lib.zip','rb'));
print 'uploading version.txt'
ftp.storbinary("STOR /version.txt",StringIO(version))
ftp.quit()

print 'logging into FTP download'
ftp=ftplib.FTP("ftp.zpevnik.net")
ftp.login(user="download.zpevnik.net",passwd="python")
print 'uploading zp7.zip'
ftp.storbinary("STOR /zp7.zip",open('zp7.zip','rb'));
ftp.quit()

if not noinc:
  os.system('svn ci zp7/lib/version.txt -m "automatic version increase"')

print 'finished OK, new version: %s' % version


os.chdir('..')