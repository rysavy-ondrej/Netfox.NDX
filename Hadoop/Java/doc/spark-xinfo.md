# NDX-SPARK-XINFO

This doucment shows how to use NDX to get HTTP request information for HTTP sessions.

We are able to parse Http requests uing HttpRequest class. 


## Prepare data source

```scala
implicit def toConsumer[A](function: A => Unit): java.util.function.Consumer[A] = new java.util.function.Consumer[A]() {
  override def accept(arg: A): Unit = function.apply(arg)
}


import org.ndx.model.Packet;
import org.ndx.model.PacketPayload;
import org.ndx.model.PacketModel.RawFrame;
import org.ndx.model.Statistics;
import org.ndx.tshark.HttpRequest;

val frames = sc.hadoopFile("hdfs://neshpc1.fit.vutbr.cz/user/rysavy/cap/*.cap", 
                            classOf[org.ndx.pcap.PcapInputFormat], 
                            classOf[org.apache.hadoop.io.LongWritable], 
                            classOf[org.apache.hadoop.io.ObjectWritable])
```

## Use Http parser to enrich Packet objects

```scala
def xf(x:PacketPayload) = x.getPacket().extendWith("http", HttpRequest.tryParseRequest(x.getPayload()))
val packets = frames.map(x=> Packet.parsePacket(x._2.get().asInstanceOf[RawFrame], toConsumer(xf))) 
```

```scala
val http = packets.filter(x=> x.containsKey("http.request"))
http.take(20).map(HttpRequest.format).foreach(println)
```
