# -*- coding: UTF-8 -*-

import wx
import wx.grid as gridlib
import code
import locale
import utils
import searchwinu
import code
import utils.dialogs
import copy
import desktop
import config
import interop

speckeynames=[d for d in dir(wx) if d.startswith('WXK_')]
speckeycodes=frozenset([getattr(wx,name) for name in speckeynames])-frozenset([wx.WXK_SPACE])

class IDBColumn:
  title=u''
  name=''
  index=-1
  size=200
  preffered_index=0
  coldef=None # DBColumnDef
  
  def __unicode__(self): return self.title
  def getvalue(self,db,id,data): pass
  def setvalue(self,db,id,data,value): pass
  def isdb(self): return False
  #def define_col(self,grid): pass
  def fillattr(self,attr): pass

class DbColumn(IDBColumn):
  order=0
  name=''
  
  def getvalue(self,db,id,data):
    return data[self.order]

  def __init__(self,name,title):
    self.name=name
    self.title=title

  def isdb(self): return True
  
  def fillattr(self,attr): 
    attr.SetReadOnly(True)

class CustomDBColumn(IDBColumn):
  fget=None
  fset=None
  renderer=None
  editor=None
  coldef=None

  def __init__(self,coldef):
    self.coldef=coldef
    self.name=coldef.name
    self.title=coldef.title
    self.fget=coldef.fget
    self.fset=coldef.fset
    self.renderer=coldef.renderer
    self.editor=coldef.editor
  
  def getvalue(self,db,id,data):
    return self.fget(db,id)

  def setvalue(self,db,id,data,value):
    self.fset(db,id,value)

  #def define_col(self,grid):
    #typename='coltype_%d'%id(self)
    #grid.RegisterDataType(typename,self.renderer(),self.editor())
    #grid.SetColFormatCustom(self.index,typename)

  def fillattr(self,attr): 
    attr.SetRenderer(self.renderer())
    attr.SetEditor(self.editor())
    

class DBTable(gridlib.PyGridTableBase):
  db=None
  data=[]
  columns=[]
  allcolumns=[]
  #dbcolumns=[] #analysed from database
  #extracolumns=[] #added by add_column
  
  lastcolcnt=0
  lastrowcnt=0
  
  sortcol=0
  sortdesc=False

  def __init__(self):
    self.data=[]
    self.allcolumns=self.getbasecolumns()
    self.columns=copy.copy(self.allcolumns)
    gridlib.PyGridTableBase.__init__(self)

#     self.odd=gridlib.GridCellAttr()
#     self.odd.SetBackgroundColour("sky blue")
#     self.even=gridlib.GridCellAttr()
#     self.even.SetBackgroundColour("sea green")
    
    self.lastcolcnt=len(self.columns)
    lastrowcnt=len(self.data)
    
    interop.define_objectflag(self,'fill_data',self.do_fill_data)

  def getbasecolumns(self):
    """ABSTRACT
    
    @return: list of L{DbColumn}
    """
    raise NotImplemented

  def retrieve_data(self,columns): 
    """ABSTRACT
    
    @param columns: list of names of columns
    @return: iterator over tuples of values
    """
    raise NotImplemented


  def GetNumberRows(self):
    return len(self.data)

  def GetNumberCols(self):
    return len(self.columns)

  def IsEmptyCell(self, row, col):
    return False

  def GetValue(self, row, col):
    return self.columns[col].getvalue(self.db,self.data[row][0],self.data[row][1])
    #self.data[row][1][col]

  def SetValue(self, row, col, value):
    return self.columns[col].setvalue(self.db,self.data[row][0],self.data[row][1],value)

  def GetColLabelValue(self, col):
    return unicode(self.columns[col]) #(u"Název",u"Autor",u"Skupina")[col]
  
  def set_data(self,db):
    self.db=db
    self.fill_data()

  def define_columns(self,grid):
    #self.GetView().ProcessTableMessage(gridlib.GridTableMessage(self,gridlib.GRIDTABLE_NOTIFY_COLS_DELETED,0,len(self.columns)))

    curdbcol=0
    for col in self.columns:
      if col.isdb():
        col.order=curdbcol
        curdbcol+=1
    
    d=len(self.columns)-self.lastcolcnt
    if d>0:
      self.GetView().ProcessTableMessage(gridlib.GridTableMessage(self,gridlib.GRIDTABLE_NOTIFY_COLS_APPENDED,d))
    else:
      self.GetView().ProcessTableMessage(gridlib.GridTableMessage(self,gridlib.GRIDTABLE_NOTIFY_COLS_DELETED,0,-d))
    self.lastcolcnt=len(self.columns)

    for i in range(len(self.columns)):
      col=self.columns[i]
      col.index=i
      
      grid.SetColSize(i,col.size)
      
      attr=gridlib.GridCellAttr()
      col.fillattr(attr)
      #attr.SetReadOnly(col.readonly())
      grid.SetColAttr(i,attr)

      #for col in self.columns: col.define_col(grid)


  def add_column(self,coldef):
    col=CustomDBColumn(coldef)
    self.allcolumns.append(col)
    self.columns.append(col)
    return col
  
  def remove_column(self,coldef):
    for c in self.allcolumns:
      if c.coldef==coldef:
        self.allcolumns.remove(c)
        if c in self.columns: self.columns.remove(c)
        return

  def fill_data(self,immediately=False):
    if immediately: self.do_fill_data(self)
    else: interop.send_objectflag(self,'fill_data') # do it LAZY !!!

  @staticmethod #!!!because of lazy fill_data
  def do_fill_data(self):
    if not self.db : return
    index=0
    self.data=[]
    
    for row in self.retrieve_data(['id']+[col.name for col in self.columns if col.isdb()]):
      self.data.append((int(row[0]),row[1:]))
      index+=1
      
    self.refresh()

  def refresh(self):
    d=len(self.data)-self.lastrowcnt
    if d>0:
      self.GetView().ProcessTableMessage(gridlib.GridTableMessage(self,gridlib.GRIDTABLE_NOTIFY_ROWS_APPENDED,d))
    else:
      self.GetView().ProcessTableMessage(gridlib.GridTableMessage(self,gridlib.GRIDTABLE_NOTIFY_ROWS_DELETED,0,-d))
    self.lastrowcnt=len(self.data)

  def sort(self,col):
    if self.sortcol==col:
      self.sortdesc=not self.sortdesc
    else:
      self.sortdesc=False
      self.sortcol=col
      
    dlg=wx.ProgressDialog(u"Zpěvníkátor",u"Probíhá řazení ...",maximum=0,parent=utils.main_window)
    self.data.sort(locale.strcoll,lambda x:unicode(x[1][col]).upper(),self.sortdesc)
    dlg.Destroy()

  def wantsearchdata(self):
    self.db.getsearchtexts()

  def qsearchindex(self,text,index,dir):
    text=utils.make_search_text(text)
    searchtexts=self.db.getsearchtexts()
    data=self.data
    if dir>0:
      cur=index+1
      while cur<len(data):
        if searchtexts[data[cur][0]].find(text)>=0 : return cur
        cur+=1
      cur=0
      while cur<index:
        if searchtexts[data[cur][0]].find(text)>=0 : return cur
        cur+=1
    if dir<0:
      cur=index-1
      while cur>=0:
        if searchtexts[data[cur][0]].find(text)>=0 : return cur
        cur-=1
      cur=len(data)-1
      while cur>index:
        if searchtexts[data[cur][0]].find(text)>=0 : return cur
        cur-=1
        
  def searchid(self,id):
    index=0
    for sid,xx in self.data:
      if id==sid: return index
      index+=1
    return -1

class DBGrid(gridlib.Grid):
  configxml=None #must be set by ancestor
  table=None
  onrowclick=None
  searchwin=None
  #lastkeyev=None
 
  def __init__(self, parent,tableclass):
    gridlib.Grid.__init__(self, parent, -1)
    self.table=tableclass()
    # The second parameter means that the grid is to take ownership of the
    # table and will destroy it when done.  Otherwise you would need to keep
    # a reference to it and call it's Destroy method later.
    self.SetTable(self.table, False)
    self.Bind(gridlib.EVT_GRID_LABEL_LEFT_CLICK,self.OnLabelLeftClick)
    self.Bind(gridlib.EVT_GRID_SELECT_CELL,self.OnCellSelect)
    #self.Bind(wx.EVT_RIGHT_UP, self.OnRightClick)
    self.Bind(gridlib.EVT_GRID_CELL_RIGHT_CLICK,self.OnRightClick)
    self.Bind(gridlib.EVT_GRID_COL_SIZE,self.OnColSize)
    self.Bind(wx.EVT_KEY_DOWN, self.OnKeyDown)
    #self.Bind(wx.EVT_KEY_UP, self.OnKeyUp)
    #self.Bind(wx.EVT_CHAR, self.OnChar)
    self.SetColLabelSize(20)
    self.SetRowLabelSize(0)
    #self.SetColSize(0,200)
    #self.SetColSize(1,200)
    #self.SetColSize(2,200)
    self.SetSelectionMode(gridlib.Grid.SelectCells)
    self.EnableDragRowSize(False)
    #self.EnableEditing(False)

  def set_data(self,db):
    self.table.set_data(db)
    self.load_config()
    self.define_columns()
    #self.refresh()

  def refresh(self):
    #self.SetTable(self.table,False)
    #self.ProcessTableMessage(gridlib.GridTableMessage(self.table,gridlib.GRIDTABLE_NOTIFY_ROWS_APPENDED,self.table.GetNumberRows()))
    self.table.refresh()
    self.ForceRefresh()
  
  def OnLabelLeftClick(self, event):
    if event.GetRow()==-1:
      self.table.sort(event.GetCol())
    self.refresh()
  
  def OnCellSelect(self,event):
    row=event.GetRow()
    cnt=len(self.table.data)
    if row>=0 and row<cnt:
      if self.onrowclick:
        self.onrowclick(self.table.db,self.table.data[row][0])
    event.Skip(True)
    
  def OnKeyDown(self,ev):
    if ev.GetKeyCode() not in speckeycodes:
      self.table.wantsearchdata()
      if not self.searchwin : 
        self.searchwin=searchwinu.SearchWindow(ev,self)
        x,y=self.ClientToScreenXY(*self.GetSizeTuple())
        w,h=self.searchwin.GetSizeTuple()
        x-=w
        y-=h
        self.searchwin.SetPosition((x,y))
      self.searchwin.Show()
    else:
      ev.Skip()
  
  def OnColSize(self,event):
    event.Skip()
    col=event.GetRowOrCol()
    self.table.columns[col].size=self.GetColSize(col)
    self.save_config()

  #callbacks from searchwindow
  def qsearch(self,text,dir):
    index=self.table.qsearchindex(text,self.GetGridCursorRow(),dir)
    if index>=0:
      cell=(index,self.GetGridCursorCol())
      self.SetGridCursor(*cell)
      self.MakeCellVisible(*cell)

  def setcurid(self,id):
    index=self.table.searchid(id)
    if index>=0:
      cell=(index,self.GetGridCursorCol())
      self.SetGridCursor(*cell)
      self.MakeCellVisible(*cell)
  
  def destroysearchwin(self):
    self.searchwin=None
    
  def getcurdbtuple(self):
    return (self.table.db,self.table.data[self.GetGridCursorRow()][0])
    
  def add_column(self,col):
    self.table.add_column(col)
    self.load_config()
    self.define_columns()
  
  def remove_column(self,col):
    self.table.remove_column(col)
    self.define_columns()

  def define_columns(self):
    self.table.define_columns(self)
    self.table.fill_data()
    #self.refresh()
    
  def OnVisibleColumns(self,event):
    if utils.dialogs.edit_ordered_set(desktop.main_window,self.table.columns,self.table.allcolumns,unicode,u'Viditelné sloupce',u'Všechny sloupce',u'Výběr sloupců'):
      self.define_columns()
      self.save_config()
    
  def OnRightClick(self,event):
    menu=wx.Menu()
    mid=wx.NewId()
    menu.Append(mid,u"Viditelné sloupce")
    self.Bind(wx.EVT_MENU,self.OnVisibleColumns,id=mid)
    self.PopupMenu(menu,event.GetPosition())
    menu.Destroy()

  def save_config(self):
    for col in self.table.allcolumns:
      colx=self.configxml/col.name
      colx['size']=col.size
      colx['visible']=int(col in self.table.columns)
      if col in self.table.columns: colx['index']=self.table.columns.index(col)
      else: del colx['index']
      
  def load_config(self):
    self.table.columns=[]
    for col in self.table.allcolumns: 
      if col.name in self.configxml:
        colx=self.configxml/col.name
        col.size=int(colx['size'])
        if int(colx['visible']):
          self.table.columns.append(col)
          col.preffered_index=int(colx['index'])
      else:
        self.table.columns.append(col)
        
    self.table.columns.sort(lambda x,y:cmp(x.preffered_index,y.preffered_index))
    