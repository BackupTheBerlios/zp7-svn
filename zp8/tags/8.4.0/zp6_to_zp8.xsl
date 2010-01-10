<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet 
    version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:sdb="http://zpevnik.net/InetSongDb.xsd" >
  <xsl:template match="/">
    <sdb:SongDb>
      <xsl:apply-templates select='/zpevnik_data/database/song|/zpevnik/items/song' mode='song'/>
    </sdb:SongDb>
  </xsl:template>

  <xsl:template match="song" mode='song'>
    <sdb:song>
      <sdb:title>
        <xsl:value-of select='@title'/>
      </sdb:title>
      <sdb:groupname>
        <xsl:value-of select='@group'/>
      </sdb:groupname>
      <sdb:author>
        <xsl:value-of select='@author'/>
      </sdb:author>
      <sdb:lang>
        <xsl:value-of select='@lang'/>
      </sdb:lang>
      <sdb:songtext>
        <xsl:value-of select='text'/>
      </sdb:songtext>
    </sdb:song>
  </xsl:template>

</xsl:stylesheet>
