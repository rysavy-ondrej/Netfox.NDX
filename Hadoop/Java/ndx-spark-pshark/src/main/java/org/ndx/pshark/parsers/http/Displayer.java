/* -----------------------------------------------------------------------------
 * Displayer.java
 * -----------------------------------------------------------------------------
 *
 * Producer : com.parse2.aparse.Parser 2.5
 * Produced : Wed Sep 06 18:02:57 CEST 2017
 *
 * -----------------------------------------------------------------------------
 */

package org.ndx.pshark.parsers.http;

import java.util.ArrayList;

public class Displayer implements Visitor
{

  public Object visit(Rule_Message rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Request rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Request_Line rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Request_Method rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_HTTP_Version rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Request_Uri rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Header_Line rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Field_Name rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Field_Value rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Response rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Status_Line rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Status_Code rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Reason_Phrase rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_Message_Body rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_chunk_line rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_mid_chunk rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_chunk_size rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_last_chunk rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_OCTET rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_CHAR rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_UPALPHA rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_LOALPHA rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_ALPHA rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_DIGIT rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_CTL rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_CR rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_LF rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_SP rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_HT rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_CRLF rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_LWS rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_TEXT_NOCRLF rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_TEXT_NOSP rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_TEXT rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_HEX rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_token rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_comment rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_ctext rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_quoted_string rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_qdtext rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(Rule_quoted_pair rule)
  {
    return visitRules(rule.rules);
  }

  public Object visit(HttpMessageBody rule)
  {
    System.out.print(rule.spelling);
    return null;
  }

  public Object visit(Terminal_StringValue value)
  {
    System.out.print(value.spelling);
    return null;
  }

  public Object visit(Terminal_NumericValue value)
  {
    System.out.print(value.spelling);
    return null;
  }

  private Object visitRules(ArrayList<Rule> rules)
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
