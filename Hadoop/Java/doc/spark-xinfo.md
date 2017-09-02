# NDX-SPARK-XINFO

This doucment shows how to use NDX to get HTTP request information for HTTP sessions.

We are able to parse Http requests uing HttpRequest class. 

We have implemented HTTP request analysis similarly to CloudShark.
https://support.cloudshark.org/user-guide/analyze-http-requests.html

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

## Get a sample of HTTP Requests

```scala
val http = packets.filter(x=> x.containsKey("http.request")).cache()
http.take(20).map(HttpRequest.format).foreach(println)
```

## Requests by Host
The Requests by Hosts view provides a list of all HTTP requests, sorted by host. The percentage of the total number of requests, per host, is displayed on the right side.

```scala
val hosts = http.map(x => (x.get("http.host"),1)).reduceByKey(_ + _)
hosts.takeOrdered(20)(Ordering[Integer].reverse.on(x=> x._2)).foreach(println)
```

## Requests by User-Agent
```scala
val ua = http.map(x => (x.get("http.user-agent"),1)).reduceByKey(_ + _)
ua.takeOrdered(20)(Ordering[Integer].reverse.on(x=> x._2)).foreach(println)
```

## Requests by Request Line
```scala
val rline = http.map(x => (x.get("http.request.line"),1)).reduceByKey(_ + _)
rline.takeOrdered(20)(Ordering[Integer].reverse.on(x=> x._2)).foreach(println)
```

## Requests by URL
```scala
val urls = http.map(x => (HttpRequest.getUrl("http", x),1)).reduceByKey(_ + _)
urls.takeOrdered(20)(Ordering[Integer].reverse.on(x=> x._2)).foreach(println)
```