import wx
import utils
import wx.lib.layoutf  as layoutf
import hooks

class _SizerItem:
  def __init__(self,sizer,kw):
    self.__dict__=kw
    self.sizer=sizer

class _CtrlItem:
  def __init__(self,ctrl,kw):
    self.__dict__=kw
    self.ctrl=ctrl

  def save(self): pass
  def load(self): pass
  def destroy(self):
    if self.ctrl:
      self.ctrl.Destroy()
      self.ctrl=None
    
class _CommonListCtrlItem(_CtrlItem,hooks.Hookable):
  #onremove=None
  _hooked=False
  
  def hook(self):
    if self.model and isinstance(self.model,hooks.HookableList):
      self.model.hook(self)
      self._hooked=True
  
  def unhook(self):
    if self._hooked:
      self.model.unhook(self)
      self._hooked=False
  
  def __init__(self,ctrl,kw):
    _CtrlItem.__init__(self,ctrl,kw)
    self.refresh()
    self.hook()
    self.load()
    
  def load(self):
    if self.curmodel:
      try:
        pos=int(self.curmodel.get())
      except:
        pos=0
      self.ctrl.SetSelection(pos)
      
    if self.valuemodel:
      try:
        pos=self.model.index(self.valuemodel.get())
      except Exception,e:
        pos=-1
      self.ctrl.SetSelection(pos)

  def save(self):
    if self.curmodel:
      try:
        self.curmodel.set(self.ctrl.GetSelection())
      except:
        pass
        
    if self.valuemodel:
      try:
        idx=self.ctrl.GetSelection()
        if idx>=0:
          self.valuemodel.set(self.model[idx])
        else:
          self.valuemodel.set(None)
      except:
        pass

  #def clearhooks(self):
    #self.onremove=None
    
  def refresh(self):
    utils.wx_fill_list(self.ctrl,self.model)

  def __contains__(self,value): 
    return value in self.model
  
  def append(self,item):
    if not self._hooked: self.onappend(item)
    self.model.append(item)
    
  def onappend(self,item):
    self.ctrl.Append(unicode(item))

  def insert(self,index,value):
    if not self._hooked: self.oninsert(index,value)
    self.model.insert(index,value)

  def oninsert(self,index,value):
    self.ctrl.Insert(unicode(value),index)

  def onremove(self,item):
    index=self.model.index(item)
    self.ctrl.Delete(index)

  def remove(self,item):
    if not self._hooked: self.onremove(item)
    self.model.remove(item)

  def getitem(self):
    sel=self.ctrl.GetSelection()
    if sel>=0: return self.model[sel]

  def onexchange(self,idx1,idx2):
    s1=self.ctrl.GetString(idx1)
    s2=self.ctrl.GetString(idx2)
    self.ctrl.SetString(idx1,s2)
    self.ctrl.SetString(idx2,s1)
    
  def exchange(self,idx1,idx2):
    if not self._hooked: self.onexchange(idx1,idx2)
    self.model[idx1],self.model[idx2]=self.model[idx2],self.model[idx1]
    
  def onsetitem(self,key,value):
    self.ctrl.SetString(key,unicode(value))
    
  def ondelitem(self,key):
    self.ctrl.Delete(key)

  def moveup(self):
    sel=self.ctrl.GetSelection()
    if sel>0: 
      self.exchange(sel,sel-1)
      self.ctrl.SetSelection(sel-1)

  def __len__(self): return len(self.model)

  def movedown(self):
    sel=self.ctrl.GetSelection()
    if sel<len(self)-1: 
      self.exchange(sel,sel+1)
      self.ctrl.SetSelection(sel+1)
    
  def eraseact(self):
    sel=self.ctrl.GetSelection()
    if sel>=0:
      #if self.onremove: self.onremove(self.model[sel])
      if not self._hooked: self.ondelitem(sel)
      del self.model[sel]
      #self.ctrl.Delete(sel)
      if sel>0: self.ctrl.SetSelection(sel-1)

  #does not work with hooking
  def clear(self):
    self.model[:]=[]
    self.ctrl.Clear()

  def setmodel(self,model):
    self.unhook()
    self.model=model
    self.hook()
    self.refresh()  

  #does not work with hooking
  def fill(self,items):
    self.model[:]=items
    self.refresh()  

class _ListCtrlItem(_CommonListCtrlItem):
  pass

class _ComboCtrlItem(_CommonListCtrlItem):
  pass

class _LabelCtrlItem(_CtrlItem):
  pass
  
class _DynamicLabelCtrlItem(_CtrlItem):
  def setvalue(self,value):
    self.ctrl.SetLabel(unicode(value))

  def load(self):
    if self.model:
      try:
        if callable(self.model): self.setvalue(self.model())
        else: self.setvalue(self.model.get())
      except:
        self.setvalue(self.default)

class _CheckCtrlItem(_CtrlItem):
  def __init__(self,ctrl,kw,parent):
    _CtrlItem.__init__(self,ctrl,kw)
    if self.autosave or self.event:
      parent.Bind(wx.EVT_CHECKBOX,self.onevent,self.ctrl)
      
  def onevent(self,ev):
    if self.autosave: self.save()
    if self.event: self.event(ev)

  def load(self):
    if self.model:
      try:
        self.ctrl.SetValue(self.model.get())
      except:
        pass

  def save(self):
    if self.model:
      try:
        self.model.set(self.ctrl.GetValue())
      except:
        pass

class _ButtonCtrlItem(_CtrlItem):
  def __init__(self,ctrl,kw):
    _CtrlItem.__init__(self,ctrl,kw)

class _CommonEditCtrlItem(_CtrlItem):
  def __init__(self,ctrl,kw):
    _CtrlItem.__init__(self,ctrl,kw)

  def getvalue(self): return self.ctrl.GetValue()
  def setvalue(self,value): self.ctrl.SetValue(value)

  def save(self):
    if not self.ctrl: return
    if not self.model: return
    self.model.set(self.ctrl.GetValue())

  def load(self):
    if not self.ctrl: return
    if not self.model: return
    self.ctrl.SetValue(self.model.get())

class _EditCtrlItem(_CommonEditCtrlItem):
  pass

class _SpinCtrlItem(_CommonEditCtrlItem):
  def __init__(self,ctrl,kw,parent):
    _CommonEditCtrlItem.__init__(self,ctrl,kw)
    parent.Bind(wx.EVT_SPINCTRL,self.onevent,self.ctrl)

  def onevent(self,ev):
    if self.autosave: self.save()
    if self.event: self.event(ev)

class _PagerCtrlItem(_CtrlItem):
  pass

class _PageCtrlItem(_CtrlItem):
  pass
  
class _PanelCtrlItem(_CtrlItem):
  pass
  
class _ScrollWinCtrlItem(_CtrlItem):
  pass