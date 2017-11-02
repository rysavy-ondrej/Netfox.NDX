# Ndx.Decoders

## Load from resource file
It is possible to load `DecodingReceipt` from a resource file. There are number of protocols packed along this library. 
For instance loading receipt from deconding TCP protocol from resources can be done easily as:

```csharp
var tcpReceipt = LoadFrom("tcp", new MemoryStream(Resources.packet_tcp));    
```