# NDX.Hadoop
This solution consists of several Java modules for analysis of PCAP files in Apache SPARK. 

It is required to have working SPARK installation (either standalone or cluster).

Currently, the tool can be used via spark-shell. To run the tool type the following command in the root project folder (necessary to localize jar files):
```bash
spark-shell --jars ndx-spark-model/target/ndx-spark-model-0.9-SNAPSHOT.jar,ndx-spark-pcap/target/ndx-spark-pcap-0.9-SNAPSHOT.jar
```

Tutorials and documentation are available in doc folder.