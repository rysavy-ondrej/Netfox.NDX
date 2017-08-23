package org.ndx.model;
import com.google.protobuf.ByteString;
import org.ndx.model.PacketModel.RawFrame;

public class RawFrameHelper  {
    public static RawFrame New(int linkType, int frameNumber, int frameLength, long ticks, byte[] rawFrameData)
    {
        RawFrame.Builder rawFrameBuilder = RawFrame.newBuilder();
        rawFrameBuilder.setLinkTypeValue(linkType);
        rawFrameBuilder.setTimeStamp(ticks);
        rawFrameBuilder.setFrameLength(frameLength);
        rawFrameBuilder.setFrameNumber(frameNumber++);
        rawFrameBuilder.setData(ByteString.copyFrom(rawFrameData));
        return rawFrameBuilder.build();
    }
}