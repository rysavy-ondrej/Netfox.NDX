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
