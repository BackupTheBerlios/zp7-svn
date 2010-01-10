# -*- coding: UTF-8 -*-

import wx
import re

class SongCanvas:
    def text(self,x,y,text) : raise NotImplemented
    def font(self,font) : raise NotImplemented
    def line(self,x1,y1,x2,y2): raise NotImplemented
    def dynamic_text(self,x,y,fn): pass
    def image(self,image,x,y): pass
    def stretchimage(self,image,x,y,w,h): pass

_scaled_image_cache={}
_bitmapped_image_cache={}

class DCCanvas(SongCanvas):
    dc=None
    def __init__(self,dc) : self.dc=dc
    def text(self,x,y,text) : self.dc.DrawText(text,x,y)
    def font(self,font) : self.dc.SetFont(font.getwxfont());self.dc.SetTextForeground(font.color)
    def line(self,x1,y1,x2,y2): self.dc.DrawLine(x1,y1,x2,y2)
    def dynamic_text(self,x,y,fn): 
        try:
            self.dc.DrawText(unicode(fn()),x,y)
        except:
            pass

    def image(self,image,x,y):
        if len(_bitmapped_image_cache)>128: _bitmapped_image_cache.clear()
        if not _bitmapped_image_cache.has_key(id(image)): _bitmapped_image_cache[id(image)]=wx.BitmapFromImage(image)
        bmp=_bitmapped_image_cache[id(image)]
        self.dc.DrawBitmap(bmp,x,y)

    def stretchimage(self,image,x,y,w,h):
        if len(_scaled_image_cache)>128: _scaled_image_cache.clear()
        if not _scaled_image_cache.has_key((id(image),w,h)): 
            newimage=image.Copy()
            newimage.Rescale(w,h)
            _scaled_image_cache[(id(image),w,h)]=newimage
        self.image(_scaled_image_cache[(id(image),w,h)],x,y)

class MemoryCanvas(SongCanvas):
    items=[]

    def __init__(self):
        self.items=[]    

    def text(self,x,y,text):
        self.items.append(('text',(x,y,text))) 

    def font(self,font): 
        self.items.append(('font',(font,)))

    def line(self,x1,y1,x2,y2):
        self.items.append(('line',(x1,y1,x2,y2)))

    def dynamic_text(self,x,y,fn):
        self.items.append(('dynamic_text',(x,y,fn)))

    def image(self,image,x,y):
        self.items.append(('image',(image,x,y)))

    def stretchimage(self,image,x,y,w,h):
        self.items.append(('stretchimage',(image,x,y,w,h)))

    def draw(self,canvas):
        for fn,args in self.items : getattr(canvas,fn)(*args)


# class _SubCanvasProc:
#   name=''
#   subcanvas=None
#   def __init__(self,subcanvas,name):
#     self.name=name
#     self.subcanvas=subcanvas
# 
#   def __call__(self,*args):
#     getattr(subcanvas.canvas,self.name)(*args)

class SubCanvas(SongCanvas):
    canvas=None
    x=0
    y=0
    def __init__(self,canvas,x,y):
        self.canvas=canvas
        self.x=x
        self.y=y

    def text(self,x,y,text) : self.canvas.text(x+self.x,y+self.y,text)
    def font(self,font) : self.canvas.font(font)
    def line(self,x1,y1,x2,y2): self.canvas.line(x1+self.x,y1+self.y,x2+self.x,y2+self.y)
    def dynamic_text(self,x,y,fn) : self.canvas.dynamic_text(x+self.x,y+self.y,fn)
    def image(self,image,x,y): self.canvas.image(image,x+self.x,y+self.y)
    def stretchimage(self,image,x,y,w,h): self.canvas.stretchimage(image,x+self.x,y+self.y,w,h)

    #def __getattr__(self,name) : return _SubCanvasProc(self,name)

