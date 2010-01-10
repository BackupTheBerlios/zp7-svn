mkdir %outputdir%
mkdir %outputdir%\img
copy img\*.png %outputdir%\img

xsltproc xml2html.xslt start.xml>%outputdir%\start.html
xsltproc xml2html.xslt inetdb.xml>%outputdir%\inetdb.html
xsltproc xml2html.xslt mainmenu.xml>%outputdir%\mainmenu.html
xsltproc xml2html.xslt bookstyles.xml>%outputdir%\bookstyles.html
xsltproc xml2html.xslt filters.xml>%outputdir%\filters.html
xsltproc xml2html.xslt howtobook.xml>%outputdir%\howtobook.html
xsltproc xml2html.xslt mainwindow.xml>%outputdir%\mainwindow.html

xsltproc xml2hhc.xslt zp8-hhc.xml>%outputdir%\zp8.hhc
copy zp8.hhp %outputdir%\zp8.hhp
copy zp8.hhk %outputdir%\zp8.hhk
"c:\Program Files\HTML Help Workshop\hhc" %outputdir%\zp8.hhp

copy %outputdir%\zp8.chm zp8.chm

