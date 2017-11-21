# Ndx.Decoders
The projects provides a collection of protocol decoders and representation objects. 
Decoders are categorized in the following classes:
* Base: Ethernet, Frame, IEEE802.11, IP, IPv6, LLC, TCP, UDP
* Core:	ARP, ATM, HTTP, HTTP2, ICMP, ICMPv6, IGMP, IPSEC, IPX, NBIPX, NBT, NETBIOS, PPP
* Common: ...


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
using (var reader = new StreamReader(File.OpenRead(input)))
{
    var factory = new DecoderFactory();
    var decoder = new PacketDecoder();
    using(var stream = new PcapJsonStream(reader))
	{
		JsonPacket packet;
		while((packet = stream.ReadPacket()) != null)
		{
			var decodedPacket = decoder.Decode(factory, packet);
			Console.WriteLine(decodedPacket);
		}
	}
}  
```