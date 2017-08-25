# NDX-SPARK-SHELL
This document shows how NDX-SPARK can be used to get flow statistics.

## Prepare data source
```scala
import org.ndx.model.Packet;
import org.ndx.model.PacketModel.RawFrame;
import org.ndx.model.Conversations;

val frames = sc.hadoopFile("hdfs://neshpc1.fit.vutbr.cz/user/rysavy/cap/*.cap", 
classOf[org.ndx.spark.pcap.PcapInputFormat], 
classOf[org.apache.hadoop.io.LongWritable], 
classOf[org.apache.hadoop.io.ObjectWritable])

val packets = frames.map(x=> Packet.parsePacket(x._2.get().asInstanceOf[RawFrame]))
val flows = packets.map(x=>(x.getFlowString(),x))
val flows = flows.cache()
```

## Top 10 flows by the number of packets
```scala
val stats = flows.map(x=>(x._1,Conversations.fromPacket(x._2))).reduceByKey(Conversations.merge)
stats.takeOrdered(10)(Ordering[Int].reverse.on(x=> x._2.getPackets())).map(c=>Conversations.format(c._1, c._2)).foreach(println)
```

## Top 20 flows with respect to octets
```scala
stats.takeOrdered(20)(Ordering[Long].reverse.on(x=> x._2.getOctets())).map(c=>Conversations.format(c._1, c._2)).foreach(println)
```

## Top 20 flows with longest duration
```scala
stats.takeOrdered(20)(Ordering[Long].reverse.on(x=> x._2.getLastSeen()-x._2.getFirstSeen())).map(c=>Conversations.format(c._1, c._2)).foreach(println)
```

## Get application flows
To get flows of the specific application/service we need to apply a filter:



