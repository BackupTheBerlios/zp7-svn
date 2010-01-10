# USAGE
# make_version.py target_version_name [branch_name]
# branch_name can be eg. 2.3 (folder name from barnches folder), of nothing is given, trunk is assumed

import os, stat, zipfile, shutil, ftplib, string, time, sys, datetime
from cStringIO import StringIO
from lxml import etree

#DEVENV = r'c:\Program Files\Microsoft Visual Studio 8\Common7\IDE\devenv'
DEVENV = r'c:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\devenv'
MAKENSIS = r'c:\Program files\NSIS\makensis'
#SVNBASE = 'https://subversion.savana.cz/datadmin' 
SVNBASE = 'file:///c:/svn/datadmin2'

# FTPHOST = 'ftp.datadmin.com'
# FTPLOGIN = 'datadmincom'
# FTPPASSWORD = 'Jv6TkZTM3q'

FTPHOST = 'www.datadmin.com'
FTPLOGIN = 'datadmin_com'
FTPPASSWORD = 'draklesu'

FTPHOST2 = 'www.jenasoft.com'
FTPLOGIN2 = 'datadmin_jenasoft_com'
FTPPASSWORD2 = 'draklesu'

WINROOT = {'install': '$INSTDIR', 'appdata': '$APPDATA'}
LINROOT = {'install': 'usr/lib/datadmin', 'appdata': 'etc/datadmin/.config', 'root':''}

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
        fr = open("install/datadmin-install.exe", "rb")
        ftp.storbinary("STOR %s/datadmin-%s.exe" % (root, version), fr)
        
        fr = open("install/datadmin-linux.tgz", "rb")
        ftp.storbinary("STOR %s/datadmin-%s.tgz" % (root, version), fr)
        
        fr = open("install/datadmin-debian.deb", "rb")
        ftp.storbinary("STOR %s/datadmin-%s.deb" % (root, version), fr)
    elif isbeta:
        fr = open("install/datadmin-install.exe", "rb")
        ftp.storbinary("STOR %s/datadmin-beta.exe" % root, fr)
        
        fr = open("install/datadmin-linux.tgz", "rb")
        ftp.storbinary("STOR %s/datadmin-beta.tgz" % root, fr)
        
        fr = open("install/datadmin-debian.deb", "rb")
        ftp.storbinary("STOR %s/datadmin-beta.deb" % root, fr)
    else:
        fr = open("install/datadmin-install.exe", "rb")
        ftp.storbinary("STOR %s/datadmin-install.exe" % root, fr)

        fr = open("install/datadmin-install.exe", "rb")
        ftp.storbinary("STOR %s/datadmin2-install.exe" % root, fr)

        fr = open("install/datadmin-linux.tgz", "rb")
        ftp.storbinary("STOR %s/datadmin-linux.tgz" % root, fr)

        fr = open("install/datadmin-debian.deb", "rb")
        ftp.storbinary("STOR %s/datadmin-linux.deb" % root, fr)
    
        fr = open("install/datadmin-pad.xml", "rb")
        ftp.storbinary("STOR %s/datadmin-pad.xml" % root, fr)
    
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
            dirname = os.path.join('datadmin-%s' % pl, LINROOT[dparent.attrib['parent']], relpath)
            if not os.path.isdir(dirname): 
                os.makedirs(dirname)

def add_file(dparent, fxml, relpath, fname, src):
    global create_content_nsi, delete_content_nsi
    if isplatform(dparent, 'windows') and isplatform(fxml, 'windows'):
        create_content_nsi += ' SetOutPath "%s"\n' % os.path.join(WINROOT[dparent.attrib['parent']], relpath).replace('/', '\\')
        create_content_nsi += ' File %s\n' % src.replace('#BIN#', r'DatAdmin\bin\Release').replace('/', '\\') 
        delete_content_nsi = ' Delete "%s"\n' % os.path.join(WINROOT[dparent.attrib['parent']], relpath, fname).replace('/', '\\') + delete_content_nsi
    for pl in UNIX_PLATFORMS:
        if isplatform(dparent, pl) and isplatform(fxml, pl):
            shutil.copy(src.replace("#BIN#", r'DatAdmin\bin\Release'), os.path.join('datadmin-%s' % pl, LINROOT[dparent.attrib['parent']], relpath))
 

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
os.system("svn co %s/%s datadmin" % (SVNBASE, branch))

os.chdir("datadmin")
os.chdir("DatAdmin")

if '.rev' in version:
    version = version.replace('rev', svninfo('.')['Revision'])  

modify_version_file('DatAdmin.Core/VersionInfo.cs')
modify_version_file('install/debian-root/DEBIAN/control')

print 'Building DatAdmin...'
os.system(r'"%s" DatAdmin3.sln /Build Release' % DEVENV)
#os.system(r'"%s" DatAdmin2.sln /Build Debug' % DEVENV)
#print 'Exporting packages...'
#os.system(r'DatAdmin\bin\Debug\daci.exe exportaddons alladdons.adp --dir addons') # addons are in debug directory

print 'Creating installations...'
os.chdir("install")
# directory for linux distribution
for pl in UNIX_PLATFORMS:
    os.makedirs('datadmin-%s/usr/lib/datadmin' % pl) 
    os.makedirs('datadmin-%s/etc/datadmin/appdata' % pl) 
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
os.system('tar -cv --file=datadmin-linux.tar datadmin-linux/*')
os.system('gzip -9 < datadmin-linux.tar > datadmin-linux.tgz')

os.system('tar -cv --file=datadmin-debian.tar datadmin-debian/*')
os.system('gzip -9 < datadmin-debian.tar > datadmin-debian.tgz')

print 'Copying to linux build machine...'
os.system('ssh jena@jdesktop "rm -rf .dinst"') 
os.system('ssh jena@jdesktop "mkdir .dinst"') 
os.system('ssh jena@jdesktop "cat > .dinst/datadmin-debian.tgz" < datadmin-debian.tgz')
print 'Remote compiling debian distribution...'
data = open('compile_linux.sh', 'r').read()
open('compile_linux.sh', 'wb').write(data)
os.system('ssh jena@jdesktop < compile_linux.sh') 
print 'Download debian distribution...'
os.system('ssh jena@jdesktop "cat .dinst/datadmin-debian.deb" > datadmin-debian.deb') 

os.chdir("..")

# CREATE PAD FILE
padtpl = open('install/datadmin-pad-template.xml').read()
padtpl = padtpl.replace('#VERSION#', version)

padtpl = padtpl.replace('#MONTH#', time.strftime('%m'))
padtpl = padtpl.replace('#DAY#', time.strftime('%d'))
padtpl = padtpl.replace('#YEAR#', time.strftime('%Y'))

sizebytes = os.path.getsize('install/datadmin-install.exe')
padtpl = padtpl.replace('#SIZE_BYTES#', str(sizebytes))
padtpl = padtpl.replace('#SIZE_KBYTES#', str(sizebytes/1000))
padtpl = padtpl.replace('#SIZE_MBYTES#', '%0.2f' % (sizebytes/1000000.0))
open('install/datadmin-pad.xml', 'w').write(padtpl)

if issnapshot:
    doupload(FTPHOST, FTPLOGIN, FTPPASSWORD, '/datadmin.com/snapshot')
elif isbeta:
    doupload(FTPHOST, FTPLOGIN, FTPPASSWORD, '/datadmin.com')
else: # release
    doupload(FTPHOST, FTPLOGIN, FTPPASSWORD, '/datadmin.com')
    doupload(FTPHOST2, FTPLOGIN2, FTPPASSWORD2, '')

print 'Uploading version...'
ftp = ftplib.FTP(FTPHOST)
ftp.login(user = FTPLOGIN, passwd = FTPPASSWORD)
if isbeta:
    ftp.storbinary("STOR /datadmin.com/includes/datadmin/version-beta.php", StringIO('<?$ver_betaversion = "%s"; $ver_betachanged = "%s"; $ver_betafilename = "datadmin-beta.exe"; $ver_betafilename_tgz = "datadmin-beta.tgz"; $ver_betafilename_deb = "datadmin-beta.deb";?>' % (version, time.strftime('%Y-%m-%d %H:%M:%S')) ))
if isrelease:
    ftp.storbinary("STOR /datadmin.com/includes/datadmin/version.php", StringIO('<?$ver_version = "%s"; $ver_lastchanged = "%s";?>' % (version, time.strftime('%Y-%m-%d %H:%M:%S')) ))
    
if not issnapshot:
    print 'Uploading changelog...'
    os.system("svn co %s/changelog changelog" % SVNBASE)
    ftp.storbinary("STOR /datadmin.com/changelog-release.txt", open('changelog/release.txt'))
    ftp.storbinary("STOR /datadmin.com/changelog-beta.txt", open('changelog/beta.txt'))
    
ftp.quit()
ftp = ftplib.FTP(FTPHOST2)
ftp.login(user = FTPLOGIN2, passwd = FTPPASSWORD2)

if isbeta:
    ftp.storbinary("STOR includes/datadmin/version-beta.php", StringIO('<?$ver_betaversion = "%s"; $ver_betachanged = "%s"; $ver_betafilename = "datadmin-beta.exe"; $ver_betafilename_tgz = "datadmin-beta.tgz"; $ver_betafilename_deb = "datadmin-beta.deb";?>' % (version, time.strftime('%Y-%m-%d %H:%M:%S')) ))
if isrelease:
    ftp.storbinary("STOR includes/datadmin/version.php", StringIO('<?$ver_version = "%s"; $ver_lastchanged = "%s";?>' % (version, time.strftime('%Y-%m-%d %H:%M:%S')) ))

ftp.quit()
   
if not issnapshot:
    print 'Creating tag in SVN...'
    os.system('svn copy %s/%s %s/tags/%s -m "Creating tag %s"' % (SVNBASE, branch, SVNBASE, version, version))

os.chdir("../../..")

emptydir(".bld")
