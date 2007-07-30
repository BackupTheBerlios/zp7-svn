mkdir %outputdir%
mkdir %outputdir%\img
copy img\*.png %outputdir%\img

xsltproc xml2html.xslt start.xml>%outputdir%\start.html
xsltproc xml2html.xslt inetdb.xml>%outputdir%\inetdb.html

xsltproc xml2hhc.xslt zp8-hhc.xml>%outputdir%\zp8.hhc
copy zp8.hhp %outputdir%\zp8.hhp
copy zp8.hhk %outputdir%\zp8.hhk
hhc %outputdir%\zp8.hhp

copy %outputdir%\zp8.chm zp8.chm

