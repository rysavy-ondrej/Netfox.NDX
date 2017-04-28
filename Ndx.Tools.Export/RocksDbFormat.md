# RocksDB Export Format
This document specifies structures and format of data types for storing index and metainformation 
for the PCAP file content in RocksDB.

Metacap file is exported to Rocks DB using the following column families:

* *pcaps* - this family contains a collection of ingested PCAP files.
* *flows* - this family contains flow records		  
* *packets* - this family contains packets pointers related to each flow

## PCAP Column Family
The PCAP column family is a collection of items that describe PCAP files. Each PCAP file
has a unique id within this collection. For each PCAP, its URI and other information is stored 
in this table. The key of PCAP table rows is represented by structure:
```C
_PcapId struct
{
    uint16 pcapId;
}
```
Where:
* *pcapId* - unique identifier of the PCAP record.

The value part of PCAP table row is represented by the following strcuture:
```C
_PcapFile struct
{
    uint16 pcapType;
    uint16 uriLength;
    char uri[uriLength];
    byte md5signature[16];
    byte shasignature[20];
}
```
Where:
* *pcapType* - the format of PCAP file. It can be one of these: LIBPCAP, PCAPNG, WINCAP.
* *uriLength* - the total length of the URI field including terminating byte.
* *uri* - null terminated string containing URI of the PCAP. Usually, relative URI is used.
* *md5signature* - hash value of the PCAP computed using MD5 algorithm.
* *shasignature* - hash value of the PCAP computed using SHA algorithm.


PCAP column family is mapping from pcap id to pcap data:
```C
ColumnFamily<_PcapKey, _PcapValue> flows;
```

## Flows Column Family
Flows column family is a collection of flows. As flow key 5-tuple is not a unique representation 
in all situations, e.g. reuse of the same port pairs, analysis of tunneled communication 
between private networks, it cannot be used as a table key. Thus for each new flow, the unique flow id 
is created (possibly in sequential manner). 

```C
_FlowId struct
{
    uint32 flowId;
}
```

```_FlowId``` is a key that is used in several collections, namely:
* *flows.key* - a collection of flow keys.
* *flows.record* - a collection of flow records.
* *flows.features* - a collection of extended flow features.

In addition, it is possible to create a new collection with other features 
without changing the basic data model.


### Flows.Key

Each flow value consists of flow key and flow record. 
```C
_FlowKey struct
{
        uint16 protocol;
        uint8 sourceAddress[16];
        uint8 destinationAddress[16];
        uint16 sourcePort;
        uint16 destinationPort;       
}
```
The ```_FlowKey``` has fixed length of 42 bytes. The meaning of the fields is as follows:
* protocol - identification of the protocol (ip, ipv6, icmp, igmp, tcp, udp, etc.)
* sourceAddress - the source address of the flow
* destinationAddress - the destination address of the flow
* sourcePort - the source port or 0 for flows without this information 
* destinationPort - the destination port or 0 for flows that do not have this field
* flowId - a flow identifier that mainly serves to distinguish between flows 
   that shares the same key 5-tuple. Because flowId is also used to reference
   flow records it should be unique in the database.

Flows.Key column family is mapping from flow id to flow key data:
```C
ColumnFamily<_FlowId, _FlowKey> flows.key;
```

### Flows.Record
This collection contains flow records, that is basic information on each flow. 
The fields in this collection is similar to netflow records.
```C
_FlowRecord struct 
{
    uint64 octets;
    uint32 packets;
    uint64 first;
    uint64 last;
    uint32 blocks;
    uint32 application;
}
```
The meaning of individual fields is following:

* *octets* - number of bytes of the flow.
* *packets* - number of packets of the flow.
* *first* - timestamp of the first packet of the flow.
* *last* - timestampt of the last packet of the flow.
* *blocks* - number of packet blocks of the flow. Block are sequentially numbered starting from 0. 
* *application* -identification of recognized application/service of the flow.


Flows.Record column family is mapping from flow id to flow record data:
```C
ColumnFamily<_FlowId, _FlowRecord> flows.record;
```

### Flows.Features
It is possible to extract additional features that may be useful for 
further flow-based (statistical) analysis.




## Packets Column Family
Packets column family contains for each flow the table of associated packet metadata called ```PacketBlock```. 
Packet metadata consists of frame information and some pointers to access the packet content
at the different layer, e.g., link, network, transport or application.

Packet block family uses a comopsed key, that consits of flow identification and 
the block sequence number.

```C
_PacketBlockId
{
    uint32 flowId;
    uint32 blockId;
}
```
Where:
* *flowId* is a flow identifier.
* *blockId* is a sequence number of the block. This number is unique int the flow scope.


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

Structure ```_ByteRange``` is a helper that is used to store pointers to frame content.

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

Packets column family consists of values represented by ```_PacketBlock``` stucture: 
```C
_PacketBlock struct
{
    _PcapId pcapRef;
    int32 count;
    _PacketMetadata items[count];
}
```
where:
* *pcapRef* - reference to pcap file. 
* *count* - number of ```_PacketMetadata``` items in the packet block.
* *items* - an array of ```_PacketMetadata``` values.

Packets column family has defined as follows:

```C
ColumnFamily<_PacketBlockId, _PacketBlock> packets;
```


## Q&A

### What is the purpose of PacketBlocks?
The reason why to store the packets in blocks using a different table is that packets metadata 
refers to target pcap files. As there may be several pcap files storing packets for a single flow, 
packet blocks stores a collection packets in a single pcap file. Then flow can be associated with 
one or more packet block. Other reason may be that for large flows, storing all packets metadata 
within a single row in the database may not be practical. Thus, packet metadata is stored in 
reasonable large packet blocks. 


### How do I access packet for the flow?
Field ```_FlowRecord.blocks``` denotes a number of blocks of packets associated with the flow.
For instance, ```flowId=2345``` and ```blocks=3``` means that there are 3 blocks in the packet column family 
associated with the flow 2345. To get these blocks use the following keys:
```{ flowId = 2345, blockId = 0 }```, 
```{ flowId = 2345, blockId = 1 }```,
```{ flowId = 2345, blockId = 2 }``` to query Packets column family.