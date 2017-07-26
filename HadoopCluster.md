# Mesos Cluster
The development Mesos cluster is equipped with:
* Mesos
* Hadoop/HDFS
* Spark 
* Cassandra

The following steps assumes using Linux based OS (tested with Ubuntu 16.04 LTS)
and it installs Hadoop, Spark and Cassandra. 

Because these packages will run several services, it is common to create a separate account.
Here, we create hadoop group and several users:
* hdfs - account to run hdfs service
* spark - account to run spark service
* cassandra - account to run Cassandra instances

All accounts are in hadoop group.
```bash
# Create new group hadoop
sudo addgroup hadoop
# Create new user hdfs and add her to group hadoop
sudo adduser hdfs hadoop
# Create new user spark and add her to group hadoop
sudo adduser spark hadoop
# Create new user cassandra and add her to group hadoop
sudo adduser cassandra hadoop
```

## Common Problems
Check that services are running on the correct interface! In some cases they are
listening on localhost only and cannot be thus accessed from network hosts. 

## JAVA
Hadoop and Spark requires JAVA 8. To install Oracle Java 8 the following 
steps should be performed:

```bash
sudo add-apt-repository ppa:webupd8team/java
sudo apt-get update
sudo apt-get install oracle-java8-installer
sudo apt-get install oracle-java8-set-default
echo "export JAVA_HOME=/usr/lib/jvm/java-8-oracle" >> ~/.bashrc
```



## Hadoop
Hadoop can be either installed from prebuilt package or from scratch. 
To download a binary version of hadoop use the following link:
http://www-eu.apache.org/dist/hadoop/common/hadoop-2.8.0/hadoop-2.8.0.tar.gz

```bash
# Get Hadoop binary package
wget http://www-eu.apache.org/dist/hadoop/common/hadoop-2.8.0/hadoop-2.8.0.tar.gz
# Unpack the package to /usr/local
sudo tar -xzf hadoop-2.8.0.tar.gz -C /usr/local
# Create a symlink
sudo ln -s /usr/local/hadoop-2.8.0.tar.gz /usr/local/hadoop
```

Hadoop requires to set system variable ```HADOOP_PREFIX``` to the location where hadoop is isntalled.
Append to user's .bashrc file:

```bash
echo "export HADOOP_PREFIX=/usr/local/hadoop" >> ~/.bashrc
```

Hadoop considers that there is a single name server (hostname is hdfsmaster). 

Hadoop configuration is required in order to provide correct 
values for the name node and data nodes. It consists of several files located in 
$HADOOP_PREFIX/etc

#### hadoop-env.sh
This file sets up the environment for hadoop. The only required content is 
the specification of Java home. Find where Java is installe don your machine. Usually, 
it is under ```/usr/lib/jvm/```.
```bash
export JAVA_HOME=/usr/lib/jvm/java-8-openjdk-amd64
```
#### core-site.xml
```xml
<?xml version="1.0"?>
<?xml-stylesheet type="text/xsl" href="configuration.xsl"?>

<!-- Put site-specific property overrides in this file. -->

<configuration>
<property>
  <name>hadoop.tmp.dir</name>
  <value>/app/hadoop/tmp</value>
  <description>A base for other temporary directories.</description>
</property>

<property>
  <name>fs.default.name</name>
  <value>hdfs://hdfsmaster:9000</value>
  <description>The name of the default file system.  A URI whose
  scheme and authority determine the FileSystem implementation.  The
  uri's scheme determines the config property (fs.SCHEME.impl) naming
  the FileSystem implementation class.  The uri's authority is used to
  determine the host, port, etc. for a filesystem.</description>
</property>
</configuration>
```

#### hdfs-site.xml
```xml
<?xml version="1.0"?>
<?xml-stylesheet type="text/xsl" href="configuration.xsl"?>

<!-- Put site-specific property overrides in this file. -->

<configuration>
<property>
  <name>dfs.replication</name>
  <value>1</value>
  <description>Default block replication.
  The actual number of replications can be specified when the file is created.
  The default is used if replication is not specified in create time.
  </description>
</property>

<property>
  <name>idfs.namenode.rpc-address</name>
  <value>192.168.111.136</value>
</property>

</configuration>
```

After all is configured, executing NameNode and DataNode on the local machine is done by the following script:
```bash
$HADOOP_HOME/sbin/start-dfs.sh
```

It should be now possible to work with HDFS is a usual way:
```bash
/usr/local/hadoop/bin/hadoop fs -ls hdfs://hdfsmaster:9000/
```
By default, Hadoop uses ```whoami``` to get user name for operations. If other 
user name needs to be used, it is possible in the following way:
```bash
HADOOP_USER_NAME=hduser bin/hadoop fs -put README.txt hdfs://192.168.111.136:9000/data
```

## Spark
Installation of Spark is straightforward:
```bash
sudo apt-get install scala
su - spark
wget https://d3kbcqa49mib13.cloudfront.net/spark-2.2.0-bin-hadoop2.7.tgz
tar -xzf spark-2.2.0-bin-hadoop2.7.tgz
sudo mv spark-2.2.0-bin-hadoop2.7 /usr/local
sudo ln -s /usr/local/spark-2.2.0-bin-hadoop2.7 /usr/local/spark
/usr/local/spark/sbin/start-master.sh
/usr/local/spark/sbin/start-slave.sh spark://sparkmaster:7077
```
To access master node from other than local machine it is necessary to check 
that host name of the master node is set properly in /etc/hosts:
```
127.0.0.1       localhost
192.168.111.136 sparkmaster.domain.org sparkmaster
```
To check that Spark is running it is possible to see UI console at http://sparkmaster:8080.
To test Spark cluster, spark-shell can be executed:
```
./bin/spark-shell --master spark://sparkmaster:7077
```
When connecting to remote Spark Cluster (from client outside this cluster) it may be necessary
to specify SPARK_LOCAL_IP in $SPARK_HOME/conf/spark-env.sh. For instance, 
if Spark cluster runs in remote private network and client connect through VPN to this network 
then the client needs to specify IP address assigned on VPN interface as SPARK_LOCAL_IP.

To test that Spark and Hadoop works together the following can be exercised:
```
$ $HADOOP_HOME/bin/hadoop fs -mkdir /data
$ $HADOOP_HOME/bin/hadoop fs -put somefile.txt /data/somefile.txt
$ $HADOOP_HOME/bin/spark-shell --master spark://sparkmaster.domain.org:7077
...
scala> val textFile = spark.read.textFile("hdfs://hdfsmaster.domain.org:9000/data/somefile.txt")
scala> textFile.count()
```
If no error is generated then Spark is able to read data from HDFS and performs 
simple computation.

## Checklist
This checklist is a for demonstration installation on mesos node (192.168.111.136):
* OS installation - Done
* HDFS installation - Done
* Mesos installation - Done
* Spark installation - Done
* Test the installation by runing simple Spark demo  - Done
* Cassandra installation
* Test HDFS-SPark-Cassandra by running C# demo application

