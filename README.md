# NDX
The network diagnostic framework (NDX) implements environment for analysis of pcap files,
netflow records and log files in order to provide diagnostic information on network device
and network services behavior.


## Architecture

The NDX architecture represents a processing pipeline that comprises of several stages:
* Data Source - provides access to different data sources. 
* Ingest - prepares and process source data. The data are then ready for analysis. 
* Analyze - applies different analytical methods to acquire information value from the data.
* Publication - transforms data to make is suitable for presentation or further processing by external system.
* Data Consumption - sends data to external system.






## Design Notes

### Packet Traces
Packet trace files contain data for every packet. The trace files can easily be very large, 
and generally require further data processing to reveal pertinent characteristics of the traffic.

### Flow Table
Network Flow is the sequence of packets or a packet that belonged to certain network session(conversation) between two end points.

The endpoint identification can be done at different levels:

| Layer     |   Endpoints                   |
|-----------|-------------------------------|
| Link      | Source MAC, Destination MAC   |
| Network   | Source IP, Destination IP     |
| Transport | Source Port, Destination Port |

The NDX considers flow with 5-tuple granularity. The flow key consists of 
source IP address, destination IP address, source port, destination port, IP protocol identifier.

Mechanisms to identify the start and the end of a flow must be defined. 
There are three primary expiry methods that are appropriate for studying 
characteristics of individual flows: 

* protocol based, 
* fixed timeout, and 
* adaptive timeout 

Flow attributes are used to describe a flow. 
They can be values from the fields in headers of packets, counters (total bytes, total packets, etc.) or summary attributes such as means, median, and variance. 

| Feature  | Description | 
|-----------|-------------------------------|
| duration | Length (number of seconds) of the connection  | 
| protocol | Type of the protocol, e.g., TCP, UDP, etc | 
| service  | Network service provided, e.g., HTTP, SMTP | 
| bytes | Number of data bytes from source to destination |
| packets | Number of data packets from source to destination |
| flag | Normal or error status of the connection |


### Conversation Table
A network conversation is the traffic between two specific application endpoints, 
denoted as the originator and the responder. The structure of conversation key is the same as
the structure of the flow key. The Originator is defined as the sender of 
the first captured packet associated to a 5-tuple key. If the trace file is incomplete, 
packets of originator may be missing and thus the roles specified in the key (originator vs responders) may not reflect reality.




# References
* https://centauri.stat.purdue.edu:98/netsecure/Papers/flowattributes_ademontigny.pdf