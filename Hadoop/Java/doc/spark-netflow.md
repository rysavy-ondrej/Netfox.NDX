# Top 10 flows:
```scala
import org.ndx.model.Packet;
import org.ndx.model.PacketModel.RawFrame;
import org.ndx.model.Conversations;

val frames = sc.hadoopFile("hdfs://neshpc1.fit.vutbr.cz/user/rysavy/cap/*.cap", classOf[org.ndx.spark.pcap.PcapInputFormat], classOf[org.apache.hadoop.io.LongWritable], classOf[org.apache.hadoop.io.ObjectWritable]);
val packets = frames.map(x=> Packet.parsePacket(x._2.get().asInstanceOf[RawFrame])); 
val flows = packets.map(x=>(x.getFlowString(),x));
val stats = flows.map(x=>(x._1,Conversations.fromPacket(x._2))).reduceByKey(Conversations.merge);
println("Date flow start          Duration Proto   Src IP Addr:Port      Dst IP Addr:Port     Packets    Bytes Flows")
stats.takeOrdered(10)(Ordering[Int].reverse.on(x=> x._2.getPackets())).map(c=>Conversations.format(c._1, c._2)).foreach(println)
println("Done.")
```