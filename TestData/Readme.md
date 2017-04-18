# RocksDB Export Format
This folder contains a sample of RocksDb created for provided http.cap source file.

Metacap file is exported to Rocks DB using two ColumnFamilies (see https://github.com/facebook/rocksdb/wiki/Column-Families):

* *flows* - this family contains flow records
		  
* *packets* - this family contains packets pointers related to each flow

Both families use *key* that is represented by a flow key of the following structure:

```C
_FlowKey struct
{
        uint16 protocol;
        uint8 sourceAddress[16];
        uint8 destinationAddress[16];
        uint16 sourcePort;
        uint16 destinationPort;
        uint16 family;
}
```
The ```_FlowKey``` has fixed length of 40 bytes. It contains the following fields:
* *protocol* - the decoded protocol up to the transport layer (see https://www.iana.org/assignments/protocol-numbers/protocol-numbers.xhtml)
* *sourceAddress* - the source IP address, either IPv4 (4 bytes) or IPv6 (16 bytes)
* *destinationAddress* -the desctination IP address
* *sourcePort* - the sourcve port or other relevant identifier
* *destinationPort* -the destination port or other relevant identifier
* *family* - address family identifier (see https://www.iana.org/assignments/address-family-numbers/address-family-numbers.xhtml)

## Flows Column Family
Flows column family is denoted as ```flows``` and maps ```_FlowKey``` to ```_FlowRecord``` values. Each
flow record is a fixes structure of the following shape:

```C
_FlowRecord struct 
{
    uint64 octets;
    uint32 packets;
    uint64 first;
    uint64 last;
    uint32 blocks;
    uint32 application;
    uint32 reserved;
}
```

## Packets Column Family
Packets column family contains for each flow the table of associated packet metadata called ```PacketBlock```. 
Packet metadata consists of frame information and the four pointers to access the packet content
at the different layer, e.g., link, network, transport or application.

Frame metadata provides basic description of each captured frame, such as its number in the pcap file, 
raw length, absolute offset in the pcap file and the frame timestamp.
```C
_FrameMetadata struct
{
    uint32 frameNumber;
    uint32 frameLength;
    uint64 frameOffset;
    uint64 timestamp;	
}
```

Structure ```_ByteRange``` is a helper that is used to store pointers to the frame content.

```C
_ByteRange struct
{
    int32 start;
    int32 count;
}
```
Packet metadata strcuture comprise of frame description and four pointers. If the frame does not contain 
transport data, for example, then ```transport``` value equals to ```{ start=0, count=0 }```.

```C
_PacketMetadata struct
{
    _FrameMetadata frame;
    _ByteRange link;
    _ByteRange network;
    _ByteRange transport;
    _ByteRange payload;
}
```

Finally, each key is assigned to value that is represented by the following structure. 

```C
_PacketBlock struct
{
	int32 count;
	_PacketMetadata items[count];
}
```

This structure is a serialized array of packet metadata. 

## Test Data
This folder contains an example of pcap file (```http.pcap```), generated metacap file and exported RocksDB representation (folder ```http.rdb```). Finally, the content of RocksDB is exported to ```http.json``` file.

To reproduce results, please, apply the following steps:

```
$ metacap.exe -r http.cap Create-Index

$ metacap.export.exe -r http.cap -w http.rdb ConvertTo-Rocks

$ metacap.export.exe -r http.rdb Show-Rocks > http.json
```
Note that json file presents the content of the RocksDb in more human readable form:

* Key value is decoded and represented as a string
* Time information is converted from Unix timestamp to usual UTC format