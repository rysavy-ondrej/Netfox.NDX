/* -----------------------------------------------------------------------------
 * XmlDisplayer.java
 * -----------------------------------------------------------------------------
 *
 * Producer : com.parse2.aparse.Parser 2.5
 * Produced : Wed Sep 06 18:02:57 CEST 2017
 *
 * -----------------------------------------------------------------------------
 */

package org.ndx.pshark.parsers.http;

import java.util.ArrayList;

public class XmlDisplayer implements Visitor
{
  private boolean terminal = true;

  public Object visit(Rule_Message rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Message>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Message>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Request rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Request>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Request>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Request_Line rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Request-Line>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Request-Line>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Request_Method rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Request-Method>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Request-Method>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_HTTP_Version rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<HTTP-Version>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</HTTP-Version>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Request_Uri rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Request-Uri>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Request-Uri>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Header_Line rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Header-Line>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Header-Line>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Field_Name rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Field-Name>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Field-Name>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Field_Value rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Field-Value>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Field-Value>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Response rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Response>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Response>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Status_Line rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Status-Line>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Status-Line>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Status_Code rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Status-Code>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Status-Code>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Reason_Phrase rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Reason-Phrase>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Reason-Phrase>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_Message_Body rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<Message-Body>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</Message-Body>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_chunk_line rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<chunk-line>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</chunk-line>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_mid_chunk rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<mid-chunk>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</mid-chunk>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_chunk_size rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<chunk-size>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</chunk-size>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_last_chunk rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<last-chunk>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</last-chunk>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_OCTET rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<OCTET>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</OCTET>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_CHAR rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<CHAR>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</CHAR>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_UPALPHA rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<UPALPHA>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</UPALPHA>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_LOALPHA rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<LOALPHA>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</LOALPHA>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_ALPHA rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<ALPHA>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</ALPHA>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_DIGIT rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<DIGIT>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</DIGIT>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_CTL rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<CTL>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</CTL>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_CR rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<CR>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</CR>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_LF rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<LF>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</LF>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_SP rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<SP>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</SP>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_HT rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<HT>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</HT>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_CRLF rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<CRLF>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</CRLF>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_LWS rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<LWS>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</LWS>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_TEXT_NOCRLF rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<TEXT_NOCRLF>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</TEXT_NOCRLF>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_TEXT_NOSP rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<TEXT_NOSP>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</TEXT_NOSP>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_TEXT rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<TEXT>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</TEXT>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_HEX rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<HEX>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</HEX>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_token rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<token>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</token>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_comment rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<comment>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</comment>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_ctext rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<ctext>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</ctext>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_quoted_string rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<quoted-string>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</quoted-string>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_qdtext rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<qdtext>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</qdtext>");
    terminal = false;
    return null;
  }

  public Object visit(Rule_quoted_pair rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<quoted-pair>");
    terminal = false;
    visitRules(rule.rules);
    if (!terminal) System.out.println();
    System.out.print("</quoted-pair>");
    terminal = false;
    return null;
  }

  public Object visit(HttpMessageBody rule)
  {
    if (!terminal) System.out.println();
    System.out.print("<HttpMessageBody>");
    System.out.print(rule.spelling);
    System.out.print("</HttpMessageBody>");
    terminal = false;
    return null;
  }

  public Object visit(Terminal_StringValue value)
  {
    System.out.print(value.spelling);
    terminal = true;
    return null;
  }

  public Object visit(Terminal_NumericValue value)
  {
    System.out.print(value.spelling);
    terminal = true;
    return null;
  }

  private Boolean visitRules(ArrayList<Rule> rules)
  {
    for (Rule rule : rules)
      rule.accept(this);
    return null;
  }
}

/* -----------------------------------------------------------------------------
 * eof
 * -----------------------------------------------------------------------------
 */
