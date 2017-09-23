# Ndx.Ingest.Trace

Library for manipulation of packet trace files. In comparison to other similar libraries the main idea here is to 
create a list of conversations first and then apply additional processing to selected conversation only.

In particular, this library provides the following features:

* Computation of PCAP index to improve efficiency of PCAP manipulation
* Sequential access to PCAP files
* Access to packets which are part of a specific conversation
* Stream export (similar to Follow Stream function of Wireshark)
* Efficient access to packet related to conversations.

