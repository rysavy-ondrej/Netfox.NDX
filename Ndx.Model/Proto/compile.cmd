@echo Generating C# output

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model Constants.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model FlowModel.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model PacketModel.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --csharp_out=..\Model ConversationModel.proto

@echo DONE C# OUTPUT
@echo Compiling JAVA Output

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --java_out=../Model Constants.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --java_out=../Model FlowModel.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --java_out=../Model PacketModel.proto

..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc -I=. --java_out=../Model ConversationModel.proto


@echo DONE JAVA output