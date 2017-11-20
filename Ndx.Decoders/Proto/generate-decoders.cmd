
SET NDXTOOL=..\..\Tools\netdx\bin\Debug\netdx.exe
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-arp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-bootp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-dns.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-eth.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-ftp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-http.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-icmp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-imap.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-ip.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-ipsec.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-isakmp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-pop.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-raw.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-sip.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-smb.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-smb2.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-smtp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-ssh.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-ssl.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-tcp.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-telnet.yaml
%NDXTOOL% Generate-Proto -o . -j -n Ndx.Decoders.Basic YAML\packet-udp.yaml

SET PROTOC=..\..\packages\Google.Protobuf.Tools.3.3.0\tools\windows_x86\protoc
%PROTOC% -I=. --csharp_out=. packet-arp.proto
%PROTOC% -I=. --csharp_out=. packet-bootp.proto
%PROTOC% -I=. --csharp_out=. packet-dns.proto
%PROTOC% -I=. --csharp_out=. packet-eth.proto
%PROTOC% -I=. --csharp_out=. packet-ftp.proto
%PROTOC% -I=. --csharp_out=. packet-http.proto
%PROTOC% -I=. --csharp_out=. packet-icmp.proto
%PROTOC% -I=. --csharp_out=. packet-imap.proto
%PROTOC% -I=. --csharp_out=. packet-ip.proto
%PROTOC% -I=. --csharp_out=. packet-ipsec.proto
%PROTOC% -I=. --csharp_out=. packet-isakmp.proto
%PROTOC% -I=. --csharp_out=. packet-pop.proto
%PROTOC% -I=. --csharp_out=. packet-raw.proto
%PROTOC% -I=. --csharp_out=. packet-sip.proto
%PROTOC% -I=. --csharp_out=. packet-smb.proto
%PROTOC% -I=. --csharp_out=. packet-smb2.proto
%PROTOC% -I=. --csharp_out=. packet-smtp.proto
%PROTOC% -I=. --csharp_out=. packet-ssh.proto
%PROTOC% -I=. --csharp_out=. packet-ssl.proto
%PROTOC% -I=. --csharp_out=. packet-tcp.proto
%PROTOC% -I=. --csharp_out=. packet-telnet.proto
%PROTOC% -I=. --csharp_out=. packet-udp.proto