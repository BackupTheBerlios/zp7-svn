import xml.sax.saxutils as saxutils
import xml.sax
import copy
from cStringIO import StringIO

class XmlNode:
    name=u""
    text=u""
    childs=[]
    attrs={}

    def __init__(self,name='xml'):
        self.name=name
        self.childs=[]
        self.attrs={}
        pass

    def add(self,name):
        res=XmlNode(name)
        self.childs.append(res)        
        return res;

    def forcesub(self,name):
        res=self.findsub(name)
        if res>=0: return self.childs[res]
        return self.add(name)

    def findsub(self,name):
        """searchs sub with name, returns index or -1, if not found

        @rtype: int"""
        childs=self.childs
        for i in xrange(len(self.childs)):
            if childs[i].name==name:
                return i
        return -1

    def save(self,f):
        f.write("<"+self.name);
        for a in self.attrs.keys():
        #f.write(' %s=%s'%(a,saxutils.quoteattr(unicode(self.attrs[a]).encode('utf-8'))))
            f.write((u' %s=%s'%(a,saxutils.quoteattr(unicode(self.attrs[a])))).encode('utf-8'))
        if len(self.childs)>0:
            f.write(">\n")
            for c in self.childs:
                c.save(f)
            f.write("</%s>\n"%(self.name,))
        elif self.text!="":
            f.write(">")
            f.write(saxutils.escape(self.text.encode('utf-8')));
            #f.write(saxutils.escape(unicode(self.text).encode('utf-8')));
            f.write("</%s>\n"%(self.name,))
        else:
            f.write("/>\n")

    @staticmethod
    def load(fr):
        handler=XmlContentHandler()
        parser=xml.sax.make_parser()
        parser.setContentHandler(handler)
        parser.parse(fr)
        return handler.xml

    @staticmethod
    def fromstr(s):
        return XmlNode.load(StringIO(s))

    def tostr(self):
        fw=StringIO()
        self.save(fw)
        return fw.getvalue()

    def clear(self):
        self.childs[:]=[]
        self.attrs.clear()
        self.text=u''

    def assign(self,src):
        self.clear()
        self.text=src.text
        self.attrs=copy.copy(src.attrs)
        for x in src: self.add(x.name).assign(x)

    def __getitem__(self,name):
        try:
            return self.attrs[name]
        except:
            return u''

    def __setitem__(self,name,value):
        self.attrs[name]=unicode(value)

    def __delitem__(self,name):
        if self.attrs.has_key(name):
            del self.attrs[name]

    def __div__(self,name):
        return self.forcesub(name)

    def __iter__(self):
        return iter(self.childs)

    def __contains__(self,name):
        return self.findsub(name)>=0


class XmlContentHandler(xml.sax.handler.ContentHandler):
    stack=[]
    xml=None

    def __init__(self):
        self.stack=[]

    def startElement(self,name,attrs):
        node=XmlNode(name)
        #node.attrs=copy.copy(attrs)
        node.attrs=dict(zip(attrs.keys(),attrs.values()))
        #for attr in attrs.keys(): node.attrs[attr]=unicode(attrs[attr],'utf-8')
        #node.attrs=dict(zip(map(unicode,attrs.keys()),map(unicode,attrs.values()))) #copy dictionary, convert to unicode
        if self.xml:
            self.stack[-1].childs.append(node)
        else:
            self.xml=node

        self.stack.append(node)

    def endElement(self,name):
        del self.stack[-1]

    def characters(self,content):
        self.stack[-1].text+=content
