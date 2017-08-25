# Data Preparation
## Spliting large PCAP files
Spark works best if the input data source is partitioned.  
This means that computing nodes can process data in parallel. If we have data source in the form of 
number of files each of reasonable size we do not have to do anything with it. 

Because PCAP files must be read sequentionally, Spark is unable to create partitions for a single input file automatically. 
Instead this file is read by as a single collection. 

Thus, if our input contains a few big (>1GB) files, it is usually better to split them before loading them in HDFS data storage. 
By splitting the file into smaller pieces, Spark is enabled to paritition the input and execute most of the operations in parallel.

Tools such as ```editcap``` from Wireshark distribution can handle this operation efficiently. The following command splits the source file into multiple out files:

```bash
editcap -c 500000 -F pcap source.cap target.cap
```
where:
* ```-c 5000000``` parameter specifies that each pcap file will contain at most 500,000 packets,
* ```-F pcap``` sets the out format to tcpdump packet capture files,
* ```source.cap``` is source pcap file, and 
* ```target.cap``` is used to name resulting pcap files. The naming convention is ```target_NNNNN.cap```.

Determinig the optimal number of packets per file may be tricky. There are several factors that affects this decision. For HDFS it is better if the size of the file is close to 
block size (default is 128MB). For Spark cluster this depends on the number of workers and available memory. 

For more information on ```editcap``` see https://www.wireshark.org/docs/man-pages/editcap.html.

## Uploading files to HDFS
After the data is prepared, it can be uploaded to HDFS. 
```
export HADOOP_USER_NAME=rysavy
hdfs dfs -put target_*.cap  hdfs://neshpc1.fit.vutbr.cz/user/rysavy/cap/
```
It takes some time depending on the total size of the collection.

# Spark Shell
It is possible to use ```spark-shell``` for running capture analysis. Bellow are some snippets
that demonstrates the common tasks.

Execute spark-shell from the project's home directory:
```bash
spark-shell --jars ndx-spark-model/target/ndx-spark-model-0.9-SNAPSHOT.jar,ndx-spark-pcap/target/ndx-spark-pcap-0.9-SNAPSHOT.jar
```


## Test environment
Because Spark depends on ```protobuf-java-2.5.0.jar``` but we use newer version ```protobuf-java-3.3.1.jar``` it is necessary to make sure that 
these two are not in conflict. To check that the Spark environment is working correctly, try the following snippet. 
```
import org.ndx.model.Conversations
val c = Conversations.create(0,0,0,0)
```

## Load files from HDFS:
When we have capture files in HDFS, it is possible to load captured packets into the source RDD:
```scala
val frames = sc.hadoopFile("hdfs://neshpc1.fit.vutbr.cz/user/rysavy/cap/*.cap", classOf[org.ndx.spark.pcap.PcapInputFormat], classOf[org.apache.hadoop.io.LongWritable], classOf[org.apache.hadoop.io.ObjectWritable]);
```

To compute a number of all packets in the data source, execute the following expression:
```spark
frames.count()
```

Compare the time necessary for this operation with Wireshark implementation:
```bash
capinfos -c sourcecap
```
for files of several GB this takes several minutes to complete, while the Spark version requires about 10s.
## Parse Frames 
Loader uses PCAP reader to provide raw frames. In order to get packets from the raw frames, we apply ```ParsePacket``` function of ```Packet``` class:

```scala
import org.ndx.model.Packet;
import org.ndx.model.PacketModel.RawFrame;
val packets = frames.map(x=> Packet.parsePacket(x._2.get().asInstanceOf[RawFrame])); 
```


## Track Flows
To get unidirectional flows, ```Packet``` class implements method ```getFlowString``` that computes flow key for each packet:

```scala
val flows = packets.map(x=>(x.getFlowString(),x));
```

## Track Conversations
To get bidirectional flows (conversations), ```Packet``` class implements method ```getSessionString``` that computes session key for each packet:

```scala
val flows = packets.map(x=>(x.getSessionString(),x));
```

## Compute some basic statistics
The following snippet shows how to compute for each flow the number of its packet:
```scala
val flowCounts = flows.map(x=>(x._1,1)).reduceByKey(_ + _).collect();
```
To compute rich statistics is is better to create a new object that would aggregate all statistics. 
In our case, we can use ```org.ndx.model.ConversationModel.FlowAttributes```.
Then it is possible to compute flow statistics in the similar way as in the preceding example:
```scala
import org.ndx.model.Conversations;
val stats = flows.map(x=>(x._1,Conversations.fromPacket(x._2))).reduceByKey(Conversations.merge);
```

Print the flows in usual format using ```ConversationFormatter```:
```
println("Date flow start          Duration Proto   Src IP Addr:Port      Dst IP Addr:Port     Packets    Bytes Flows") ;
stats.takeOrdered(10)(Ordering[Int].reverse.on(x=> x._2.getPackets())).map(c=>Conversations.format(c._1, c._2)).foreach(println)

 Date flow start          Duration Proto   Src IP Addr:Port      Dst IP Addr:Port     Packets    Bytes Flows
 2010-09-01 00:00:00.459     0.000 UDP     127.0.0.1:24920   ->  192.168.0.1:22126        1       46     1
 2010-09-01 00:00:00.363     0.000 UDP     192.168.0.1:22126 ->  127.0.0.1:24920          1       80     1
```

The following snippet shows how to get the top 10 flows having the most packets and octets, respectively:
```scala
val top10flowsPackets = flows.map(x=>(x._1,Conversations.fromPacket(x._2))).reduceByKey(Conversations.merge).takeOrdered(10)(Ordering[Int].reverse.on(x=> x._2.getPackets()));
val top10flowsOctets = flows.map(x=>(x._1,Conversations.fromPacket(x._2))).reduceByKey(Conversations.merge).takeOrdered(10)(Ordering[Long].reverse.on(x=> x._2.getOctets()));
```

