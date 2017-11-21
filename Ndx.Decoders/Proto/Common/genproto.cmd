
SET NSPACE=Common
SET NDXTOOL=..\..\..\bin\netdx.exe
SET PROTOC=..\..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc

%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.%NSPACE% %1

%PROTOC% -I=. --csharp_out=. %~n1%.proto

move *.cs ..\..\Decoders\%NSPACE%\