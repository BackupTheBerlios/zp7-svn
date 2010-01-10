# -*- coding: UTF-8 -*-

import interop
import anchors.songformat as songformat

def printtext(text,font,panegrp,realsb):
    pane=panegrp.addpane()
    realsb.dc.SetFont(realsb.getfont(font).getwxfont())
    w,h=realsb.dc.GetTextExtent(text)
    pane.hi=h
    pane.canvas.font(realsb.getfont(font))
    pane.canvas.text(0,0,text)

class StdHeader(songformat.IHeader):
    name='std'

    def get_title(self): return u'Název+Autor'

    def printheader(self,song,panegrp,realsb):
        printtext(song.title,'title',panegrp,realsb)
        printtext(song.author,'author',panegrp,realsb)

class OnlyTitleHeader(songformat.IHeader):
    name='title'

    def get_title(self): return u'Název'

    def printheader(self,song,panegrp,realsb):
        printtext(song.title,'title',panegrp,realsb)


interop.anchor['songheader'].add_default(StdHeader())
interop.anchor['songheader'].add_feature(OnlyTitleHeader())


class StdDistribAlg(songformat.IDistribAlg):
    name='normal'  
    def get_title(self): return u'Normální rozdělení'
    def creator(self,logpages,sbtype):
        import distribalg,a4distrib

        if sbtype.a4distribtype==a4distrib.DistribType.BOOK:
            return distribalg.BookDistribAlg(logpages)

        if sbtype.a4distribtype==a4distrib.DistribType.LINES:
            res=distribalg.LinesDistribAlg(logpages)
            res.hcnt=sbtype.hcnt
            return res

        return distribalg.SimpleDistribAlg(logpages)

class SimpleDistribAlg(songformat.IDistribAlg):
    name='simple'
    def get_title(self): return u'Jednoduché rozdělení'
    def creator(self,logpages,sbtype):
        import distribalg
        return distribalg.SimpleDistribAlg(logpages)

interop.anchor['distribalg'].add_default(StdDistribAlg())
interop.anchor['distribalg'].add_feature(SimpleDistribAlg())

class SongDelimiterBase(songformat.ISongDelimiter):

    def printdelimiter(self,panegrp,realsb):
        pane=panegrp.addpane()
        realsb.dc.SetFont(realsb.getfont('text').getwxfont())
        w,h=realsb.dc.GetTextExtent('M')
        pane.hi=h*2
        self.drawdelimiter(pane,realsb)
        pane.delim=True

    def drawdelimiter(self,pane,realsb): 
        pass

class StdDelimiter(SongDelimiterBase):
    name='std'
    def get_title(self): return u'Jednoduchá čára'

    def drawdelimiter(self,pane,realsb):
        pane.canvas.line(0,pane.hi/2,realsb.pgwi,pane.hi/2)


class EmptyDelimiter(SongDelimiterBase):
    name='space'
    def get_title(self): return u'Mezera'


interop.anchor['songdelimiter'].add_default(StdDelimiter())
interop.anchor['songdelimiter'].add_feature(EmptyDelimiter())

