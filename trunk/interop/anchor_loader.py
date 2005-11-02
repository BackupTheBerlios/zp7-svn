# -*- coding: UTF-8 -*-

import os
import os.path
import sys

import interop

def load_anchors():
  d=os.path.join(interop.library_directory,'anchors')
  for f in os.listdir(d):
    f=os.path.join(d,f)
    if os.path.isfile(f) and os.path.splitext(f)[1].upper()=='.PY':
      f=os.path.splitext(os.path.split(f)[1])[0]
      __import__('anchors.'+f)
