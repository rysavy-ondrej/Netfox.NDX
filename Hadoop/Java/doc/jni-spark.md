# Java JNI in SPARK

First of all, the native library must be preinstalled on all worker nodes. Path to that library must be specified in spark-env.sh:

```bash
export SPARK_LIBRARY_PATH=/path/to/native/library
```

Use ```SPARK_PRINT_LAUNCH_COMMAND``` environment variable to diagnose it:

```bash
export SPARK_PRINT_LAUNCH_COMMAND=1
```

If everything's set correctly, you will see output like this:

```bash
/path/to/java -cp <long list of jars> -Djava.library.path=/path/to/native/library <etc>
```


# Maven JNI/Native

http://www.tricoder.net/blog/?p=197
