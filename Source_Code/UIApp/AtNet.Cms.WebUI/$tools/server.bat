@echo off
echo .NET�������й���(.NET 4.0)
echo.
echo ������������������µ�ַ����,�������������
echo ����serverb.vbs��̨����
echo.

::����˿�
set port=80

::������(����������Ҫ�趨)
set host=localhost

::�����˿�
set port2=8000


cd ../
start /b $tools\server-proxy.exe -host %host% -port %port% -proxy http://localhost:%port2%
start /b $tools\server_console.exe /a:./ /pm:Specific /port:%port2%"