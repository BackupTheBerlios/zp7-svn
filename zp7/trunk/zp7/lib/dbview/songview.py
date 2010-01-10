# -*- coding: UTF-8 -*-

import wx
import format
import songtool.transp as transpmod
import format
import config

class SongView(wx.ScrolledWindow):
    text=''
    titlefont=None
    panegrp=None
    pars=None

    def __init__(self, parent, id = -1, size = wx.DefaultSize):
        self.pars=format.SongFormatPars()
        self.pars.fonts['label']=format.SongFont(**config.font.songview_label)
        self.pars.fonts['text']=format.SongFont(**config.font.songview_text)
        self.pars.fonts['chord']=format.SongFont(**config.font.songview_chord)
        wx.ScrolledWindow.__init__(self, parent, id, (0, 0), size=size, style=wx.SUNKEN_BORDER)
        self.Bind(wx.EVT_PAINT, self.OnPaint)
        self.SetBackgroundColour("WHITE")
        self.SetVirtualSize((0,0))
        self.SetScrollRate(20,20)

    def OnPaint(self, event):
        dc = wx.PaintDC(self)
        self.PrepareDC(dc)
        dc.BeginDrawing()
        if self.panegrp:
            self.panegrp.draw(format.DCCanvas(dc))
        dc.EndDrawing()

    def setsong(self,text):
        self.text=text
        dc=wx.WindowDC(self)
        alg=format.SongFormatter(dc,self.pars,text,self.GetSize().Get()[0]-32)
        alg.run()
        self.SetVirtualSize((alg.pagewi,alg.pagehi))
        self.panegrp=alg.panegrp
        self.Refresh()

class SongCtrl(wx.Notebook):
    songv=None
    songsrc=None
    acttr=0
    song=None

    def __init__(self,parent):
        wx.Notebook.__init__(self,parent,-1)

        self.songsrc=wx.TextCtrl(self,-1,style=wx.TE_MULTILINE)
        self.songv=SongView(self)
        self.AddPage(self.songv,u"Text písně")
        self.AddPage(self.songsrc,u"Zdrojový text")
        self.songsrc.SetEditable(False)

    def setsong(self,song):
        self.song=song
        self.transp('orig')
        self.songsrc.SetValue(song.text)

    def transp(self,trtype):
        self.acttr=transpmod.changetr(self.acttr,self.song.text,trtype)
        text=transpmod.transp(self.song.text,self.acttr)
        self.songv.setsong(text)

