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

class ISongColumn:
  title=u''
  name=''
  index=-1
  size=200
  preffered_index=0
  
  def __unicode__(self): return self.title
  def getvalue(self,db,id,data): pass
  def setvalue(self,db,id,data,value): pass
  def isdb(self): return False
  #def define_col(self,grid): pass
  def fillattr(self,attr): pass

class DbSongColumn(ISongColumn):
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

class CustomSongColumn(ISongColumn):
  fget=None
  fset=None
  renderer=None
  editor=None

  def __init__(self,name,title,fget,fset,renderer,editor):
    self.name=name
    self.title=title
    self.fget=fget
    self.fset=fset
    self.renderer=renderer
    self.editor=editor
  
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
    

class SongTable(gridlib.PyGridTableBase):
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
    self.allcolumns=[DbSongColumn('title',u"Název"),DbSongColumn('author',u"Autor"),DbSongColumn('group',u"Skupina")]
    self.columns=copy.copy(self.allcolumns)
    gridlib.PyGridTableBase.__init__(self)

    self.odd=gridlib.GridCellAttr()
    self.odd.SetBackgroundColour("sky blue")
    self.even=gridlib.GridCellAttr()
    self.even.SetBackgroundColour("sea green")
    
    self.lastcolcnt=len(self.columns)
    lastrowcnt=len(self.data)
    
    interop.define_objectflag(self,'fill_data',self.do_fill_data)

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


  def add_column(self,name,title,getprop,setprop,renderer,editor):
    col=CustomSongColumn(name,title,getprop,setprop,renderer,editor)
    self.allcolumns.append(col)
    self.columns.append(col)
    return col
  
  def remove_column(self,col):
    if col in self.allcolumns: self.allcolumns.remove(col)
    if col in self.columns: self.columns.remove(col)

  def fill_data(self):
    interop.send_objectflag(self,'fill_data') # do it LAZY !!!

  @staticmethod #!!!because of lazy fill_data
  def do_fill_data(self):
    if not self.db : return
    index=0
    self.data=[]
    
    for song in self.db.getsongsby('id',['id']+[col.name for col in self.columns if col.isdb()]):# ,'title','author','group')):
      self.data.append((int(song[0]),song[1:]))
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
        

#class SongGrid(wx.ListCtrl,listmix.ColumnSorterMixin):
#  db=None
#  
#  def __init__(self, parent):
#    """initialize
#    @type db: L{songdb.SongDB}
#    @type parent: wx.Control
#    """
#    wx.ListCtrl.__init__(self,parent,-1,wx.DefaultPosition,wx.DefaultSize,wx.LC_REPORT|wx.LC_SORT_ASCENDING)
#    #listmix.ColumnSorterMixin.__init__(self)
#    self.fill_data()
#
#  def set_data(self,db):
#    self.db=db
#    self.fill_data()
#
#  def fill_data(self):
#    self.ClearAll()
#    self.InsertColumn(0, u"Název")
#    self.InsertColumn(1, u"Autor")
#    self.InsertColumn(2, u"Skupina")
#    if not self.db : return
#    index=0
#    for song in self.db.getsongsby('id',('title','author','group')):
#      self.Append(song)
#      #self.SetItemData(index,key)
#      index+=1


class SongGrid(gridlib.Grid):
  configxml=config.xml/'dbview'/'songgrid'
  table=None
  onsongclick=None
  searchwin=None
  #lastkeyev=None
 
  def __init__(self, parent):
    gridlib.Grid.__init__(self, parent, -1)
    self.table=SongTable()
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
      if self.onsongclick:
        self.onsongclick(self.table.db,self.table.data[row][0])
    event.Skip(True)
    
  def OnKeyDown(self,ev):
    if ev.GetKeyCode() not in speckeycodes:
      if not self.searchwin : self.searchwin=searchwinu.SearchWindow(ev,self)
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

  def destroysearchwin(self):
    self.searchwin=None
    
  def getcursong(self):
    return (self.table.db,self.table.data[self.GetGridCursorRow()][0])
    
  def add_song_column(self,name,title,getprop,setprop,renderer,editor):
    col=self.table.add_column(name,title,getprop,setprop,renderer,editor)
    self.load_config()
    self.define_columns()
    return col
  
  def remove_song_column(self,col):
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
    