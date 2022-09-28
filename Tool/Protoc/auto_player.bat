@echo off

::协议文件路径, 最后不要跟“\”符号
set source=In_protoc
 
::C#编译器路径
set toolPath=protoc-3.20.2-win64\bin\protoc.exe

::C#文件生成路径, 最后不要跟“\”符号
set out_CSharpPath=out_CSharp


::删除以前建立的文件
del %out_CSharpPath%\*.* /f /s /q

::遍历全部文件
for /f "delims=" %%i in ('dir /b "%source%\*.proto"') do (

     ::生成 C# 代码
     echo %toolPath% -i:%%i -o:%out_CSharpPath%\%%~ni.cs
          %toolPath% -i:%%i -o:%out_CSharpPath%\%%~ni.cs
)

echo Success

pause


