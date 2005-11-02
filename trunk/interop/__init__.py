# -*- coding: UTF-8 -*-

import anchor_loader
#import weakref

library_directory='' #set by main program

_message_disable_level=0
_message_queue=[]
_flags=set()
_flag_defs={} #str(name)->def()

_anchors={}

def enable_messaging():
  global _message_disable_level
  _message_disable_level-=1
  assert _message_disable_level>=0

def disable_messaging():
  global _message_disable_level
  _message_disable_level+=1
  
def define_flag(flag,fn):
  """defines flag
  
  @type flag: str
  @type fn:def()
  """
  _flag_defs[flag]=fn  

#!!! TODO - objectflags over weak references
def define_objectflag(obj,flag,fn):
  """defines flag, which is joined with object, after deleting object also flag is deleted
  
  @type fn:def(obj), object reference is passed as argument
  """
  _flag_defs[(id(obj),flag)]=lambda: fn(obj)

def send_objectflag(obj,flag):
  _flags.add((id(obj),flag))

def send_message(fn):
  _message_queue.append(fn)

def send_flag(flag):
  _flags.add(flag)
  
def process_messages():
  global _flags
  global _message_queue
  if _message_disable_level>0: return
  for msg in _message_queue: msg()
  for flag in _flags: _flag_defs[flag]()
  _message_queue=[]
  _flags=set()
  
def define_anchor(name,intf):
  """defines new anchor
  
  @type intf: class
  """
  assert not _anchors.has_key(name)
  _anchors[name]=(intf,[])
  
def add_feature(anchor,obj):
  """defines new feature, adds it to anchor
  
  @type anchor: str
  @type obj: defined by define_anchor(anchor,XXX)
  """
  assert _anchors.has_key(anchor)
  assert isinstance(obj,_anchors[anchor][0])
  _anchors[anchor][1].append(obj)
  
def get_features(anchor):
  """return list of features from given anchor
  
  @rtype: list(feature type)
  """
  return _anchors[anchor][1]
  
def initialize():
  pass
#  anchor_loader.load_anchors()