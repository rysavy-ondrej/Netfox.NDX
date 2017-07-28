@echo COMPILE PROTO FILES 

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model SSH.proto

@echo DONE