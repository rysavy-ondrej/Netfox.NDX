package org.ndx.model;

import org.ndx.model.ConversationModel.FlowAttributes;
import org.ndx.model.FlowModel.FlowKey;
import java.util.Date;
public class Statistics {
    static long UnixBaseTicks = 621355968000000000L; 
    static long TickPerMicroseconds = 10L; 
    static long TickPerMilliseconds = 10000L;
    static long TickPerSeconds = 10000000L;
    public static Date ticksToDate(long ticks)
    {
            long us = (ticks-UnixBaseTicks) / TickPerMicroseconds ;
            return new Date(us/1000);
    }

    public static float ticksToSeconds(long ticks)
    {
        return (((float)ticks) / (float)TickPerSeconds);
    }

 
    public static FlowAttributes merge (FlowAttributes x, FlowAttributes y) 
    {
        FlowAttributes.Builder builder  = FlowAttributes.newBuilder();
        builder.setFirstSeen(Math.min(x.getFirstSeen(),y.getFirstSeen()));
        builder.setLastSeen(Math.max(x.getLastSeen(),y.getLastSeen()));
        builder.setPackets(x.getPackets()+y.getPackets());
        builder.setOctets(x.getOctets() + y.getOctets());
        builder.setMaximumPayloadSize(Math.max(x.getMaximumPayloadSize(),y.getMaximumPayloadSize()));
        builder.setMinimumPayloadSize(Math.min(x.getMinimumPayloadSize(),y.getMinimumPayloadSize()));
        builder.setMeanPayloadSize((x.getMeanPayloadSize() + y.getMeanPayloadSize()) / 2); 
        return builder.build();
    }

    public static FlowAttributes fromPacket(Packet p)
    {
        Long first = ((Number)p.get(Packet.TIMESTAMP)).longValue();
        Long last = ((Number)p.get(Packet.TIMESTAMP)).longValue();
        Long octets = ((Number)p.get(Packet.LEN)).longValue();
      
        FlowAttributes.Builder builder  = FlowAttributes.newBuilder();
        builder.setFirstSeen(first);
        builder.setLastSeen(last);
        builder.setPackets(1);
        builder.setOctets(octets);
        builder.setMaximumPayloadSize(octets.intValue());
        builder.setMinimumPayloadSize(octets.intValue());
        builder.setMeanPayloadSize(octets.intValue());
        return builder.build();
    }

    static java.text.DecimalFormat df = new java.text.DecimalFormat("#.###");
    // Formats 
    public static String format(String flowkey, FlowAttributes attributes)
    {
        Date first = ticksToDate(attributes.getFirstSeen());
     
        Date last = ticksToDate(attributes.getLastSeen());
        float diff = ((float)(last.getTime() - first.getTime()))/1000;
        FlowKey fkey = Packet.flowKeyParse(flowkey);
        String fkeystr = String.format("%5s %20s -> %20s ", 
            fkey.getProtocol().toStringUtf8(), 
            fkey.getSourceAddress().toStringUtf8() + ":" + fkey.getSourceSelector().toStringUtf8(), 
            fkey.getDestinationAddress().toStringUtf8() + ":" + fkey.getDestinationSelector().toStringUtf8());
        return String.format("%30s %12s %60s %10d %15d %5d",first.toString(), df.format(diff), fkeystr, attributes.getPackets(), attributes.getOctets(), 1 );
    }
}
