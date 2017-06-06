# NDX runnning in Spark
This document explains how NDX is executed in Apache Spark environment.

## NDX Ingest
Ingest data is transferred into the Hadoop cluster, where they are transformed and loaded into data stores.
Ingest is represented as a SPARK job depending on the data source.

## NDX Processor
NDX Processor is a collection of modules that perform automatic extration, indexing, filtering enrichment, and transformation of ingested data.
For each type of data at one or more modules can be registered for processing.
Currently supported data types are Flow and Packets. Soon we consider to include also Logs and other data sources.
For enrichment open data sources are used.  
Processing modules provide result that are stored in the solution data store for further processing. 

## NDX Analytics
NDX Analytics a collection of analytical modules. These modules include machine learning 
for performing intelligent analysis.

## NDX UI
NDX User Interface constains a dashboard and spezialzed views for 
results of analytics. Also UI supports manual analysis. The UI is written 
in Node.js employing Angular framework. The programming language is Typescript.

# Implementation Notes
* For processing Spark is used.
* For data store the available options are HDFS, Hive and Cassandra
* For communication Kafka is used.
Though native language for the technology used is Java, some parts of the system is developed in Python and C#.	User Interface 
is developed using Node.js and Angular framework.


# Installation
The following services are required to run:
* HDFS - distributed data storage, the core service for other components 
* HIVE - distributed data storage that offers SQL access.
* KAFKA - distributed high performance messaging platform.
* SPARK -  is a fast and general engine for big data processing, with built-in modules for streaming, SQL, machine learning and graph processing.
* YARN -  allows different data processing engines to run in a single platform.
* Zookeeper - is a centralized service for maintaining configuration information, naming, providing distributed synchronization, and providing group services.
* Cassandra DB - is scalable database ...

Different roles are defined and required by these services:
* NN - Name Node
* DN - Data Node
* JN - Journal Node
* RM - Resource Manager
* JHS - Job History Server
* SHS - Spark Histroy Server 

The experimental cluster consists of several nodes with different roles:
* Master Server - runs Name Node, Resource Manager, Zookeeper, HBase Master
* Worker Server - runs Data Node, Node Manager, HBase Record Server
* Edge Server - runs Gateway and Ingest