SET NDXTOOL=..\..\Tools\netdx\bin\Debug\netdx.exe
SET DISSECTORS_PATH=..\..\..\wireshark\epan\dissectors
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-arp.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-bootp.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-dns.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-eth.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-ftp.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-http.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-icmp.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-imap.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-ip.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-ipsec.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-isakmp.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-pop.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-raw.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-sip.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-smb.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-smb2.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-smtp.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-ssh.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-ssl.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-tcp.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-telnet.c
%NDXTOOL% Generate-TypeInfo -o YAML %DISSECTORS_PATH%\packet-udp.c
