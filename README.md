# NDX
The network diagnostic framework (NDX) implements environment for analysis of pcap files,
netflow records and log files in order to provide diagnostic information on network device
and network services behavior.


## Architecture

The NDX architecture represents a processing pipeline that comprises of several stages:
* Data Source - provides access to different data sources. 
* Ingest - prepares and process source data. The data are then ready for analysis. 
* Analyze - applies different analytical methods to acquire information value from the data.
* Publication - transforms data to make is suitable for presentation or further processing by external system.
* Data Consumption - sends data to external system.