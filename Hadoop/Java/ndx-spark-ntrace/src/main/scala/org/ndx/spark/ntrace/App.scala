package org.ndx.spark.ntrace
import org.apache.spark.sql.SparkSession
/**
 * @author ${user.name}
 */
object App {
  
  def main(args: Array[String]) {

    if (args.length < 2) {
      System.err.println("Usage: ndx-ntrace <file>")
      System.exit(1)
    }

    val inputPath = args(0)

    val spark = SparkSession
      .builder
      .appName("ndx-ntrace")
      .getOrCreate()
    val ctx = spark.sparkContext
    System.err.println(spark.version)
    spark.stop()
  }
}