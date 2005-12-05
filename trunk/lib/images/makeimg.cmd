set cmd=c:\prg\Python24\Lib\site-packages\wx-2.6-msw-unicode\wx\tools\img2py.py
rem %cmd% -u -i -m #FB02FB -n trprev bmp_source/trprev.bmp __init__.py
%cmd% -u -i -m #FF00FF -n trprev bmp_source/trprev.bmp __init__.py
%cmd% -a -u -m #FF00FF -n trnext bmp_source/trnext.bmp __init__.py
%cmd% -a -u -m #FF00FF -n trprev5 bmp_source/trprev5.bmp __init__.py
%cmd% -a -u -m #FF00FF -n trnext5 bmp_source/trnext5.bmp __init__.py
%cmd% -a -u -m #FF00FF -n trsimple bmp_source/trsimple.bmp __init__.py
%cmd% -a -u -m #FF00FF -n treasy bmp_source/treasy.bmp __init__.py
%cmd% -a -u -m #FF00FF -n trorig bmp_source/trorig.bmp __init__.py