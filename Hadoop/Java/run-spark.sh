ssh neshpc1.fit.vutbr.cz 'rm -R ~/ndx-spark-shell ; mkdir ~/ndx-spark-shell'
scp ndx-spark-shell/target/ndx-spark-shell.jar neshpc1.fit.vutbr.cz:~/ndx-spark-shell/
ssh neshpc1.fit.vutbr.cz 'spark-shell --jars ~/ndx-spark-shell/ndx-spark-shell.jar'
