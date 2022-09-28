@echo off
 
::C#编译器路径
set "toolPath=%cd%\protoc-3.20.2-win64\bin\protoc.exe"

::C#文件生成路径, 最后不要跟“\”符号
set "out_CSharpPath=%cd%\out_CSharp"

::协议源文件路径, 最后不要跟“\”符号
set "source=%cd%\In_protoc"


::删除旧的文件，重新生成
del %out_CSharpPath%\*.* /f /s /q

::遍历全部文件
for /f "delims=" %%i in ('dir /b "%source%\*.proto"') do (
    
     echo gen source/%%i...
     
     %toolPath%  --proto_path="%source%"  --csharp_out="%out_CSharpPath%"   "%source%\%%i" 
 
)

echo Success

pause


