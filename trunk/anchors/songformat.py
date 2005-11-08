# -*- coding: UTF-8 -*-

import interop

class IHeader:
  name=''
  def printheader(self,song,panegrp,realsb): 
    """adds header to panegrp
    
    @param song: has attributes title,author,group
    @type realsb: L{songbook.realsb.RealSBType}
    """
    pass
  def get_title(self): raise NotImplemented


interop.anchor.define('songheader',IHeader)