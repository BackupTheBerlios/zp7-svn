# -*- coding: utf-8 -*- 

import wx
import browse
import os
import os.path
import desktop
import utils

class Cond:
  def gensql(self): pass
  def filled(self): return False

class StrCond(Cond):
  text=u''
  tests=[u'je přítomen',u'je roven',u'není přítomen',u'není roven']
  test=0
  fields=[u'Libovolné',u'Název',u'Autor',u'Skupina',u'Text']
  field=0
  fieldnames=['s.title','s.author','g.name','t.songtext']
  
  def __init__(self,brw):
    brw.hbox(border=5)
    brw.edit(model=browse.attr(self,'text'))
    brw.combo(model=self.tests,curmodel=browse.attr(self,'test'))
    brw.combo(model=self.fields,curmodel=browse.attr(self,'field'))
    brw.endsizer()

  def gensql(self):
    jointexts=False
    txt=self.text
    op='LIKE'
    if self.test in (0,2): txt=u'%'+txt+u'%'
    if self.test in (2,3): op='NOT LIKE'
    if self.field>0:
      sql='%s %s :txt%d' % (self.fieldnames[self.field-1],op,id(self))
      jointexts=self.field==4
    else:
      jointexts=True
      sqls=[]
      for fld in self.fieldnames:
        sqls.append('%s %s :txt%d' % (fld,op,id(self)))
      if self.test in (2,3): sql=' AND '.join(sqls)
      else: sql=' OR '.join(sqls)
    return '('+sql+')',{'txt%d'%id(self):txt.encode('utf-8')},{'jointexts':jointexts}

  def filled(self): return self.text!=u''

class FilterDialog:
  brw=None
  conds=[]
  
  def __init__(self):
    self.brw=browse.DialogBrowse(desktop.main_window,u'Filtr písní')
    self.conds=[]
    self.brw.vbox()
    for i in range(3):
      self.conds.append(StrCond(self.brw))
    self.brw.hbox(border=5)
    self.brw.button(text=u'Vyhledat předchozí',event=self.onsearchprev)
    self.brw.button(text=u'Vyhledat další',event=self.onsearchnext)
    self.brw.button(text=u'Filtrovat',event=self.onfilter)
    self.brw.button(text=u'Zavřít',event=self.onclose)
    self.brw.endsizer()
    self.brw.endsizer()

  def run(self):
    return self.brw.run()

  def onsearchprev(self,ev):
    pass

  def onsearchnext(self,ev):
    pass

  def onfilter(self,ev):
    self.brw.saveall()
    self.brw.ok()

  def onclose(self,ev):
    self.brw.cancel()

  def gensql(self):
    pars={}
    callpars={'jointexts':False}
    sqls=[]
    for cond in self.conds:
      if cond.filled():
        sql,actpars,actcallpars=cond.gensql()
        sqls.append(sql)
        if actcallpars['jointexts']:  callpars['jointexts']=True
        pars.update(actpars)
    return '('+' AND '.join(sqls)+')',pars,callpars
    
