package org.ndx.pcap;

import java.io.DataInputStream;
import java.io.EOFException;
import java.io.IOException;
import java.util.Iterator;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.google.common.base.Objects;
import com.google.common.collect.ComparisonChain;

import org.ndx.model.RawFrameHelper;
import org.ndx.model.PacketModel.RawFrame;

public class PcapReader implements Iterable<RawFrame> {
	public static final Log LOG = LogFactory.getLog(PcapReader.class);
	public static final long MAGIC_NUMBER = 0xA1B2C3D4;
	public static final int HEADER_SIZE = 24;
	public static final int PCAP_HEADER_SNAPLEN_OFFSET = 16;
	public static final int PCAP_HEADER_LINKTYPE_OFFSET = 20;
	public static final int PACKET_HEADER_SIZE = 16;
	public static final int TIMESTAMP_OFFSET = 0;
	public static final int TIMESTAMP_MICROS_OFFSET = 4;
	public static final int CAP_LEN_OFFSET = 8;
	public static final int ETHERNET_HEADER_SIZE = 14;
	public static final int ETHERNET_TYPE_OFFSET = 12;
	public static final int ETHERNET_TYPE_IP = 0x800;
	public static final int ETHERNET_TYPE_IPV6 = 0x86dd;
	public static final int ETHERNET_TYPE_8021Q = 0x8100;
	public static final int SLL_HEADER_BASE_SIZE = 10; // SLL stands for Linux cooked-mode capture
	public static final int SLL_ADDRESS_LENGTH_OFFSET = 4; // relative to SLL header
	public static final int IPV6_HEADER_SIZE = 40;
	public static final int IP_VHL_OFFSET = 0;	// relative to start of IP header
	public static final int IP_TTL_OFFSET = 8;	// relative to start of IP header	
	public static final int IP_TOTAL_LEN_OFFSET = 2;	// relative to start of IP header
	public static final int IPV6_PAYLOAD_LEN_OFFSET = 4; // relative to start of IP header
	public static final int IPV6_HOPLIMIT_OFFSET = 7; // relative to start of IP header
	public static final int IP_PROTOCOL_OFFSET = 9;	// relative to start of IP header
	public static final int IPV6_NEXTHEADER_OFFSET = 6; // relative to start of IP header
	public static final int IP_SRC_OFFSET = 12;	// relative to start of IP header
	public static final int IPV6_SRC_OFFSET = 8; // relative to start of IP header
	public static final int IP_DST_OFFSET = 16;	// relative to start of IP header
	public static final int IPV6_DST_OFFSET = 24; // relative to start of IP header
	public static final int IP_ID_OFFSET = 4;	// relative to start of IP header
	public static final int IPV6_ID_OFFSET = 4;	// relative to start of IP header
	public static final int IP_FLAGS_OFFSET = 6;	// relative to start of IP header
	public static final int IPV6_FLAGS_OFFSET = 3;	// relative to start of IP header
	public static final int IP_FRAGMENT_OFFSET = 6;	// relative to start of IP header
	public static final int IPV6_FRAGMENT_OFFSET = 2;	// relative to start of IP header
	public static final int UDP_HEADER_SIZE = 8;
	public static final int PROTOCOL_HEADER_SRC_PORT_OFFSET = 0;
	public static final int PROTOCOL_HEADER_DST_PORT_OFFSET = 2;
	public static final int PROTOCOL_HEADER_TCP_SEQ_OFFSET = 4;
	public static final int PROTOCOL_HEADER_TCP_ACK_OFFSET = 8;
	public static final int TCP_HEADER_DATA_OFFSET = 12;
	public static final String PROTOCOL_ICMP = "ICMP";
	public static final String PROTOCOL_TCP = "TCP";
	public static final String PROTOCOL_UDP = "UDP";
	public static final String PROTOCOL_FRAGMENT = "Fragment";

	private final DataInputStream is;
	private Iterator<RawFrame> iterator;
	private LinkType linkType;
	private long snapLen;
	private boolean caughtEOF = false;
	private int frameNumber = 1;	
	//To read reversed-endian PCAPs; the header is the only part that switches
	private boolean reverseHeaderByteOrder = false;
	

	public byte[] pcapHeader;
	public byte[] pcapRawFrameHeader;
	public byte[] rawFrameData;

	public PcapReader(DataInputStream is) throws IOException {
		this.is = is;
		iterator = new RawFrameIterator();

		pcapHeader = new byte[HEADER_SIZE];
		if (!readBytes(pcapHeader)) {
			//
			// This special check for EOF is because we don't want
			// PcapReader to barf on an empty file.  This is the only
			// place we check caughtEOF.
			//
			if (caughtEOF) {
				LOG.warn("Skipping empty file");
				return;
			}
			throw new IOException("Couldn't read PCAP header");
		}

		if (!validateMagicNumber(pcapHeader))
			throw new IOException("Not a PCAP file (Couldn't find magic number)");

		snapLen = PcapReaderUtil.convertInt(pcapHeader, PCAP_HEADER_SNAPLEN_OFFSET, reverseHeaderByteOrder);

		long linkTypeVal = PcapReaderUtil.convertInt(pcapHeader, PCAP_HEADER_LINKTYPE_OFFSET, reverseHeaderByteOrder);
		if ((linkType = getLinkType(linkTypeVal)) == null)
			throw new IOException("Unsupported link type: " + linkTypeVal);
	}

	// Only use this constructor for testcases
	protected PcapReader(LinkType lt) {
		this.is = null;
		linkType = lt;
	}
	static long UnixBaseTicks = 621355968000000000L; 
	static long TicksPerSecond = 10000000L;
	static long TickPerMicroseconds = 10L; 
	private RawFrame nextRawFrame() {
		pcapRawFrameHeader = new byte[PACKET_HEADER_SIZE];
		if (!readBytes(pcapRawFrameHeader))
			return null;

		long ts = PcapReaderUtil.convertInt(pcapRawFrameHeader, TIMESTAMP_OFFSET, reverseHeaderByteOrder);
		long tsMicroseconds = PcapReaderUtil.convertInt(pcapRawFrameHeader, TIMESTAMP_MICROS_OFFSET, reverseHeaderByteOrder);
		long ticks = UnixBaseTicks + (ts * TicksPerSecond) + (tsMicroseconds * TickPerMicroseconds);
		

		int frameLength = (int)PcapReaderUtil.convertInt(pcapRawFrameHeader, CAP_LEN_OFFSET, reverseHeaderByteOrder);
		rawFrameData = new byte[(int)frameLength];

		if (readBytes(rawFrameData))
		{
			return RawFrameHelper.New(linkType.ordinal(), frameNumber++, frameLength, ticks,  rawFrameData);
		}
		else
		{
			return null;
		}
	}

	protected void processRawFramePayload(RawFrame frame, byte[] payload) {}

	protected boolean validateMagicNumber(byte[] pcapHeader) {
		if (PcapReaderUtil.convertInt(pcapHeader) == MAGIC_NUMBER) {
			return true;
		} else if (PcapReaderUtil.convertInt(pcapHeader, true) == MAGIC_NUMBER) {
			reverseHeaderByteOrder = true;
			return true;
		} else {
			return false;
		}
	}

	protected enum LinkType {
		NULL, EN10MB, RAW, LOOP, LINUX_SLL
	}

	protected LinkType getLinkType(long linkTypeVal) {
		switch ((int)linkTypeVal) {
			case 0:
				return LinkType.NULL;
			case 1:
				return LinkType.EN10MB;
			case 101:
				return LinkType.RAW;
			case 108:
				return LinkType.LOOP;
			case 113: 
				return LinkType.LINUX_SLL;
		}
		return null;
	}

	

	/**
	 * Reads the RawFrame payload and returns it as byte[].
	 * If the payload could not be read an empty byte[] is returned.
	 * @param rawFrameData
	 * @param payloadDataStart
	 * @return payload as byte[]
	 */
	protected byte[] readPayload(byte[] rawFrameData, int payloadDataStart, int payloadLength) {
		if (payloadLength < 0) {
			LOG.warn("Malformed RawFrame - negative payload length. Returning empty payload.");
			return new byte[0];
		}
		if (payloadDataStart > rawFrameData.length) {
			LOG.warn("Payload start (" + payloadDataStart + ") is larger than RawFrame data (" + rawFrameData.length + "). Returning empty payload.");
			return new byte[0];
		}
		if (payloadDataStart + payloadLength > rawFrameData.length) {
			if (payloadDataStart + payloadLength <= snapLen) // Only corrupted if it was not because of a reduced snap length
				LOG.warn("Payload length field value (" + payloadLength + ") is larger than available RawFrame data (" 
						+ (rawFrameData.length - payloadDataStart) 
						+ "). RawFrame may be corrupted. Returning only available data.");
			payloadLength = rawFrameData.length - payloadDataStart;
		}
		byte[] data = new byte[payloadLength];
		System.arraycopy(rawFrameData, payloadDataStart, data, 0, payloadLength);
		return data;
	}

	protected boolean readBytes(byte[] buf) {
		try {
			is.readFully(buf);
			return true;
		} catch (EOFException e) {
			// Reached the end of the stream
			caughtEOF = true;
			return false;
		} catch (IOException e) {
			e.printStackTrace();
			return false;
		}
	}

	@Override
	public Iterator<RawFrame> iterator() {
		return iterator;
	}

	private class RawFrameIterator implements Iterator<RawFrame> {
		private RawFrame next;

		private void fetchNext() {
			if (next == null)
				next = nextRawFrame();
		}

		@Override
		public boolean hasNext() {
			fetchNext();
			if (next != null)
				return true;			
			return false;
		}

		@Override
		public RawFrame next() {
			fetchNext();
			try {
				return next;
			} finally {
				next = null;
			}
		}

		@Override
		public void remove() {
			// Not supported
		}
	}

	private class SequencePayload implements Comparable<SequencePayload> {
		private Long seq;
		private byte[] payload;

		public SequencePayload(Long seq, byte[] payload) {
			this.seq = seq;
			this.payload = payload;
		}

		@Override
		public int compareTo(SequencePayload o) {
			return ComparisonChain.start().compare(seq, o.seq)
			                              .compare(payload.length, o.payload.length)
			                              .result();
		}

		public boolean linked(SequencePayload o) {
			if ((seq + payload.length) == o.seq)
				return true;
			if ((o.seq + o.payload.length) == seq)
				return true;
			return false;
		}

		@Override
		public String toString() {
			return Objects.toStringHelper(this.getClass()).add("seq", seq)
			                                              .add("len", payload.length)
			                                              .toString();
		}
	}

	private class DatagramPayload implements Comparable<DatagramPayload> {
		private Long offset;
		private byte[] payload;

		public DatagramPayload(Long offset, byte[] payload) {
			this.offset = offset;
			this.payload = payload;
		}

		@Override
		public int compareTo(DatagramPayload o) {
			return ComparisonChain.start().compare(offset, o.offset)
			                              .compare(payload.length, o.payload.length)
			                              .result();
		}

		public boolean linked(DatagramPayload o) {
			if ((offset + payload.length) == o.offset)
				return true;
			if ((o.offset + o.payload.length) == offset)
				return true;
			return false;
		}

		@Override
		public String toString() {
			return Objects.toStringHelper(this.getClass()).add("offset", offset)
			                                              .add("len", payload.length)
			                                              .toString();
		}
	}
}
