
# class HookableList(list):
#   __slots__=('_onappend','_ondelitem','_onsetitem')
#   
#   def __init__(self):
#     list.__init__(self)
#     self._onappend=[]
#     self._ondelitem=[]
#     self._onsetitem=[]
#   
#   def hook_append(self,proc):
#     self._onappend.append(proc)
#   
#   def append(self,item):
#     list.append(self,item)
#     for proc in self._onappend: proc(item)
#   
#   def hook_setitem(self,proc):
#     self._onsetitem.append(proc)
#   
#   def __setitem__(self,key,value):
#     list.__setitem__(self,key,value)
#     for proc in self._onsetitem: proc(key,value)
# 
#   def hook_delitem(self,proc):
#     self._ondelitem.append(proc)
#   
#   def __delitem__(self,key):
#     list.__delitem__(self,key)
#     for proc in self._ondelitem: proc(key)

class Hookable:
  def onappend(self,item): 
    pass
  def ondelitem(self,key):
    pass
  def onsetitem(self,key,value):
    pass
  def onremove(self,value):
    pass
  def oninsert(self,index,value):
    pass

class HookableList(list):
  __slots__=('_hooks')
  
  def __init__(self):
    list.__init__(self)
    self._hooks=[]
    
  def hook(self,obj):
    """@type obj: L{Hookable}"""
    self._hooks.append(obj)
  
  def unhook(self,obj):
    self._hooks.remove(obj)

  def append(self,item):
    for obj in self._hooks: obj.onappend(item)
    list.append(self,item)

  def remove(self,item):
    for obj in self._hooks: obj.onremove(item)
    list.remove(self,item)

  def __setitem__(self,key,value):
    list.__setitem__(self,key,value)
    for obj in self._hooks: obj.onsetitem(key,value)
 
  def __delitem__(self,key):
    list.__delitem__(self,key)
    for obj in self._hooks: obj.ondelitem(key)

  def insert(self,index,value):
    for obj in self._hooks: obj.oninsert(index,value)
    list.insert(self,index,value)
 