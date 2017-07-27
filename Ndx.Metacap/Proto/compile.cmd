@echo COMPILE PROTO FILES 

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model Constants.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model FlowModel.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model PacketModel.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model McapModel.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model ConversationModel.proto

@echo DONE