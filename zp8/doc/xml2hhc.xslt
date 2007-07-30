<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
  <xsl:output method="html" encoding="Cp1250"/>

  <xsl:template match="/">
    <HTML>
      <HEAD>
        <meta name="GENERATOR" content="xml2hhc"/>
        <!-- Sitemap 1.0 -->
      </HEAD>
      <BODY>
        <UL>
          <xsl:apply-templates/>
    		</UL>
    	</BODY>
    </HTML>
  </xsl:template>

  <xsl:template match="item">
    <LI>
      <OBJECT type="text/sitemap">
		    <param name="Name">
		      <xsl:attribute name="value">
		        <xsl:value-of select="@title"/>
		      </xsl:attribute>
        </param>
        <xsl:if test="@href">
          <param name="Local">
  		      <xsl:attribute name="value">
              <xsl:value-of select="concat(@href,'.html')"/>
            </xsl:attribute>
          </param>
        </xsl:if>
		  </OBJECT>
		</LI>
	  <UL>
	    <xsl:apply-templates/>
	  </UL>
  </xsl:template>
	  
</xsl:stylesheet>