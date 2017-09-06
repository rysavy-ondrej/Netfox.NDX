/* -----------------------------------------------------------------------------
 * Visitor.java
 * -----------------------------------------------------------------------------
 *
 * Producer : com.parse2.aparse.Parser 2.5
 * Produced : Wed Sep 06 18:02:57 CEST 2017
 *
 * -----------------------------------------------------------------------------
 */

package org.ndx.pshark.parsers.http;

public interface Visitor
{
  public Object visit(Rule_Message rule);
  public Object visit(Rule_Request rule);
  public Object visit(Rule_Request_Line rule);
  public Object visit(Rule_Request_Method rule);
  public Object visit(Rule_HTTP_Version rule);
  public Object visit(Rule_Request_Uri rule);
  public Object visit(Rule_Header_Line rule);
  public Object visit(Rule_Field_Name rule);
  public Object visit(Rule_Field_Value rule);
  public Object visit(Rule_Response rule);
  public Object visit(Rule_Status_Line rule);
  public Object visit(Rule_Status_Code rule);
  public Object visit(Rule_Reason_Phrase rule);
  public Object visit(Rule_Message_Body rule);
  public Object visit(Rule_chunk_line rule);
  public Object visit(Rule_mid_chunk rule);
  public Object visit(Rule_chunk_size rule);
  public Object visit(Rule_last_chunk rule);
  public Object visit(Rule_OCTET rule);
  public Object visit(Rule_CHAR rule);
  public Object visit(Rule_UPALPHA rule);
  public Object visit(Rule_LOALPHA rule);
  public Object visit(Rule_ALPHA rule);
  public Object visit(Rule_DIGIT rule);
  public Object visit(Rule_CTL rule);
  public Object visit(Rule_CR rule);
  public Object visit(Rule_LF rule);
  public Object visit(Rule_SP rule);
  public Object visit(Rule_HT rule);
  public Object visit(Rule_CRLF rule);
  public Object visit(Rule_LWS rule);
  public Object visit(Rule_TEXT_NOCRLF rule);
  public Object visit(Rule_TEXT_NOSP rule);
  public Object visit(Rule_TEXT rule);
  public Object visit(Rule_HEX rule);
  public Object visit(Rule_token rule);
  public Object visit(Rule_comment rule);
  public Object visit(Rule_ctext rule);
  public Object visit(Rule_quoted_string rule);
  public Object visit(Rule_qdtext rule);
  public Object visit(Rule_quoted_pair rule);
  public Object visit(HttpMessageBody rule);

  public Object visit(Terminal_StringValue value);
  public Object visit(Terminal_NumericValue value);
}

/* -----------------------------------------------------------------------------
 * eof
 * -----------------------------------------------------------------------------
 */
