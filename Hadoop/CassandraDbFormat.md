# Cassandra Export Format


Metacap file is loaded in to Cassandra DB and populates the two tables:

* *flows* this table contains flow records
		  
* *packets* this table contains packets pointers realted to each flow


## Flows Table
Flows table is declared by the following statement:

```SQL
CREATE TABLE flows (
    flowId int,
    pcapUri text,
    protocol smallint, 
    sourceAddress inet, 
    destinationAddress inet, 
    sourcePort int, 
    destinationPort int,
    service text, 
    packets int, 
    bytes bigint, 
    firstSeen timestamp, 
    lastSeen timestamp,
    PRIMARY KEY ((sourceAddress, destinationAddress), sourcePort, destinationPort, protocol, flowId)
); 
```

The primary key is composed of:
* *sourceAddress* - the source IP address of the flow.
* *destinationAddress*  - the festination IP address of the flow.
* *sourcePort* - the source port of the flow if available.
* *destinationPort* - the destination port of the flow if avialable.
* *protocol* - the code of the flow protocol (usually, tcp, udp)
* *flowId* - the unique flow identification generated for each new flow.

Other columns are:
* *pcapUri* - the URI of the capture file that contains the flow packets.
* *service* - an identified service protocol.
* *packets* - a number of packets of the flow.
* *bytes* - a number of bytes of the flow.
* *firstSeen* - a timestamp of the first packet in the flow.
* *lastSeen* - a timestamp of the last packet in the flow.
  
The primary key is split into partition key (sourceAddress, destinationAddress)
and clustering columns (sourcePort, destinationPort, protocol, flowId). 
By this definition, all communication between a pair of hosts is always stored 
within the same partition.

## Packets Table
Packets table contains packet metadata for each frame in the pcap.
Packet metadata consists of frame information and the four pointers to access the packet content
at the different layer, e.g., link, network, transport or application.

```SQL
CREATE TYPE byteRange (
    start int,
    count int,
);
```

Frame metadata provides basic description of each captured frame, such as its number in the pcap file, 
raw length, absolute offset in the pcap file and the frame timestamp.
```SQL
CREATE TABLE packets (
    frameNumber int,
    frameLength int,
    frameOffset bigint,
    frameTime timestamp,
    linkProtocol smallint,
    linkData byteRange,
    networkData byteRange,
    transportData byteRange,
    payloadData byteRange,
    flowId int,
    PRIMARY KEY (frameNumber)
);
```
In order to quickly find all packet metadata for a single flow, secondary index is created:
```SQL
CREATE INDEX packetFlowId ON packets (flowId);
```

## Features
Computing flow features is the first step in the analysis phase. As there may be 
various feature sets there is not a single table for all of them. Instead, each 
feature set uses its own table. The notation requirement is that such table
uses ```FlowFeatures``` suffix.

The predefined flow features set table is ```basicFlowFeatures``` defined as follows:

```SQL
CREATE TABLE basicFlowFeatures (
    application text,
    packets int,
    bytes bigint,
    minPayloadSize int,
    meanPayloadSize int,
    maxPayloadSize int,
    stdevPayloadSize int,
    minInterTime int,
    meanInterTime int,
    maxInterTime int,
    stdevInterTime int,
    duration int,
    startTime int,
    PRIMARY KEY (flowId)
)
```

Conversation attributes are as follows:

* Application protocol as reported by selected classification method
* Total number of packets sent 
* Total number of bytes sent 
* Minimum payload size sent 
* Mean payload size sent 
* Maximum payload size sent 
* Standard deviation of payload size sent 
* Minimum packet interarrival time for packets sent 
* Mean packet interarrival time for packets sent 
* Maximum packet interarrival time for packets sent 
* Standard deviation of packet interarrival time for packets sent
* Flow duration (in microseconds)
* Flow start time (as a Unix timestamp)




## Content
Packet and flow content is stored in HDFS rather than in Cassandra. The 
natural representation is to create a pcap file for each flow. This pcap is 
named by UUID and referenced under this identifier from flows table, for example:
```
hdfs://ndx/flows/6c84fb90-12c4-11e1-840d-7b25c5ee775a
```
