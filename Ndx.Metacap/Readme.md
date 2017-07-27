# Ndx.Metacap

Library for manipulation of packet trace files. In comparison to other similar libraries the main idea here is to 
create a list of conversations first and then apply additional processing to selected conversation only.


## Suport for multiple PCAPs
It is possible to have a collection of PCAP files as the source for all operations offered by NDX.
Each frame is assigned an offset which is 64 bit value that consists of two parts:

| Lenght (bits) | Meaning | Limits |
|---- | -----|
| 40 | offset in the PCAP file stream | Maximum file size is ~ 500GB. |
| 24 | index of capture file within the collection | Maximum size of the collection is thus 8,388,607 files. |