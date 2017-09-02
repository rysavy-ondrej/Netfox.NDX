# NDX-SPARK-NETFLOW

This document shows how NDX-SPARK can be used to get flow statistics.

## Prepare data source

```scala
import org.ndx.model.Packet;
import org.ndx.model.PacketModel.RawFrame;
import org.ndx.model.Statistics;

val frames = sc.hadoopFile("hdfs://neshpc1.fit.vutbr.cz/user/rysavy/cap/*.cap", 
classOf[org.ndx.pcap.PcapInputFormat], 
classOf[org.apache.hadoop.io.LongWritable], 
classOf[org.apache.hadoop.io.ObjectWritable])

val packets = frames.map(x=> Packet.parsePacket(x._2.get().asInstanceOf[RawFrame]))
val flows = packets.map(x=>(x.getFlowString(),x))
```

Next we compute stats collection that for each flow collects some statistics:

```scala
val stats = flows.map(x=>(x._1,Statistics.fromPacket(x._2))).reduceByKey(Statistics.merge)
```

Note that ```stats``` is a tuple of type ```(String, FlowAttributes)```.

## Top 10 flows by the number of packets

```scala
stats.takeOrdered(10)(Ordering[Int].reverse.on(x=> x._2.getPackets())).map(c=>Statistics.format(c._1, c._2)).foreach(println)
```

## Top 20 flows with respect to octets

```scala
stats.takeOrdered(20)(Ordering[Long].reverse.on(x=> x._2.getOctets())).map(c=>Statistics.format(c._1, c._2)).foreach(println)
```

## Top 20 flows with longest duration

```scala
stats.takeOrdered(20)(Ordering[Long].reverse.on(x=> x._2.getLastSeen()-x._2.getFirstSeen())).map(c=>Statistics.format(c._1, c._2)).foreach(println)
```

## Get application flows

To get flows of the specific application/service we need to apply a filter:

```scala
stats.filter(x=> Packet.flowKeyParse(x._1).getSourceSelector().toStringUtf8().equals("80")).takeOrdered(20)(Ordering[Long].reverse.on(x=> x._2.getPackets())).map(c=>Statistics.format(c._1, c._2)).foreach(println)
```

The filter is predicate on stat tuple:

```scala
x=> Packet.flowKeyParse(x._1).getSourceSelector().toStringUtf8().equals("80")
```

Because the first component of the tuple is string label of the flow, we parse the label to ```FlowKey``` objects and then 
access source port.
