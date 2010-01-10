# -*- coding: utf-8 -*- 

import utils.xmltools as xmltools
import utils.xmlnode as xmlnode
import utils
import desktop
import browse
import config
import wx
import copy
import realsb
import format
import sbtype

class DistribType:
    BOOK=0
    LINES=1

class A4Distribution:
    hcnt=0
    vcnt=0

    _shcnt=0
    _pgcnt=0
    _lpersh=0 #count of logical pages per sheet
    _virtlogcnt=0 #number of logical pages, also empty
    _virtpages=[] #pages with None,so that len(_virtpages)==_virtlogcnt
    _pages=[] # importonce sequence: sheet,row,col,side

    def pageindex(self,sheet,x,y,side):
        if side: return self._lpersh*sheet+y*self.hcnt*2+x*2+side
        return self._lpersh*sheet+y*self.hcnt*2+(self.hcnt-x-1)*2+side

    def __init__(self,hcnt,vcnt,pages,distribtype=DistribType.BOOK):
        """
        @type pages: list
        """
        pages=list(pages)
        self.hcnt=hcnt
        self.vcnt=vcnt

        self._pgcnt=len(pages)
        self._lpersh=2*vcnt*hcnt
        self._virtlogcnt=self._pgcnt
        while self._virtlogcnt%self._lpersh: self._virtlogcnt+=1
        self._virtpages=pages+[None]*(self._virtlogcnt-len(pages))
        self._pages=[None]*self._virtlogcnt
        self._shcnt=self._virtlogcnt/self._lpersh

        if distribtype==DistribType.LINES:
            actpage=iter(self._virtpages)
            for i in xrange(self._shcnt):
                for l in xrange(self.vcnt):
                    for j in xrange(2):
                        for k in xrange(self.hcnt):
                            self._pages[self.pageindex(i,k,l,j)]=actpage.next()

        elif distribtype==DistribType.BOOK:
            if self.hcnt%2==1: #liche horizontalne
                for i in xrange(self._virtlogcnt): self._pages[i]=self._virtpages[i]
            else: #sude horizontalne
                incer=iter(self._virtpages)
                decer=reversed(self._virtpages)
                for i in xrange(0,self._virtlogcnt,4):
                    self._pages[i+0]=incer.next()
                    self._pages[i+1]=incer.next()
                    self._pages[i+2]=decer.next()
                    self._pages[i+3]=decer.next()

    def getpage(self,sheet,x,y,side):
        """returns page or None

        @param sheet: sheet number
        @param side: 0 or 1
        """
        return self._pages[self.pageindex(sheet,x,y,side)]


    def sheetcnt(self): return self._shcnt
    def freepgcnt(self): return self._shcnt*self.hcnt*self.vcnt*2-self._pgcnt
