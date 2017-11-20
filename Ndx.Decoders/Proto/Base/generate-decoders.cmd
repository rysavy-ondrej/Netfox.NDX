
SET NDXTOOL=..\..\..\bin\netdx.exe
SET PROTOC=..\..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc

%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-eth.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-frame.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-ieee80211.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-ip.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-ipv6.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-llc.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-tcp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-udp.yaml


%PROTOC% -I=. --csharp_out=. packet-eth.proto
%PROTOC% -I=. --csharp_out=. packet-frame.proto
%PROTOC% -I=. --csharp_out=. packet-ieee80211.proto
%PROTOC% -I=. --csharp_out=. packet-ip.proto
%PROTOC% -I=. --csharp_out=. packet-ipv6.proto
%PROTOC% -I=. --csharp_out=. packet-llc.proto
%PROTOC% -I=. --csharp_out=. packet-tcp.proto
%PROTOC% -I=. --csharp_out=. packet-udp.proto

move *.cs ..\..\Decoders\Base\