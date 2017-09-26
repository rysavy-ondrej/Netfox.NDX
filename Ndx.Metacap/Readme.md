# Ndx.Model

The library provides various data models used in NDX solution. All models are defined
as protocol buffer objects which simplifies their implementation and provides the efficient 
serialization and deserialization operations.

## Suport for multiple PCAPs
It is possible to have a collection of PCAP files as the source for all operations offered by NDX.
Each frame is assigned an offset which is 64 bit value that consists of two parts:

| Lenght (bits) | Meaning | Limits |
|---- | -----|
| 40 | offset in the PCAP file stream | Maximum file size is ~ 500GB. |
| 24 | index of capture file within the collection | Maximum size of the collection is thus 8,388,607 files. |