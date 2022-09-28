@echo off
 
::C#������·��
set "toolPath=%cd%\protoc-3.20.2-win64\bin\protoc.exe"

::C#�ļ�����·��, ���Ҫ����\������
set "out_CSharpPath=%cd%\out_CSharp"

::Э��Դ�ļ�·��, ���Ҫ����\������
set "source=%cd%\In_protoc"


::ɾ���ɵ��ļ�����������
del %out_CSharpPath%\*.* /f /s /q

::����ȫ���ļ�
for /f "delims=" %%i in ('dir /b "%source%\*.proto"') do (
    
     echo gen source/%%i...
     
     %toolPath%  --proto_path="%source%"  --csharp_out="%out_CSharpPath%"   "%source%\%%i" 
 
)

echo Success

pause


