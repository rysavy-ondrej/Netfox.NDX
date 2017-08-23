#!/bin/bash
export HADOOP_USER_NAME=spark
hdfs dfs -put ndx-spark-model/target/ndx-spark-model-0.9-SNAPSHOT.jar hdfs://neshpc1.fit.vutbr.cz:8020/user/spark/jars
hdfs dfs -put ndx-spark-ntrace/target/ndx-spark-ntrace-0.9-SNAPSHOT.jar hdfs://neshpc1.fit.vutbr.cz:8020/user/spark/jars
hdfs dfs -put ndx-spark-pcap/target/ndx-spark-pcap-0.9-SNAPSHOT.jar hdfs://neshpc1.fit.vutbr.cz:8020/user/spark/jars
