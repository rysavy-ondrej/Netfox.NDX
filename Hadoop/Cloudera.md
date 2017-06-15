# Cloudera Installation
This doucment describes the steps necessary to install cloudera manager in Ubuntu 16 Xenial.

1. Install key necessary for accepting packege from Cloudera: 
```
wget http://archive.cloudera.com/cm5/ubuntu/xenial/amd64/cm/archive.key
sudo apt-key add archive.key
```

2. Add Cloudera repository:
```
wget https://archive.cloudera.com/cm5/ubuntu/xenial/amd64/cm/cloudera.list
sudo apt-get update
```
3. Try to install Oracle JDK package:
```
sudo apt-get install oracle-j2sdk1.7
```
4. Download and execute Cloudera Manager Installer:
```
wget https://archive.cloudera.com/cm5/installer/latest/cloudera-manager-installer.bin
chmod u+x cloudera-manager-installer.bin
sudo ./cloudera-manager-installer.bin
```
5. When installation finishes, open browser to connect to Cloudera Manager:
```
http://cloudera-manager-server:7180
```