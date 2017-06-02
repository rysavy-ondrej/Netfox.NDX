This document provides a manual for installation and configuration SparkClr.
BEware to obtain correct version of Spark and SparkClr as runnig incompatbile versions results
in exception in Spark commands.
See https://github.com/Microsoft/Mobius/blob/master/notes/mobius-release-info.md#versioning-policy for more details.
SparkClr contains two version numbers: SCALA VERSION followed by SPARK VERSION. Corresponding Spark version needs to be used.

# Spark Installation
The following describes steps needed to run Spark on Windows Machine. This was tested on Windows 10.

* Download and install JDK (http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html)

* Download Spark (http://spark.apache.org/downloads.html), upack and copy to a target folder, e.g., (C:\Apache\Spark)  

* Download winutils for Hadoop (https://github.com/steveloughran/winutils), because Spark will complain that winutils.exe is missing. Note that version
of Hadoop of Winutils should match to the version of Hadoop of Spark. Copy the content of the winutils directory to the folder (C:\Apache\Hadoop\bin).


* We assume that all will be installed in C:\Apache folder. Thus, set the following environment variables:
```
set JAVA_HOME="C:\Progra~1\Java\jre1.8.0_131"
set HADOOP_HOME=C:\Apache\Hadoop
set SPARK_HOME=C:\Apache\Spark
set PATH=%HADOOP_HOME%\bin;%SPARK_HOME%\bin;%PATH%
```
Note that paths should not contain space. Use 8.3 notation instead. Adjust JAVA_HOME variable according to your Java installation.

* Check the existence of C:\tmp\hive directory and set the following permission:
```
winutils.exe chmod -R 777 C:\tmp\hive
```

* Execute Spark Shell to check that it is working:
```
spark-shell.cmd
```
There should not be any java exceptions. Then execute the following Spark command 
to test the Spark:
```
spark.range(1).withColumn("status", lit("All seems fine. Congratulations!")).show(false)
```
Another test is to load a local test file and print all its lines:
```
var f = textFile("file:///C:/Users/John/Documents/readme.md")
f.foreach(println)
``` 
To check that this works open web page http://localhost:4040/jobs/ that provides Spark Shell UI.

# Spark Clr 
To write and execute jobs in C# additional configuration steps are necessary:
* Obtain SparkClr 
* Set SPARKCLR_HOME variable to point to SparkClr home directory.
```
set SPARKCLR_HOME=C:\Apache\SparkClr
```
* To execute a job written in C# use sparkclr-submit.cmd:
```
sparkclr-submit --exe <DRIVER-PROGRAM> <WORKER-DIRECTORY> <ARGUMENTS>
```
Where:
<DRIVER-PROGRAM> is the executable file in WORKER-DIRECTORY to be executed
<WORKER-DIRECTORY> is a folder that contains binary files of the worker 
<ARGUMENTS> are arguments to be send to the program

Example:
```
sparkclr-submit --exe WordCount C:\Users\John\WCProject\bin\Debug file:///C:/Users/John/Documents/bigfile.txt
```