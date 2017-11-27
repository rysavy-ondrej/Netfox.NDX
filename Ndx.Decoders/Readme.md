# Ndx.Decoders
The project provides a collection of protocol decoders and representation objects. 
Decoders enable to read JSON input produced by the tshark tool:

```bash
$ tshark -T ek -r input.pcap
```

Protocols are categorized in the following groups:
* Base: Ethernet, Frame, IEEE802.11, IP, IPv6, LLC, TCP, UDP
* Core:	ARP, ATM, HTTP, HTTP2, ICMP, ICMPv6, IGMP, IPSEC, IPX, NBIPX, NBT, NETBIOS, PPP
* Common: ...

Protocol specifications are provided by yaml files found in Proto subfolders. 
Each protocol specification is a collection of protocol fields. Each field
has given name, type, info (description) and json name. For instance, 
part of Ethernet protocol specification is as follows:
```yaml
Name: eth 
Fields:
  eth.dst:
    Name: eth.dst
    Type: FtEther
    Info: Destination
    JsonName: eth_eth_dst
  eth.src:
    Name: eth.src
    Type: FtEther
    Info: Source
    JsonName: eth_eth_src
```
This information is used to generate representational object and decoder for each protocol.
Decoder is generated directly from YAML specification. Representation object is generated in form of 
Protobuf message. 

Source protocol specifications are stored in `Proto` subfolders.
Target C# classes generated from the source specification are stored in `Decoders` subfolder.
Decoder method is generated in the partial class of the same name as the representational class.


## DecoderFactory
DecoderFactory aids in finding and creating packet decoders. It enumerates all available decoders in runtime thus
it is not necessary to manually register newly added protocol decoder. Each protocol decoder needs to provide 
static decode method, for instance:

```csharp
public partial class Udp {
	public static Udp DecodeJson(JToken token) { ... }
}
```

DecoderFactory object is usually used with PacketDecoder (see bellow). The direct use of
DecoderFactory is also possible, as show in the following example:

```csharp
var factory = new DecoderFactory();
var dns = factory.DecodeProtocol("dns", JToken.Parse(dnsStr)) as Dns;
```

The factory is used to decode input JSON by DNS decoder.

## PacketDecoder
PacketDecoder accepts JSON input that represents a single packet and produces correspoding Packet object.

```csharp
var factory = new DecoderFactory();
var decoder = new PacketDecoder();
var decodedPacket = decoder.Decode(factory, jsonPacket);
```

## Reading and decoding packets
The following example shows reading packets in JSON representation and decoding them using 
implemented decoders.

```csharp
var input = // path to JSON file

var instream = File.OpenRead(input);
var factory = new DecoderFactory();
var decoder = new PacketDecoder();
var packets = new List<Packet>();
using (var pcapstream = new PcapJsonStream(new StreamReader(instream)))
{
	string jsonLine;
	while ((jsonLine = pcapstream.ReadPacketLine()) != null)
	{
		var packet = decoder.Decode(factory, jsonLine);
		packets.Add(packet);
	}
}  
```