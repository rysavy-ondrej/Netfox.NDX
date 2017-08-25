# NDX-SPARK-CAPINFOS
First we precompute information about the pcap file:

```scala
val capinfo = packets.map(x => Statistics.fromPacket(x)).reduce(Statistics.merge)
```

##  Number of packets
```scala
capinfo.getPackets()
```

## Capture Duration
```scala
Statistics.ticksToSeconds((capinfo.getLastSeen() - capinfo.getFirstSeen()))
```

## First packet time:
```scala
Statistics.ticksToDate(capinfo.getFirstSeen())
```

## Last packet time:
```scala
Statistics.ticksToDate(capinfo.getLastSeen())
```

## Data byte rate:
```scala
capinfo.getOctets() / Statistics.ticksToSeconds((capinfo.getLastSeen() - capinfo.getFirstSeen()))
```

## Data bit rate:
```scala
(capinfo.getOctets() / Statistics.ticksToSeconds((capinfo.getLastSeen() - capinfo.getFirstSeen()))) * 8
```

## Average packet size:
```scala
capinfo.getAvgPacketSize()
```

## Average packet rate: 
```scala
capinfo.getPackets() / ( (capinfo.getLastSeen() - capinfo.getFirstSeen()) / 10000000 )
```