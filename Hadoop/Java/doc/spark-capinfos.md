# NDX-SPARK-CAPINFOS


First we precompute information about the pcap file:

```scala
import org.ndx.model.Packet;
import org.ndx.model.PacketModel.RawFrame;
import org.ndx.model.Statistics;

val frames = sc.hadoopFile("hdfs://neshpc1.fit.vutbr.cz/user/rysavy/cap/*.cap", 
classOf[org.ndx.spark.pcap.PcapInputFormat], 
classOf[org.apache.hadoop.io.LongWritable], 
classOf[org.apache.hadoop.io.ObjectWritable])

val packets = frames.map(x=> Packet.parsePacket(x._2.get().asInstanceOf[RawFrame]))

val capinfo = packets.map(x => Statistics.fromPacket(x)).reduce(Statistics.merge)
```

##  Number of packets
```scala
capinfo.getPackets()
```

## Capture Duration
```scala
Statistics.ticksToSeconds((capinfo.getLastSeen() - capinfo.getFirstSeen()))
```

## First packet time:
```scala
Statistics.ticksToDate(capinfo.getFirstSeen())
```

## Last packet time:
```scala
Statistics.ticksToDate(capinfo.getLastSeen())
```

## Data byte rate:
```scala
capinfo.getOctets() / Statistics.ticksToSeconds((capinfo.getLastSeen() - capinfo.getFirstSeen()))
```

## Data bit rate:
```scala
(capinfo.getOctets() / Statistics.ticksToSeconds((capinfo.getLastSeen() - capinfo.getFirstSeen()))) * 8
```

## Average packet size:
```scala
capinfo.getMeanPayloadSize()
```

## Average packet rate: 
```scala
capinfo.getPackets() / ( (capinfo.getLastSeen() - capinfo.getFirstSeen()) / 10000000 )
```