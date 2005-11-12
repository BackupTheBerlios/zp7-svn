import wx
import utils
import ctrlitem
import wx.lib.stattext as stattext

def _ctrlpars(ext):
  """adds to dictionary ext ctrl default parameters
  
  @rtype: dict
  @type ext: dict
  """
  ext['layoutflags']=0
  ext['id']=''
  ext['proportion']=0
  return ext

def _layoutpars(ext):
  """adds to dictionary ext layout default parameters
  
  @rtype: dict
  @type ext: dict
  """
  ext['border']=0
  ext['flags']=wx.ALL|wx.EXPAND
  ext['layoutflags']=0
  ext['proportion']=0
  return ext

class BrowseBase:
  stack=[] #sizers, parents
  #sizerstack=[]
  ctrls=[]
  #parentstack=[]
  named_ctrls={}

  def __init__(self,parent):
    self.parent=parent
    self.stack=[]
    self.ctrls=[]
    #self.parentstack=[]
    self.named_ctrls={}

  def _split_args(self,kw,mykws):
    mypars={}
    for mykw in mykws:
      if kw.has_key(mykw):
        mypars[mykw]=kw[mykw]
        del kw[mykw]
      else:
        mypars[mykw]=mykws[mykw]
    return (kw,mypars)
  
  def listbox(self,**kw):
    """creates ListBox control
  
    @param model: list-like object, which is model (data source) for visual control
    """
    kw,mypars=self._split_args(kw,_ctrlpars({'event':lambda ev:ev.Skip(),'model':[],'curmodel':None,'valuemodel':None}))
    ctrl=wx.ListBox(self._getparent(),-1,**kw)
    self.parent.Bind(wx.EVT_LISTBOX,mypars['event'],ctrl)
    self._after_create(ctrlitem._ListCtrlItem(ctrl,mypars),**mypars)

  def _getparent(self):
    for x in reversed(self.stack):
      if isinstance(x,ctrlitem._CtrlItem):
        return x.ctrl
    return self.parent

  def _sizersontop(self):
    res=0
    for x in reversed(self.stack):
      if not isinstance(x,ctrlitem._SizerItem): return res
      res+=1
    return res
  
  def _getsizer(self):
    if not self.stack: return None
    if not isinstance(self.stack[-1],ctrlitem._SizerItem): return None
    return self.stack[-1].sizer

  def combo(self,**kw):
    """creates ComboBox control
  
    @param model: list-like object, which is model (data source) for visual control
    """
    kw,mypars=self._split_args(kw,_ctrlpars({'event':lambda ev:ev.Skip(),'model':[],'curmodel':None,'valuemodel':None}))
    ctrl=wx.ComboBox(self._getparent(),-1,style=wx.CB_DROPDOWN|wx.CB_READONLY,**kw)
    self.parent.Bind(wx.EVT_COMBOBOX,mypars['event'],ctrl)
    self._after_create(ctrlitem._ComboCtrlItem(ctrl,mypars),**mypars)

  def button(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({'event':lambda ev:ev.Skip(),'text':''}))
    ctrl=wx.Button(self._getparent(),-1,mypars['text'],**kw)
    self.parent.Bind(wx.EVT_BUTTON,mypars['event'],ctrl)
    res=ctrlitem._ButtonCtrlItem(ctrl,mypars)
    self._after_create(res,**mypars)
    return res

  def label(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({'text':'','model':None,'default':''}))
    if mypars['model']:
      ctrl=stattext.GenStaticText(self._getparent(),-1,mypars['text'],**kw)
      res=ctrlitem._DynamicLabelCtrlItem(ctrl,mypars)
      res.load()
    else:
      ctrl=wx.StaticText(self._getparent(),-1,mypars['text'],**kw)
      res=ctrlitem._LabelCtrlItem(ctrl,mypars)
    if res: self._after_create(res,**mypars)
    return res

  def check(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({'text':'','model':None,'autosave':False,'event':None,'value':False}))
    ctrl=wx.CheckBox(self._getparent(),-1,mypars['text'],**kw)
    if mypars['model']: ctrl.SetValue(mypars['model'].get())
    else: ctrl.SetValue(mypars['value'])
    self._after_create(ctrlitem._CheckCtrlItem(ctrl,mypars,self.parent),**mypars)

  def pager(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({}))
    ctrl=wx.Notebook(self._getparent(),-1,**kw)
    self._after_create_parent(ctrlitem._PagerCtrlItem(ctrl,mypars),**mypars)

  def panel(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({}))
    ctrl=wx.Window(self._getparent(),-1,**kw)
    res=ctrlitem._PanelCtrlItem(ctrl,mypars)
    self._after_create_parent(res,**mypars)
    return res

  def page(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({'text':''}))
    nb=self._getparent() #self.parentstack[-1].ctrl
    assert isinstance(nb,wx.Notebook)
    ctrl=wx.Window(nb,-1)
    nb.AddPage(ctrl,mypars['text'])
    self._after_create_parent(ctrlitem._PageCtrlItem(ctrl,mypars),**mypars)

  def edit(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({'text':'','model':None}))
    ctrl=wx.TextCtrl(self._getparent(),-1,**kw)
    if mypars['model']: ctrl.SetValue(mypars['model'].get())
    else: ctrl.SetValue(mypars['text'])
    self._after_create(ctrlitem._EditCtrlItem(ctrl,mypars),**mypars)

  def spin(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({'value':0,'model':None,'autosave':False,'event':None}))
    ctrl=wx.SpinCtrl(self._getparent(),-1,**kw)
    if mypars['model']: ctrl.SetValue(mypars['model'].get())
    else: ctrl.SetValue(mypars['value'])
    self._after_create(ctrlitem._SpinCtrlItem(ctrl,mypars,self.parent),**mypars)

  def scrollwin(self,**kw):
    kw,mypars=self._split_args(kw,_ctrlpars({'onpaint':lambda ev:ev.Skip(),'color':None}))
    kw['style']=wx.SUNKEN_BORDER
    ctrl=wx.ScrolledWindow(self._getparent(),-1,**kw)
    ctrl.Bind(wx.EVT_PAINT,mypars['onpaint'],ctrl)
    ctrl.SetScrollRate(20,20)
    if mypars['color']: ctrl.SetBackgroundColour(mypars['color'])
    res=ctrlitem._ScrollWinCtrlItem(ctrl,mypars)
    self._after_create(res,**mypars)
    return res
      
  def space(self,size=(1,1)):
    sizer=self._getsizer()
    if sizer: 
      sizer.Add(size)

  def _after_create_parent(self,obj,**kw):
    self._after_create(obj,**kw)
    self.stack.append(obj)

  def endparent(self):
    del self.stack[-1]
      
  def _after_create(self,obj,**kw):
    id=kw['id']
    if id: self.named_ctrls[id]=obj
    self.ctrls.append(obj)
    self._addtolayout(obj.ctrl,**kw)
  
  def _addtolayout(self,obj,**kw):
    sizer=self._getsizer()
    if sizer:
      x=self.stack[-1]
      sizer.Add(obj,kw['proportion'],x.flags|kw['layoutflags'],x.border)
  
  def _sizer(self,sizer,**kw):
    obj=ctrlitem._SizerItem(sizer,kw)
    self._addtolayout(obj.sizer,**kw)
    self.stack.append(obj)

  def _boxsizer(self,type,**kw):
    kw,mypars=self._split_args(kw,_layoutpars({}))
    sizer=wx.BoxSizer(type)
    self._sizer(sizer,**mypars)
  
  def vbox(self,**kw):
    self._boxsizer(wx.VERTICAL,**kw)

  def hbox(self,**kw):
    self._boxsizer(wx.HORIZONTAL,**kw)

  def grid(self,**kw):
    kw,mypars=self._split_args(kw,_layoutpars({'rows':1,'cols':1,'vgap':0,'hgap':0}))
    sizer=wx.GridSizer(mypars['rows'],mypars['cols'],mypars['vgap'],mypars['hgap'])
    self._sizer(sizer,**mypars)
    
  def endsizer(self):
    sizer=self._getsizer()
    if sizer:
      parent=self._getparent()
      if parent and self._sizersontop()==1:
        parent.SetSizer(sizer)
        if len(self.stack)>1: parent.Bind(wx.EVT_SIZE,lambda ev:parent.Layout())
        self.onendfinalsizer(sizer)
    del self.stack[-1]
  
  def __getitem__(self,name):
    return self.named_ctrls[name]

  def onendfinalsizer(self,sizer):
    pass

  def saveall(self):
    for item in self.ctrls: item.save()

  def loadall(self):
    for item in self.ctrls: item.load()

  def destroy(self): 
    for item in reversed(self.ctrls): item.destroy()
    self.stack[:]=[]
    self.ctrls[:]=[]
    self.named_ctrls.clear()
    self.parent.SetSizer(None)
 
 