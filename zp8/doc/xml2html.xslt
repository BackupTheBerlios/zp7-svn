<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
  <xsl:output method="html" encoding="Cp1250"/>

  <xsl:template match="/">
    <html>
      <head>
        <meta name="GENERATOR" content="xml2html"/>
        <title><xsl:value-of select="/help/@title"/></title>
      </head>
      <body>
        <h1><xsl:value-of select="/help/@title"/></h1>
        <xsl:apply-templates/>
    	</body>
    </html>
  </xsl:template>

  <xsl:template match="//list/list">
    <li>
      <b><xsl:value-of select="@title"/></b>
      <xsl:if test="desc">
        - <xsl:value-of select="desc"/>
      </xsl:if>
      <ul>
        <xsl:apply-templates/>
      </ul>
    </li>
  </xsl:template>

  <xsl:template match="//list/desc"></xsl:template>

  <xsl:template match="list">
    <h2><xsl:value-of select="@title"/></h2>
    <xsl:if test="desc">
      <xsl:value-of select="desc"/>
    </xsl:if>
    <ul>
      <xsl:apply-templates/>
    </ul>
  </xsl:template>

  <xsl:template match="//list/item">
    <li>
      <xsl:if test="@title">
        <b><xsl:value-of select="@title"/></b> - 
      </xsl:if>
      <xsl:apply-templates/>
    </li>
  </xsl:template>

  <!-- Přímé HTML tagy -->
  <xsl:template match="img">
    <img>
      <xsl:attribute name="src">
        <xsl:value-of select="@src"/>
      </xsl:attribute >
    </img>
  </xsl:template>
  <xsl:template match="a">
    <a>
      <xsl:attribute name="href">
        <xsl:value-of select="concat(@href,'.html')"/>
      </xsl:attribute >
    </a>
  </xsl:template>
  <xsl:template match="ul"><ul><xsl:apply-templates/></ul></xsl:template>
  <xsl:template match="li"><li><xsl:apply-templates/></li></xsl:template>
  <xsl:template match="b"><b><xsl:apply-templates/></b></xsl:template>
  <xsl:template match="p"><p><xsl:apply-templates/></p></xsl:template>
  <xsl:template match="br"><br/></xsl:template>
  <xsl:template match="h1"><h1><xsl:value-of select="."/></h1></xsl:template>
  <xsl:template match="h2"><h2><xsl:value-of select="."/></h2></xsl:template>

</xsl:stylesheet>
