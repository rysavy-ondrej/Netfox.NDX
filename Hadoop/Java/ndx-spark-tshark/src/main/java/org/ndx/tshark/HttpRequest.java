package org.ndx.tshark;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.StringReader;
import java.util.Date;
import java.util.HashMap;
import org.ndx.model.Packet;
import org.ndx.model.Statistics;
/**
 * Class for HTTP request parsing as defined by RFC 2612:
 * 
 * Request = Request-Line ; Section 5.1 (( general-header ; Section 4.5 |
 * request-header ; Section 5.3 | entity-header ) CRLF) ; Section 7.1 CRLF [
 * message-body ] ; Section 4.3
 * 
 * @author izelaya
 *
 */
public class HttpRequest extends HashMap<String, Object> {
    private static final long serialVersionUID = 8723225921174160156L;
    private StringBuffer _messagetBody;

    public HttpRequest() {
        _messagetBody = new StringBuffer();
    }

    /**
     * Tries to parse provided payload to HttpRequest
     */
    public static HttpRequest tryParseRequest(byte [] bytes)
    {
        try {
            String requestString = new String(bytes, "ASCII");
            return parseRequest(requestString);  
        } catch (Exception e) {
            return null;
        }
    }

    /**
     * Parse and HTTP request.
     * 
     * @param request
     *            String holding http request.
     * @throws IOException
     *             If an I/O error occurs reading the input stream.
     * @throws HttpFormatException
     *             If HTTP Request is malformed
     */
    public static HttpRequest parseRequest(String requestString) throws IOException, HttpFormatException {
        BufferedReader reader = new BufferedReader(new StringReader(requestString));

        HttpRequest request = new HttpRequest();
        request.setRequestLine(reader.readLine()); // Request-Line ; Section 5.1

        String header = reader.readLine();
        while (header.length() > 0) {
            request.appendHeaderParameter(header);
            header = reader.readLine();
        }

        String bodyLine = reader.readLine();
        while (bodyLine != null) {
            request.appendMessageBody(bodyLine);
            bodyLine = reader.readLine();
        }
        request.put("request", true);  
        return request;
    }

    /**
     * 
     * 5.1 Request-Line The Request-Line begins with a method token, followed by
     * the Request-URI and the protocol version, and ending with CRLF. The
     * elements are separated by SP characters. No CR or LF is allowed except in
     * the final CRLF sequence.
     * 
     * @return String with Request-Line
     */
    public String getRequestLine() {
        return (String)this.get("request.line");
    }

    private void setRequestLine(String requestLine) throws HttpFormatException {
        if (requestLine == null || requestLine.length() == 0) {
            throw new HttpFormatException("Invalid Request-Line: " + requestLine);
        }
        this.put("request.line",requestLine);
    }

    private void appendHeaderParameter(String header) throws HttpFormatException {
        int idx = header.indexOf(":");
        if (idx == -1) {
            throw new HttpFormatException("Invalid Header Parameter: " + header);
        }
        this.put(header.substring(0, idx).toLowerCase(), header.substring(idx + 1, header.length()));
    }

    /**
     * The message-body (if any) of an HTTP message is used to carry the
     * entity-body associated with the request or response. The message-body
     * differs from the entity-body only when a transfer-coding has been
     * applied, as indicated by the Transfer-Encoding header field (section
     * 14.41).
     * @return String with message-body
     */
    public String getMessageBody() {
        return _messagetBody.toString();
    }

    private void appendMessageBody(String bodyLine) {
        _messagetBody.append(bodyLine).append("\r\n");
    }

    /**
     * For list of available headers refer to sections: 4.5, 5.3, 7.1 of RFC 2616
     * @param headerName Name of header
     * @return String with the value of the header or null if not found.
     */
    public String getHeaderParam(String headerName){
        return (String)this.get(headerName);
    }


    /** 
     * Creates log like format for the provided Packet that contains HTTP request.
     * TIMESTAMP SRC_IP -> DST_IP : HOST 'REQUEST LINE' 
     */
    public static String format(Packet packet)
    {
        Date ts = Statistics.ticksToDate((Long)packet.get(Packet.TIMESTAMP));
        String src = packet.get(Packet.SRC).toString();
        String dst = packet.get(Packet.DST).toString();
        Object rline = packet.get("http.request.line");
        if (rline!=null)
        {
            return String.format("%30s %15s -> %15s: %s - '%s'",ts.toString(), src, dst, packet.get("http.host"), rline.toString());
        }
        else
        {
            return String.format("%30s %15s -> %15s: ???",ts.toString(), src, dst);   
        }
    }
}