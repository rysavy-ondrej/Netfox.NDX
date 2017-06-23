# TShark Usage

TShark can be used to provide parsed information in form of JSON 
output as follows:
```
tshark -r source.pcap -T json
```

## Decode conversation using specified protocol parser

```-d <layer type>==<selector>,<decode-as protocol>```

This switch can be applied if the specific decoder should be used to parse the conversation content.
For example: ```-d tcp.port==8888,http``` will decode any traffic running over TCP port 8888 as HTTP.

## Specifying output fields
By default, TShark parses each frame and provide all available information. 
When only specific fields are in the interest, the output can specified using -e flag:

```-e <field>```

Add a field to the list of fields to display if -T fields is selected. This option can be used multiple times on the command line.
For example, to print relevant information from HTTP requests:
* ```http.request.method```
* ```http.request.uri```
* ```http.request.version```
* ```http.host```
* ```http.user_agent```
* ```http.accept```
* ```http.accept_language```
* ```http.accept_encoding```
* ```http.connection```
* ```http.referer```
* ```http.request.full_uri```
Similarly, information from HTTP response may be obtained from the following fields:
* ```http.response.code```
* ```http.content_type```
* ```http.content_encoding```
* ```http.server```
* ```http.cache_control```
* ```http.content_length```
* ```http.date```
* ```http.file_data```


## Packet sources for TSHARK
### STDIN
It is possible to run TSHARK in a mode that it reads input PCAP from STDIN:

```
tshark -r - 
```
### NamedPipes
It is also possible to send data to TSHARK using named pipes.

```
tshark -i \\.\pipe\tsharkpipe
```
