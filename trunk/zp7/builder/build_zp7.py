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

emptydir('build')
os.mkdir('build')
os.chdir('build')

os.mkdir("zp7")
os.system("svn co svn://svn.berlios.de/zp7/trunk/zp7")

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
  for name in files:
    if os.path.splitext(name)[1].lower()=='.py' or name.lower()=='version.txt':
      zip.write(os.path.join(root,name),os.path.join(root,name).replace("zp7\\",""))
zip.close()


print 'logging into FTP'
ftp=ftplib.FTP("ftp.zpevnik.net")
ftp.login(user="zpevnik.net",passwd="u55XhSPF")
print 'uploading lib.zip'
ftp.storbinary("STOR /www/auto-install/lib.zip",open('lib.zip','rb'));
print 'uploading version.txt'
ftp.storbinary("STOR /www/auto-install/version.txt",StringIO(version))
ftp.quit()

os.system('svn ci zp7/lib/version.txt -m "automatic version increase"')

print 'finished OK, new version: %s' % version


os.chdir('..')
