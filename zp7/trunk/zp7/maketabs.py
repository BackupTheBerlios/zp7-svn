import os, os.path, string, sys
from os.path import join, getsize

TABSIZE = 4

def process(fn):
    print 'Processing file: %s' % fn
    text = open(fn).read()
    fw = open(fn, 'w')
    orig_tab_counts = [0]
    linenum = 0
    for line in text.split('\n'):
        linenum += 1
        if line.startswith('#'): print >>fw, line
        elif line.lstrip().startswith('#'):
            print >>fw, '%s%s' % (' ' * TABSIZE * (len(orig_tab_counts) - 1), line.lstrip())
        elif line.strip() == '': print >>fw, ''
        else:
            indent = len(line) - len(line.lstrip())
            if indent > orig_tab_counts[-1]:
                orig_tab_counts.append(indent)
            elif indent < orig_tab_counts[-1]:
                try:
                    pos = orig_tab_counts.index(indent)
                except:
                    print 'Bad indentation, file %s, line %s:\n%s' % (fn, linenum, line)
                    sys.exit()
                del orig_tab_counts[pos+1:]              
            print >>fw, '%s%s' % (' ' * TABSIZE * (len(orig_tab_counts) - 1), line.lstrip())
    fw.close()
        

for root, dirs, files in os.walk('.'):
    for name in files:
        if name.lower().endswith('.py') and name.lower() != 'maketabs.py':
            process(os.path.join(root, name))
    if '.svn' in dirs:
        dirs.remove('.svn')

