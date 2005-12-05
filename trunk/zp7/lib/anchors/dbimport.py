# -*- coding: UTF-8 -*-

import interop

class IImportFilter(object):
  def getsongxml(self,files):
    """@rtype: lxml.etree.ElementTree"""
    pass

interop.anchor.define('importfilter',IImportFilter)