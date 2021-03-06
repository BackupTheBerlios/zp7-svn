# -*- coding: UTF-8 -*-

import interop

class IHeader(object):
    name=''
    def printheader(self,song,panegrp,realsb): 
        """adds header to panegrp

        @param song: has attributes title,author,group
        @type realsb: L{songbook.realsb.RealSBType}
        """
        pass
    def get_title(self): raise NotImplemented
    def __unicode__(self): return self.get_title()

class IDistribAlg(object):
    name=''
    def get_title(self): raise NotImplemented
    def creator(self,logpages,sbtype): raise NotImplemented
    def __unicode__(self): return self.get_title()

class ISongDelimiter(object):
    name=''
    def get_title(self): raise NotImplemented
    def printdelimiter(self,panegrp,realsb): 
        """adds header to panegrp

        @type realsb: L{songbook.realsb.RealSBType}
        """
        pass
    def __unicode__(self): return self.get_title()


interop.anchor.define('songheader',IHeader)
interop.anchor.define('distribalg',IDistribAlg)
interop.anchor.define('songdelimiter',ISongDelimiter)
