
SET NDXTOOL=..\..\..\bin\netdx.exe
SET PROTOC=..\..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc

%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-arp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-atm.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-dns.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-http.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-http2.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-icmp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-icmpv6.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-igmp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-ipsec.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-ipx.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-nbipx.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-nbt.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-netbios.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Core packet-ppp.yaml

%PROTOC% -I=. --csharp_out=. packet-arp.proto
%PROTOC% -I=. --csharp_out=. packet-atm.proto
%PROTOC% -I=. --csharp_out=. packet-dns.proto
%PROTOC% -I=. --csharp_out=. packet-http.proto
%PROTOC% -I=. --csharp_out=. packet-http2.proto
%PROTOC% -I=. --csharp_out=. packet-icmp.proto
%PROTOC% -I=. --csharp_out=. packet-icmpv6.proto
%PROTOC% -I=. --csharp_out=. packet-igmp.proto
%PROTOC% -I=. --csharp_out=. packet-ipsec.proto
%PROTOC% -I=. --csharp_out=. packet-ipx.proto
%PROTOC% -I=. --csharp_out=. packet-nbipx.proto
%PROTOC% -I=. --csharp_out=. packet-nbt.proto
%PROTOC% -I=. --csharp_out=. packet-netbios.proto
%PROTOC% -I=. --csharp_out=. packet-ppp.proto

move *.cs ..\..\Decoders\Core\