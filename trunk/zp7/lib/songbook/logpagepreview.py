# -*- coding: UTF-8 -*-

import format

class LogPagePreview:
    sb=None
    def __init__(self,sb): self.sb=sb
    def getcount(self): return 0
    def fmtpagenum(self,pagenum): return 'Strana ???'
    def getpagesize(self): return (0,0)
    def drawpage(self,page,canvas): pass

class SimpleLogPagePreview(LogPagePreview): # triv - ukazuje logicke stranky
    def getcount(self): return len(self.sb.logpages)
    def fmtpagenum(self,pagenum): return 'Strana %d/%d' % (pagenum,self.getcount())
    def getpagesize(self): return self.sb.rbt.pgwi,self.sb.rbt.pghi
    def drawpage(self,page,canvas):
        if page>=0 and page<len(self.sb.logpages): self.sb.logpages[page].draw(canvas)

class BookLogPagePreview(LogPagePreview):
    def getcount(self): return len(self.sb.logpages)/2+1
    def fmtpagenum(self,pagenum):
        txt='Strana '
        if pagenum>0: txt+=str(pagenum*2+0)
        else: txt+='?'
        txt+=','
        if pagenum<self.getcount()-1: txt+=str(pagenum*2+1)
        else: txt+='?'
        return txt

    def getpagesize(self): return self.sb.rbt.pgwi*2,self.sb.rbt.pghi
    def drawpage(self,page,canvas):
        if page*2-1>=0: self.sb.logpages[page*2-1].draw(canvas)
        if page*2<len(self.sb.logpages): self.sb.logpages[page*2].draw(format.SubCanvas(canvas,self.sb.rbt.pgwi,0))

