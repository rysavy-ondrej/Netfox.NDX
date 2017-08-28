package org.ndx.model;

import org.ndx.model.Packet;

public class PacketPayload {
    Packet _packet;
    byte[] _payload;
    public PacketPayload(Packet packet, byte[] payload)
    {
        _packet = packet;
        _payload = payload;
    }

    public Packet getPacket()
    {
        return _packet;
    }
    public byte[] getPayload()
    {
        return _payload;
    }
}