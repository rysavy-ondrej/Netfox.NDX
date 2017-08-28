package org.ndx.model;

import java.util.HashMap;
import java.util.Map;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.ndx.model.FlowModel.FlowKey;
import org.ndx.model.PacketModel.RawFrame;
import java.util.function.Consumer;
import com.google.protobuf.ByteString;

public class Packet extends HashMap<String, Object> {
    private static final long serialVersionUID = 8723206921174160146L;
    public static final Log LOG = LogFactory.getLog(Packet.class);
	private static Map<Integer, String> protocols;
	public static final String PROTOCOL_ICMP = "ICMP";
	public static final String PROTOCOL_TCP = "TCP";
	public static final String PROTOCOL_UDP = "UDP";
	public static final String PROTOCOL_FRAGMENT = "Fragment";
	static {
		protocols = new HashMap<Integer, String>();
		protocols.put(1, PROTOCOL_ICMP);
		protocols.put(6, PROTOCOL_TCP);
		protocols.put(17, PROTOCOL_UDP);
		protocols.put(44, PROTOCOL_FRAGMENT); // Using IPv4 fragment protocol number across protocols (see http://www.iana.org/assignments/protocol-numbers/protocol-numbers.xhtml)
	}
	public static final String TIMESTAMP = "ts";
    public static final String TIMESTAMP_USEC = "ts_usec";
	public static final String TIMESTAMP_MICROS = "ts_micros";
	public static final String NUMBER = "number";
	
	public static final String TTL = "ttl";
	public static final String IP_VERSION = "ip_version";	
	public static final String IP_HEADER_LENGTH = "ip_header_length";	
	public static final String IP_FLAGS_DF = "ip_flags_df";
	public static final String IP_FLAGS_MF = "ip_flags_mf";
	public static final String IPV6_FLAGS_M = "ipv6_flags_m";
	public static final String FRAGMENT_OFFSET = "fragment_offset";
	public static final String FRAGMENT = "fragment";
	public static final String LAST_FRAGMENT = "last_fragment";
	public static final String PROTOCOL = "protocol";
	public static final String SRC = "src";
	public static final String DST = "dst";
	public static final String ID = "id";
	public static final String SRC_PORT = "src_port";
	public static final String DST_PORT = "dst_port";
	public static final String TCP_HEADER_LENGTH = "tcp_header_length";
	public static final String TCP_SEQ = "tcp_seq";
	public static final String TCP_ACK = "tcp_ack";
	public static final String LEN = "len";
	public static final String UDPSUM = "udpsum";
	public static final String UDP_LENGTH = "udp_length";
	public static final String TCP_FLAG_NS = "tcp_flag_ns";
	public static final String TCP_FLAG_CWR = "tcp_flag_cwr";
	public static final String TCP_FLAG_ECE = "tcp_flag_ece";
	public static final String TCP_FLAG_URG = "tcp_flag_urg";
	public static final String TCP_FLAG_ACK = "tcp_flag_ack";
	public static final String TCP_FLAG_PSH = "tcp_flag_psh";
	public static final String TCP_FLAG_RST = "tcp_flag_rst";
	public static final String TCP_FLAG_SYN = "tcp_flag_syn";
	public static final String TCP_FLAG_FIN = "tcp_flag_fin";
	public static final String REASSEMBLED_TCP_FRAGMENTS = "reassembled_tcp_fragments";
	public static final String REASSEMBLED_DATAGRAM_FRAGMENTS = "reassembled_datagram_fragments";
	public static final String PAYLOAD_LEN = "payload_len";
    
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
	
	public String getFlowString() {
    	StringBuffer sb = new StringBuffer();
		sb.append("[");
		sb.append(this.get(Packet.PROTOCOL));
		sb.append("@");
		sb.append(this.get(Packet.SRC));
		sb.append(":");
		sb.append(this.get(Packet.SRC_PORT));
		sb.append("->");
		sb.append(this.get(Packet.DST));
		sb.append(":");
		sb.append(this.get(Packet.DST_PORT));
		sb.append("]");
		return sb.toString();
	}

    public static FlowModel.FlowKey flowKeyParse(String flowkey)
    {
		String[] parts = flowkey.split("\\[|@|:|->|\\]");
		FlowKey.Builder fb = FlowKey.newBuilder();
		fb.setProtocol(ByteString.copyFromUtf8(parts[1]));
		fb.setSourceAddress(ByteString.copyFromUtf8(parts[2]));
		fb.setSourceSelector(ByteString.copyFromUtf8(parts[3]));		
		fb.setDestinationAddress(ByteString.copyFromUtf8(parts[4]));
		fb.setDestinationSelector(ByteString.copyFromUtf8(parts[5]));
		return fb.build();
	}
	
	/**
	 * Returns FlowKey for the current Packet.
	 * @return FlowKey for the current Packet.
	 */
	public FlowModel.FlowKey getFlowKey()
	{
		FlowModel.FlowKey.Builder fb = FlowModel.FlowKey.newBuilder();
		fb.setProtocol(ByteString.copyFromUtf8(get(Packet.PROTOCOL).toString()));
		fb.setSourceAddress(ByteString.copyFromUtf8(get(Packet.SRC).toString() ));
		fb.setSourceSelector(ByteString.copyFromUtf8(get(Packet.SRC_PORT).toString() ));		
		fb.setDestinationAddress(ByteString.copyFromUtf8(get(Packet.DST).toString() ));
		fb.setDestinationSelector(ByteString.copyFromUtf8(get(Packet.DST_PORT).toString() ));
		return fb.build();
	}




	public String getSessionString()
	{
		String loAddress;
		String hiAddress;
		Integer loPort;
		Integer hiPort;
		if (((String)get(Packet.SRC)).compareTo((String)get(Packet.DST)) == 0)
		{
			loAddress = (String)get(Packet.SRC);
			hiAddress = (String)get(Packet.DST);
			if ((Integer)get(Packet.SRC_PORT) < (Integer)get(Packet.DST_PORT))
			{
				loPort = (Integer)get(Packet.SRC_PORT);
				hiPort = (Integer)get(Packet.DST_PORT);				
			}
			else
			{
				loPort = (Integer)get(Packet.DST_PORT);
				hiPort = (Integer)get(Packet.SRC_PORT);	
			}
		}
		else if (((String)get(Packet.SRC)).compareTo((String)get(Packet.DST)) < 0)
		{
			loAddress = (String)get(Packet.SRC);
			loPort = (Integer)get(Packet.SRC_PORT);
			hiAddress = (String)get(Packet.DST);
			hiPort = (Integer)get(Packet.DST_PORT);
		}
		else
		{
			loAddress = (String)get(Packet.DST);
			loPort = (Integer)get(Packet.DST_PORT);
			hiAddress = (String)get(Packet.SRC);
			hiPort = (Integer)get(Packet.SRC_PORT);
		}
		

		StringBuffer sb = new StringBuffer();
		sb.append("[");
		sb.append(this.get(Packet.PROTOCOL));
		sb.append("@");
		sb.append(loAddress);
		sb.append(":");
		sb.append(loPort);
		sb.append("<->");
		sb.append(hiAddress);
		sb.append(":");
		sb.append(hiPort);
		sb.append("]");
		return sb.toString();
	}

	/**
	 * Extends the collection of attributes of the current Packet with the provided colleciton. 
	 * 
	 * @param prefix The prefix to be used when adding attributes. If null then no prefix will be used.
	 * @param source The source collection of the attributes. It can be null. 
	 */
	public void extendWith(String prefix, HashMap<String,Object> source)
	{
		if (source == null) return;
		for (Map.Entry<String,Object> entry : source.entrySet()) {
			this.put(prefix == null ? entry.getKey() : prefix + "." + entry.getKey(), entry.getValue());	
		}
	}

	@Override
	public String toString() {
		StringBuffer sb = new StringBuffer();
		for (Map.Entry<String, Object> entry : entrySet()) {
			sb.append(entry.getKey());
			sb.append('=');
			sb.append(entry.getValue());
			sb.append(',');
		}
		if (sb.length() > 0)
			return sb.substring(0, sb.length() - 1);
		return null;
	}

	/**
	 * Attempts to parse the input RawFrame into Packet.
	 * @param frame An input frame to be parsed.
	 * @return a Packet object for the given input RawFrame.
	 */
	public static Packet parsePacket(RawFrame frame)
	{
		return parsePacket(frame, null);
	}

    public static Packet parsePacket(RawFrame frame, Consumer<PacketPayload> processPayload)
    {
        return parsePacket(frame.getLinkTypeValue(), frame.getTimeStamp() , frame.getFrameNumber(), frame.getData().toByteArray(), 65535, processPayload);   
    }
    public static Packet parsePacket(int linkType, long timestamp, int number, byte[] packetData, int snapLen, Consumer<PacketPayload> processPayload)
    {
		Packet packet = new Packet();
		packet.put(Packet.TIMESTAMP, timestamp);
		packet.put(Packet.NUMBER, number);

        int ipStart = findIPStart(linkType, packetData);
		if (ipStart == -1)
			return packet;

		int ipProtocolHeaderVersion = getInternetProtocolHeaderVersion(packetData, ipStart);
		packet.put(Packet.IP_VERSION, ipProtocolHeaderVersion);

		if (ipProtocolHeaderVersion == 4 || ipProtocolHeaderVersion == 6) {
			int ipHeaderLen = getInternetProtocolHeaderLength(packetData, ipProtocolHeaderVersion, ipStart);
			int totalLength = 0;
			if (ipProtocolHeaderVersion == 4) {
				buildInternetProtocolV4Packet(packet, packetData, ipStart);
				totalLength = BitConverter.convertShort(packetData, ipStart + IP_TOTAL_LEN_OFFSET);
			} else if (ipProtocolHeaderVersion == 6) {
				buildInternetProtocolV6Packet(packet, packetData, ipStart);
				ipHeaderLen += buildInternetProtocolV6ExtensionHeaderFragment(packet, packetData, ipStart);
				int payloadLength = BitConverter.convertShort(packetData, ipStart + IPV6_PAYLOAD_LEN_OFFSET);
				totalLength = payloadLength + IPV6_HEADER_SIZE;
			}
			packet.put(Packet.IP_HEADER_LENGTH, ipHeaderLen);

			if ((Boolean)packet.get(Packet.FRAGMENT)) {
				LOG.info("IP fragment detected - fragmented packets are not supported.");
			} else {
			    String protocol = (String)packet.get(Packet.PROTOCOL);
			    int payloadDataStart = ipStart + ipHeaderLen;
			    int payloadLength = totalLength - ipHeaderLen;
			    byte[] packetPayload = packet.readPayload(packetData, payloadDataStart, payloadLength, snapLen);
                if (PROTOCOL_UDP == protocol || PROTOCOL_TCP == protocol) {
				    packetPayload = packet.buildTcpAndUdpPacket(packetData, ipProtocolHeaderVersion, ipStart, ipHeaderLen, totalLength, snapLen);
				}
			
			    packet.put(Packet.LEN, packetPayload.length);
                packet.processPacketPayload(packetPayload, processPayload);
            }
        }
        return packet;
    }

	/** 
	 * This method call function for further processing the content of TCP or UDP segment.
	 * @param payload 			payload of the packet, it is the content of UDP or TCP segment
	 * @param processPayload	function that is called for processing the content. It can be null.
	 */
	private void processPacketPayload(byte[] payload, Consumer<PacketPayload> processPayload) 
	{
		if (processPayload != null)
		{
			processPayload.accept(new PacketPayload(this, payload));
		}
	}

    private static int findIPStart(int linkType, byte[] packet) {
		int start = -1;
		switch (linkType) {
			case Constants.DataLinkType.Null_VALUE:
				return 4;
			case Constants.DataLinkType.Ethernet_VALUE:
				start = ETHERNET_HEADER_SIZE;
				int etherType = BitConverter.convertShort(packet, ETHERNET_TYPE_OFFSET);
				if (etherType == ETHERNET_TYPE_8021Q) {
					etherType = BitConverter.convertShort(packet, ETHERNET_TYPE_OFFSET + 4);
					start += 4;
				}
				if (etherType == ETHERNET_TYPE_IP || etherType == ETHERNET_TYPE_IPV6)
					return start;
				break;
			case Constants.DataLinkType.Raw_VALUE:
				return 0;
			case Constants.DataLinkType.Loop_VALUE:
				return 4;
			case Constants.DataLinkType.LinuxSLL_VALUE:
			    start = SLL_HEADER_BASE_SIZE;
				int sllAddressLength = BitConverter.convertShort(packet, SLL_ADDRESS_LENGTH_OFFSET);
				start += sllAddressLength;
				return start;
		}
		return -1;
	}

	private static int getInternetProtocolHeaderLength(byte[] packet, int ipProtocolHeaderVersion, int ipStart) {
		if (ipProtocolHeaderVersion == 4)
			return (packet[ipStart + IP_VHL_OFFSET] & 0xF) * 4;
		else if (ipProtocolHeaderVersion == 6)
			return 40;
		return -1;
	}

	private static int getInternetProtocolHeaderVersion(byte[] packet, int ipStart) {
		return (packet[ipStart + IP_VHL_OFFSET] >> 4) & 0xF;
	}

	private static int getTcpHeaderLength(byte[] packet, int tcpStart) {
		int dataOffset = tcpStart + TCP_HEADER_DATA_OFFSET;
		return ((packet[dataOffset] >> 4) & 0xF) * 4;
	}
    public static String convertProtocolIdentifier(int identifier) {
		return protocols.get(identifier);
	}
	private static void buildInternetProtocolV4Packet(Packet packet, byte[] packetData, int ipStart) {
		long id = new Long(BitConverter.convertShort(packetData, ipStart + IP_ID_OFFSET));
		packet.put(Packet.ID, id);

		int flags = packetData[ipStart + IP_FLAGS_OFFSET] & 0xE0;
		packet.put(Packet.IP_FLAGS_DF, (flags & 0x40) == 0 ? false : true);
		packet.put(Packet.IP_FLAGS_MF, (flags & 0x20) == 0 ? false : true);

		long fragmentOffset = (BitConverter.convertShort(packetData, ipStart + IP_FRAGMENT_OFFSET) & 0x1FFF) * 8;
		packet.put(Packet.FRAGMENT_OFFSET, fragmentOffset);

		if ((flags & 0x20) != 0 || fragmentOffset != 0) {
			packet.put(Packet.FRAGMENT, true);
			packet.put(Packet.LAST_FRAGMENT, ((flags & 0x20) == 0 && fragmentOffset != 0));
		} else {
			packet.put(Packet.FRAGMENT, false);
		}

		int ttl = packetData[ipStart + IP_TTL_OFFSET] & 0xFF;
		packet.put(Packet.TTL, ttl);

		int protocol = packetData[ipStart + IP_PROTOCOL_OFFSET];
		packet.put(Packet.PROTOCOL, convertProtocolIdentifier(protocol));

		String src = BitConverter.convertAddress(packetData, ipStart + IP_SRC_OFFSET, 4);
		packet.put(Packet.SRC, src);

		String dst = BitConverter.convertAddress(packetData, ipStart + IP_DST_OFFSET, 4);
		packet.put(Packet.DST, dst);
	}

	private static void buildInternetProtocolV6Packet(Packet packet, byte[] packetData, int ipStart) {
		int ttl = packetData[ipStart + IPV6_HOPLIMIT_OFFSET] & 0xFF;
		packet.put(Packet.TTL, ttl);

		int protocol = packetData[ipStart + IPV6_NEXTHEADER_OFFSET];
		packet.put(Packet.PROTOCOL, convertProtocolIdentifier(protocol));

		String src = BitConverter.convertAddress(packetData, ipStart + IPV6_SRC_OFFSET, 16);
		packet.put(Packet.SRC, src);

		String dst = BitConverter.convertAddress(packetData, ipStart + IPV6_DST_OFFSET, 16);
		packet.put(Packet.DST, dst);
	}

	private static int buildInternetProtocolV6ExtensionHeaderFragment(Packet packet, byte[] packetData, int ipStart) {
		if (PROTOCOL_FRAGMENT.equals((String)packet.get(Packet.PROTOCOL))) {
			long id = BitConverter.convertUnsignedInt(packetData, ipStart + IPV6_HEADER_SIZE + IPV6_ID_OFFSET);
			packet.put(Packet.ID, id);

			int flags = packetData[ipStart + IPV6_HEADER_SIZE + IPV6_FLAGS_OFFSET] & 0x7;
			packet.put(Packet.IPV6_FLAGS_M, (flags & 0x1) == 0 ? false : true);

			long fragmentOffset = BitConverter.convertShort(packetData, ipStart + IPV6_HEADER_SIZE + IPV6_FRAGMENT_OFFSET) & 0xFFF8;
			packet.put(Packet.FRAGMENT_OFFSET, fragmentOffset);

			packet.put(Packet.FRAGMENT, true);
			packet.put(Packet.LAST_FRAGMENT, ((flags & 0x1) == 0 && fragmentOffset != 0));

			int protocol = packetData[ipStart + IPV6_HEADER_SIZE];
			packet.put(Packet.PROTOCOL, convertProtocolIdentifier(protocol)); // Change protocol to value from fragment header

			return 8; // Return fragment header extension length
		}

		// Not a fragment
		packet.put(Packet.FRAGMENT, false);
		return 0;
	}

	/*
	 * packetData is the entire layer 2 packet read from pcap
	 * ipStart is the start of the IP packet in packetData
	 */
	private byte[] buildTcpAndUdpPacket(byte[] packetData, int ipProtocolHeaderVersion, int ipStart, int ipHeaderLen, int totalLength, int snapLen) {
		this.put(Packet.SRC_PORT, BitConverter.convertShort(packetData, ipStart + ipHeaderLen + PROTOCOL_HEADER_SRC_PORT_OFFSET));
		this.put(Packet.DST_PORT, BitConverter.convertShort(packetData, ipStart + ipHeaderLen + PROTOCOL_HEADER_DST_PORT_OFFSET));

		int tcpOrUdpHeaderSize;
		final String protocol = (String)this.get(Packet.PROTOCOL);
		if (PROTOCOL_UDP.equals(protocol)) {
			tcpOrUdpHeaderSize = UDP_HEADER_SIZE;

			if (ipProtocolHeaderVersion == 4) {
				int cksum = getUdpChecksum(packetData, ipStart, ipHeaderLen);
				if (cksum >= 0)
					this.put(Packet.UDPSUM, cksum);
			}
			// TODO UDP Checksum for IPv6 packets

			int udpLen = getUdpLength(packetData, ipStart, ipHeaderLen);
			this.put(Packet.UDP_LENGTH, udpLen);
			this.put(Packet.PAYLOAD_LEN, udpLen);
		} else if (PROTOCOL_TCP.equals(protocol)) {
			tcpOrUdpHeaderSize = getTcpHeaderLength(packetData, ipStart + ipHeaderLen);
			this.put(Packet.TCP_HEADER_LENGTH, tcpOrUdpHeaderSize);

			// Store the sequence and acknowledgement numbers --M
			this.put(Packet.TCP_SEQ, BitConverter.convertUnsignedInt(packetData, ipStart + ipHeaderLen + PROTOCOL_HEADER_TCP_SEQ_OFFSET));
			this.put(Packet.TCP_ACK, BitConverter.convertUnsignedInt(packetData, ipStart + ipHeaderLen + PROTOCOL_HEADER_TCP_ACK_OFFSET));

			// Flags stretch two bytes starting at the TCP header offset
			int flags = BitConverter.convertShort(new byte[] { packetData[ipStart + ipHeaderLen + TCP_HEADER_DATA_OFFSET],
			                                                     packetData[ipStart + ipHeaderLen + TCP_HEADER_DATA_OFFSET + 1] })
			                                       & 0x1FF; // Filter first 7 bits. First 4 are the data offset and the other 3 reserved for future use.
												   this.put(Packet.TCP_FLAG_NS, (flags & 0x100) == 0 ? false : true);
												   this.put(Packet.TCP_FLAG_CWR, (flags & 0x80) == 0 ? false : true);
												   this.put(Packet.TCP_FLAG_ECE, (flags & 0x40) == 0 ? false : true);
												   this.put(Packet.TCP_FLAG_URG, (flags & 0x20) == 0 ? false : true);
												   this.put(Packet.TCP_FLAG_ACK, (flags & 0x10) == 0 ? false : true);
												   this.put(Packet.TCP_FLAG_PSH, (flags & 0x8)  == 0 ? false : true);
												   this.put(Packet.TCP_FLAG_RST, (flags & 0x4)  == 0 ? false : true);
												   this.put(Packet.TCP_FLAG_SYN, (flags & 0x2)  == 0 ? false : true);
												   this.put(Packet.TCP_FLAG_FIN, (flags & 0x1)  == 0 ? false : true);
			// The TCP payload size is calculated by taking the "Total Length" from the IP header (ip.len) 
			// and then substract the "IP header length" (ip.hdr_len) and the "TCP header length" (tcp.hdr_len).
			int tcpLen = totalLength-(tcpOrUdpHeaderSize + ipHeaderLen);
			this.put(Packet.PAYLOAD_LEN, tcpLen);
		} else {
			return null;
		}

		int payloadDataStart = ipStart + ipHeaderLen + tcpOrUdpHeaderSize;
		int payloadLength = totalLength - ipHeaderLen - tcpOrUdpHeaderSize;
		byte[] data = readPayload(packetData, payloadDataStart, payloadLength, snapLen);
		return data;
    }
    private int getUdpChecksum(byte[] packetData, int ipStart, int ipHeaderLen) {
		/*
		 * No Checksum on this packet?
		 */
		if (packetData[ipStart + ipHeaderLen + 6] == 0 &&
		    packetData[ipStart + ipHeaderLen + 7] == 0)
			return -1;

		/*
		 * Build data[] that we can checksum.  Its a pseudo-header
		 * followed by the entire UDP packet.
		 */
		byte data[] = new byte[packetData.length - ipStart - ipHeaderLen + 12];
		int sum = 0;
		System.arraycopy(packetData, ipStart + IP_SRC_OFFSET,      data, 0, 4);
		System.arraycopy(packetData, ipStart + IP_DST_OFFSET,      data, 4, 4);
		data[8] = 0;
		data[9] = 17;	/* IPPROTO_UDP */
		System.arraycopy(packetData, ipStart + ipHeaderLen + 4,    data, 10, 2);
		System.arraycopy(packetData, ipStart + ipHeaderLen,        data, 12, packetData.length - ipStart - ipHeaderLen);
		for (int i = 0; i<data.length; i++) {
			int j = data[i];
			if (j < 0)
				j += 256;
			sum += j << (i % 2 == 0 ? 8 : 0);
		}
		sum = (sum >> 16) + (sum & 0xffff);
		sum += (sum >> 16);
		return (~sum) & 0xffff;
	}

	private int getUdpLength(byte[] packetData, int ipStart, int ipHeaderLen) {
		int udpLen = BitConverter.convertShort(packetData, ipStart + ipHeaderLen + 4);
		return udpLen;
    }
    
    private byte[] readPayload(byte[] packetData, int payloadDataStart, int payloadLength, int snapLen) {
		if (payloadLength < 0) {
			LOG.warn("Malformed packet - negative payload length. Returning empty payload.");
			return new byte[0];
		}
		if (payloadDataStart > packetData.length) {
			LOG.warn("Payload start (" + payloadDataStart + ") is larger than packet data (" + packetData.length + "). Returning empty payload.");
			return new byte[0];
		}
		if (payloadDataStart + payloadLength > packetData.length) {
			if (payloadDataStart + payloadLength <= snapLen) // Only corrupted if it was not because of a reduced snap length
				LOG.warn("Payload length field value (" + payloadLength + ") is larger than available packet data (" 
						+ (packetData.length - payloadDataStart) 
						+ "). Packet may be corrupted. Returning only available data.");
			payloadLength = packetData.length - payloadDataStart;
		}
		byte[] data = new byte[payloadLength];
		System.arraycopy(packetData, payloadDataStart, data, 0, payloadLength);
		return data;
	}
}