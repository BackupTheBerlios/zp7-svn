import os, sys, threading, os.path, wx, traceback

#abc_encoding = 'iso8859_2'
abc_encoding = 'latin_1'

basedir = os.path.dirname(sys.argv[0])
abcm2ps_exe = os.path.join(basedir, 'abcm2ps', 'abcm2ps.exe')
abc2midi_exe = os.path.join(basedir, 'abcmidi', 'abc2midi.exe')
gsdir = os.path.join(basedir, 'gs5.50')
gs_exe = 'gswin32c.exe'
tmpdir = os.path.join(basedir, 'tmp')
try: os.mkdir(tmpdir)
except: pass

# clear temp
for fn in os.listdir(tmpdir):
    os.remove(os.path.join(tmpdir, fn))

def run_gsexe(infile, outfile, device):
    try:
        cwd = os.getcwd()
        os.chdir(gsdir)
        cmd = '%s -sDEVICE=%s -dNOPAUSE -dBATCH -q -sOutputFile=%s %s' % (gs_exe, device, outfile, infile)
        print 'Executing: %s' % cmd
        os.system(cmd)
    finally:
        os.chdir(cwd)

def run_abc2ps(infile, outfile):
    cmd = '%s -O%s %s' % (abcm2ps_exe, outfile, infile)
    print 'Executing: %s' % cmd
    os.system(cmd)

def run_abc2midi(infile, outfile):
    cmd = '%s %s -o %s' % (abc2midi_exe, infile, outfile)
    print 'Executing: %s' % cmd
    os.system(cmd)

def remove_files(*files):
    for f in files:
        if os.path.isfile(f):
            os.remove(f)
            
tmp_count = 0

def tmp_file(ext):
    global tmp_count
    tmp_count += 1
    return os.path.join(tmpdir, 'tmp%s.%s' % (tmp_count, ext))
