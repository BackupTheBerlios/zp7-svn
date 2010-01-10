# USAGE
# make_version.py target_version_name [branch_name]
# branch_name can be eg. 2.3 (folder name from barnches folder), of nothing is given, trunk is assumed

import os, stat, zipfile, shutil, ftplib, string, time, sys, datetime
from cStringIO import StringIO
from lxml import etree

DEVENV = r'c:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\devenv'
MAKENSIS = r'c:\Program files\NSIS\makensis'
SVNBASE = 'https://svn.berlios.de/svnroot/repos/zp7/zp8'

FTPHOST = 'www.jenasoft.com'
FTPLOGIN = 'jenasoft_com'
FTPPASSWORD = 'draklesu'

WINROOT = {'install': '$INSTDIR', 'appdata': '$APPDATA'}
LINROOT = {'install': 'usr/lib/zpevnikator', 'appdata': 'etc/zpevnikator/.config', 'root':''}

UNIX_PLATFORMS = ['linux', 'debian']

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

def doupload(host, user, password, root):
    print "Provadim upload na %s" % host
    ftp = ftplib.FTP(host)
    ftp.login(user = user, passwd = password)
    
    if issnapshot:
        fr = open("install/zpevnikator-install.exe", "rb")
        ftp.storbinary("STOR %s/zpevnikator-%s.exe" % (root, version), fr)
        
        fr = open("install/zpevnikator-linux.tgz", "rb")
        ftp.storbinary("STOR %s/zpevnikator-%s.tgz" % (root, version), fr)
        
        fr = open("install/zpevnikator-debian.deb", "rb")
        ftp.storbinary("STOR %s/zpevnikator-%s.deb" % (root, version), fr)
    elif isbeta:
        fr = open("install/zpevnikator-install.exe", "rb")
        ftp.storbinary("STOR %s/zpevnikator-beta.exe" % root, fr)
        
        fr = open("install/zpevnikator-linux.tgz", "rb")
        ftp.storbinary("STOR %s/zpevnikator-beta.tgz" % root, fr)
        
        fr = open("install/zpevnikator-debian.deb", "rb")
        ftp.storbinary("STOR %s/zpevnikator-beta.deb" % root, fr)
    else:
        fr = open("install/zpevnikator-install.exe", "rb")
        ftp.storbinary("STOR %s/zpevnikator-install.exe" % root, fr)

        fr = open("install/zpevnikator-linux.tgz", "rb")
        ftp.storbinary("STOR %s/zpevnikator-linux.tgz" % root, fr)

        fr = open("install/zpevnikator-debian.deb", "rb")
        ftp.storbinary("STOR %s/zpevnikator-linux.deb" % root, fr)
    
    ftp.quit()

def svninfo(path):
    fr = os.popen('svn info %s' % path, 'r')
    res = {}
    for line in fr:
        try:
            name, value = line.strip().split(':', 1)
            res[name.strip()] = value.strip()
        except:
            pass
    return res

def modify_version_file(fn):
    print 'Modifying', fn
    vinfo = open(fn).read()
    vinfo = vinfo.replace('#BUILT_AT#', str(datetime.datetime.utcnow()))
    vinfo = vinfo.replace('#SVN_REVISION#', svninfo('.')['Revision'])
    vinfo = vinfo.replace('#VERSION#', version)
    open(fn, 'wb').write(vinfo)


def copyfiles(srcfolder, dstfolder, files):
    for f in files:
        shutil.copyfile(os.path.join(srcfolder, f), os.path.join(dstfolder, f))

def copyallfiles(srcfolder, dstfolder):
    for f in os.listdir(srcfolder):
        if not os.path.isfile(os.path.join(srcfolder, f)): continue
        shutil.copyfile(os.path.join(srcfolder, f), os.path.join(dstfolder, f))

def isplatform(xml, platform):
    if xml is None: return True
    if not xml.attrib.has_key('platforms'): return True
    return platform in xml.attrib['platforms']

def add_directory(dparent, relpath, extxml):
    global create_content_nsi, delete_content_nsi
    if isplatform(dparent, 'windows') and isplatform(extxml, 'windows'):
        create_content_nsi += '  CreateDirectory "%s\\%s"\n' % (WINROOT[dparent.attrib['parent']], relpath.replace('/', '\\')) 
        delete_content_nsi = '  RMDir "%s\\%s"\n' % (WINROOT[dparent.attrib['parent']], relpath.replace('/', '\\')) + delete_content_nsi 
    for pl in UNIX_PLATFORMS:
        if isplatform(dparent, pl) and isplatform(extxml, pl):
            dirname = os.path.join('zpevnikator-%s' % pl, LINROOT[dparent.attrib['parent']], relpath)
            if not os.path.isdir(dirname): 
                os.makedirs(dirname)

def add_file(dparent, fxml, relpath, fname, src):
    global create_content_nsi, delete_content_nsi
    if isplatform(dparent, 'windows') and isplatform(fxml, 'windows'):
        create_content_nsi += ' SetOutPath "%s"\n' % os.path.join(WINROOT[dparent.attrib['parent']], relpath).replace('/', '\\')
        create_content_nsi += ' File %s\n' % src.replace('#BIN#', r'zp8\bin\Release').replace('/', '\\') 
        delete_content_nsi = ' Delete "%s"\n' % os.path.join(WINROOT[dparent.attrib['parent']], relpath, fname).replace('/', '\\') + delete_content_nsi
    for pl in UNIX_PLATFORMS:
        if isplatform(dparent, pl) and isplatform(fxml, pl):
            shutil.copy(src.replace("#BIN#", r'zp8\bin\Release'), os.path.join('zpevnikator-%s' % pl, LINROOT[dparent.attrib['parent']], relpath))
 

def add_content(dparent, srcfolder, dstfolder, item):  
    #if isplatform(dparent, 'windows') and isplatform(item, 'windows'):
#        srcdir = 
    for f in os.listdir(srcfolder):
        if os.path.isfile(os.path.join(srcfolder, f)):
            add_file(dparent, None, dstfolder, f, os.path.join(srcfolder, f))
        if os.path.isdir(os.path.join(srcfolder, f)):
            if '.svn' in f.lower():
                continue
            add_directory(dparent, os.path.join(dstfolder, f), item)
            add_content(dparent, os.path.join(srcfolder, f), os.path.join(dstfolder, f), item)

version = sys.argv[1]
issnapshot = '.rev' in version
isbeta = not issnapshot and (int(version.split('.')[1]) % 2) == 1 # druhe cislo ve verzi je liche
isrelease = not issnapshot and (int(version.split('.')[1]) % 2) == 0 # druhe cislo ve verzi je sude

if issnapshot:
    if (int(version.split('.')[1]) % 2) == 1: # druhe cislo ve verzi je liche
        vertype = 'ALPHA' 
    if (int(version.split('.')[1]) % 2) == 0: # druhe cislo ve verzi je sude
        vertype = 'GAMMA' 
elif isbeta:
    vertype = 'BETA'
else:
    vertype = ''

print 'Building', vertype

if len(vertype) > 0: spvertype = ' ' + vertype
else: spvertype = vertype  

try: branch = 'branches/' + sys.argv[2]
except: branch = 'trunk'

emptydir(".bld")
    
emptydir(".version")
if not os.path.isdir('.bld') : os.mkdir(".bld")

os.chdir(".bld")

print 'Checkouting from SVN...'
os.system("svn co %s/%s zpevnikator" % (SVNBASE, branch))

os.chdir("zpevnikator")

if '.rev' in version:
    version = version.replace('rev', svninfo('.')['Revision'])  

modify_version_file('install/debian-root/DEBIAN/control')

print 'Building Zpevnikator...'
os.system(r'"%s" zp8.sln /Build Release' % DEVENV)

print 'Creating installations...'
os.chdir("install")
# directory for linux distribution
for pl in UNIX_PLATFORMS:
    os.makedirs('zpevnikator-%s/usr/lib/zpevnikator' % pl) 
    os.makedirs('zpevnikator-%s/etc/zpevnikator/appdata' % pl) 
doc = etree.parse(open('install.xml'))
create_content_nsi = ''
delete_content_nsi = ''
for dparent in doc.xpath('/Install/Directory'):
    try: relpath = dparent.attrib['path']
    except: relpath = ''
    if relpath: add_directory(dparent, relpath, None)
    for item in dparent:
        if item.tag == 'File':
            add_file(dparent, item, relpath, item.attrib['name'], os.path.join('..', item.attrib['src']))
        if item.tag == 'CopyAll':
            add_content(dparent, os.path.join("..", item.attrib['src']), relpath, item)

data = open('install-tpl.nsi').read()
data = data.replace('#CREATE_CONTENT#', create_content_nsi)
data = data.replace('#DELETE_CONTENT#', delete_content_nsi)
data = data.replace('#VERTYPE#', vertype)
data = data.replace('#SPACEVERTYPE#', spvertype)
open('install-repl.nsi', 'w').write(data)

print 'Creating windows installer...'
os.system('"%s" install-repl.nsi' % MAKENSIS)

print 'Creating linux distributions...'
os.system('tar -cv --file=zpevnikator-linux.tar zpevnikator-linux/*')
os.system('gzip -9 < zpevnikator-linux.tar > zpevnikator-linux.tgz')

os.system('tar -cv --file=zpevnikator-debian.tar zpevnikator-debian/*')
os.system('gzip -9 < zpevnikator-debian.tar > zpevnikator-debian.tgz')

print 'Copying to linux build machine...'
os.system('ssh jena@jdesktop "rm -rf .dinst"') 
os.system('ssh jena@jdesktop "mkdir .dinst"') 
os.system('ssh jena@jdesktop "cat > .dinst/zpevnikator-debian.tgz" < zpevnikator-debian.tgz')
print 'Remote compiling debian distribution...'
data = open('compile_linux.sh', 'r').read()
open('compile_linux.sh', 'wb').write(data)
os.system('ssh jena@jdesktop < compile_linux.sh') 
print 'Download debian distribution...'
os.system('ssh jena@jdesktop "cat .dinst/zpevnikator-debian.deb" > zpevnikator-debian.deb') 

os.chdir("..")

doupload(FTPHOST, FTPLOGIN, FTPPASSWORD, '/new.jenasoft.com/download')
  
if not issnapshot:
    print 'Creating tag in SVN...'
    os.system('svn copy %s/%s %s/tags/%s -m "Creating tag %s"' % (SVNBASE, branch, SVNBASE, version, version))

os.chdir("../..")

emptydir(".bld")
