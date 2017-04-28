# Running Spark with Cassandra in Docker
This document describes how to setup environment with Spark and Cassandra 
nodes deployed all in docker and implement a simple Spark process in C#. 
This document extracts is based on information from 
https://hub.docker.com/r/clakech/sparkassandra-dockerized/
and 
https://academy.datastax.com/resources/getting-started-apache-cassandra-and-c-net.



## Cassandra and Spark
It is possible to run Cassandra and Spark nodes following the several simple steps
outlined bellow. The image is already prepared so it is not necessary to install anything.

1. To run the Spark master node execute the following command:
```bash
$ docker run -d -t -P --name SPARK-MASTER clakech/sparkassandra-dockerized /start-master.sh
```

2. Run the main Cassandra node: 
```bash
$ docker run -it --name CASSANDRA-HEAD --link SPARK-MASTER:spark_master -d clakech/sparkassandra-dockerized
```
3. Run several other Cassandra/Spark nodes.
```bash
$ docker run -it --name CASSANDRA-NODE --link SPARK-MASTER:spark_master --link CASSANDRA-HEAD:cassandra -d clakech/sparkassandra-dockerized
```

## Implementing C# Spark Worker
C# worker can be implemented using Spark Driver 
