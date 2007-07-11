from distutils.core import setup
import py2exe
import sys
import shutil

if len(sys.argv) == 1:
    sys.argv.append("py2exe")
    sys.argv.append("-q")

class Target:
    def __init__(self, **kw):
        self.__dict__.update(kw)
        self.version = "0.9.0"
        self.company_name = "Jan Prochazka"
        self.copyright = "no copyright"
        self.name = "NotoEditor"

notedit = Target(
    description = "NotoEditor",
    script = "notedit.py",
    icon_resources = [(1, "notedit.ico")],
    dest_base = "notedit")

setup(
    options = {"py2exe": {"compressed": 1,
                          "optimize": 2,
                          "bundle_files": 1}},
    zipfile = None,
    console = [notedit],
    )

shutil.copyfile('dist/notedit.exe', 'notedit.exe')