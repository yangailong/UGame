@echo off

::Э���ļ�·��, ���Ҫ����\������
set source=In_protoc
 
::C#������·��
set toolPath=protoc-3.20.2-win64\bin\protoc.exe

::C#�ļ�����·��, ���Ҫ����\������
set out_CSharpPath=out_CSharp


::ɾ����ǰ�������ļ�
del %out_CSharpPath%\*.* /f /s /q

::����ȫ���ļ�
for /f "delims=" %%i in ('dir /b "%source%\*.proto"') do (

     ::���� C# ����
     echo %toolPath% -i:%%i -o:%out_CSharpPath%\%%~ni.cs
          %toolPath% -i:%%i -o:%out_CSharpPath%\%%~ni.cs
)

echo Success

pause


