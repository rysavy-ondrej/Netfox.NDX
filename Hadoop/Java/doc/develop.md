# Development Instructions

## Scala Maven
The easiest way to create new projects is using an “archetype”. An archetype is a general skeleton structure, or template for a project.
Use Maven to generate the new project and to adjust solution POM file accordingly. In solution folder type:

```mvn archetype:generate```

Then select Spark project type, e.g.:
```net.alchim31.maven:scala-archetype-simple```

To propely setup Spark project in Scala some additional configuration is necessary in ```pom.xml``` file:

* Add the following content to the ```dependencies``` tree:
```xml
    <!-- Fix issue with missing JUnitRunner class -->
    <dependency>
        <groupId>org.specs2</groupId>
        <artifactId>specs2_2.11</artifactId>
        <version>3.7</version>
        <type>pom</type>
        <scope>test</scope>
    </dependency>
    <!-- Apache Spark dependencies (2.11) -->
    <dependency>
        <groupId>org.apache.spark</groupId>
        <artifactId>spark-core_2.11</artifactId>
        <version>2.2.0</version>
        <scope>provided</scope>
    </dependency>
    <dependency>
        <groupId>org.apache.spark</groupId>
        <artifactId>spark-sql_2.11</artifactId>
        <version>2.2.0</version>
        <scope>provided</scope>
    </dependency>
    <dependency>
        <groupId>org.apache.spark</groupId>
        <artifactId>spark-streaming_2.11</artifactId>
        <version>2.2.0</version>
        <scope>provided</scope>
    </dependency>
    <dependency>
        <groupId>org.apache.spark</groupId>
        <artifactId>spark-mllib_2.11</artifactId>
        <version>2.2.0</version>
        <scope>provided</scope>
    </dependency>
    <dependency>
        <groupId>org.apache.spark</groupId>
        <artifactId>spark-graphx_2.11</artifactId>
        <version>2.2.0</version>
        <scope>provided</scope>
    </dependency>
```
* Check build section:
```xml
<build>
    <sourceDirectory>src/main/scala</sourceDirectory>
    <testSourceDirectory>src/test/scala</testSourceDirectory>
    <plugins>
        <plugin>
            <groupId>net.alchim31.maven</groupId>
            <artifactId>scala-maven-plugin</artifactId>
            <version>3.2.2</version>
            <executions>
            <execution>
                <goals>
                <goal>compile</goal>
                <goal>testCompile</goal>
                </goals>
            </execution>
            </executions>
        </plugin>
        <plugin>			
            <groupId>org.apache.maven.plugins</groupId>
            <artifactId>maven-jar-plugin</artifactId>
            <version>2.4</version>
            <configuration>
                <archive>
                    <manifest>
                        <mainClass>org.ndx.spark.ntrace.App</mainClass>
                    </manifest>
                </archive>
            </configuration>
        </plugin>
    </plugins>
</build>
```



## Submit Application
Considerng that we have sucessfuly compiled Spark application, it is possible to execute it in Spark environment.
More information about this is at https://spark.apache.org/docs/latest/submitting-applications.html

