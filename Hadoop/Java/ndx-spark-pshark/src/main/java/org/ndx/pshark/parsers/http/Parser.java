/* -----------------------------------------------------------------------------
 * Parser.java
 * -----------------------------------------------------------------------------
 *
 * Producer : com.parse2.aparse.Parser 2.5
 * Produced : Wed Sep 06 18:02:57 CEST 2017
 *
 * -----------------------------------------------------------------------------
 */

package org.ndx.pshark.parsers.http;

import java.util.Stack;
import java.util.Properties;
import java.io.File;
import java.io.FileReader;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.IOException;

public class Parser
{
  private Parser() {}

  static public void main(String[] args)
  {
    Properties arguments = new Properties();
    String error = "";
    boolean ok = args.length > 0;

    if (ok)
    {
      arguments.setProperty("Trace", "Off");
      arguments.setProperty("Rule", "Message");

      for (int i = 0; i < args.length; i++)
      {
        if (args[i].equals("-trace"))
          arguments.setProperty("Trace", "On");
        else if (args[i].equals("-visitor"))
          arguments.setProperty("Visitor", args[++i]);
        else if (args[i].equals("-file"))
          arguments.setProperty("File", args[++i]);
        else if (args[i].equals("-string"))
          arguments.setProperty("String", args[++i]);
        else if (args[i].equals("-rule"))
          arguments.setProperty("Rule", args[++i]);
        else
        {
          error = "unknown argument: " + args[i];
          ok = false;
        }
      }
    }

    if (ok)
    {
      if (arguments.getProperty("File") == null &&
          arguments.getProperty("String") == null)
      {
        error = "insufficient arguments: -file or -string required";
        ok = false;
      }
    }

    if (!ok)
    {
      System.out.println("error: " + error);
      System.out.println("usage: Parser [-rule rulename] [-trace] <-file file | -string string> [-visitor visitor]");
    }
    else
    {
      try
      {
        Rule rule = null;

        if (arguments.getProperty("File") != null)
        {
          rule = 
            parse(
              arguments.getProperty("Rule"), 
              new File(arguments.getProperty("File")), 
              arguments.getProperty("Trace").equals("On"));
        }
        else if (arguments.getProperty("String") != null)
        {
          rule = 
            parse(
              arguments.getProperty("Rule"), 
              arguments.getProperty("String"), 
              arguments.getProperty("Trace").equals("On"));
        }

        if (arguments.getProperty("Visitor") != null)
        {
          Visitor visitor = 
            (Visitor)Class.forName(arguments.getProperty("Visitor")).newInstance();
          rule.accept(visitor);
        }
      }
      catch (IllegalArgumentException e)
      {
        System.out.println("argument error: " + e.getMessage());
      }
      catch (IOException e)
      {
        System.out.println("io error: " + e.getMessage());
      }
      catch (ParserException e)
      {
        System.out.println("parser error: " + e.getMessage());
      }
      catch (ClassNotFoundException e)
      {
        System.out.println("visitor error: class not found - " + e.getMessage());
      }
      catch (IllegalAccessException e)
      {
        System.out.println("visitor error: illegal access - " + e.getMessage());
      }
      catch (InstantiationException e)
      {
        System.out.println("visitor error: instantiation failure - " + e.getMessage());
      }
    }
  }

  static public Rule parse(String rulename, String string)
  throws IllegalArgumentException,
         ParserException
  {
    return parse(rulename, string, false);
  }

  static public Rule parse(String rulename, InputStream in)
  throws IllegalArgumentException,
         IOException,
         ParserException
  {
    return parse(rulename, in, false);
  }

  static public Rule parse(String rulename, File file)
  throws IllegalArgumentException,
         IOException,
         ParserException
  {
    return parse(rulename, file, false);
  }

  static private Rule parse(String rulename, String string, boolean trace)
  throws IllegalArgumentException,
         ParserException
  {
    if (rulename == null)
      throw new IllegalArgumentException("null rulename");
    if (string == null)
      throw new IllegalArgumentException("null string");

    ParserContext context = new ParserContext(string, trace);

    Rule rule = null;
    if (rulename.equalsIgnoreCase("Message")) rule = Rule_Message.parse(context);
    else if (rulename.equalsIgnoreCase("Request")) rule = Rule_Request.parse(context);
    else if (rulename.equalsIgnoreCase("Request-Line")) rule = Rule_Request_Line.parse(context);
    else if (rulename.equalsIgnoreCase("Request-Method")) rule = Rule_Request_Method.parse(context);
    else if (rulename.equalsIgnoreCase("HTTP-Version")) rule = Rule_HTTP_Version.parse(context);
    else if (rulename.equalsIgnoreCase("Request-Uri")) rule = Rule_Request_Uri.parse(context);
    else if (rulename.equalsIgnoreCase("Header-Line")) rule = Rule_Header_Line.parse(context);
    else if (rulename.equalsIgnoreCase("Field-Name")) rule = Rule_Field_Name.parse(context);
    else if (rulename.equalsIgnoreCase("Field-Value")) rule = Rule_Field_Value.parse(context);
    else if (rulename.equalsIgnoreCase("Response")) rule = Rule_Response.parse(context);
    else if (rulename.equalsIgnoreCase("Status-Line")) rule = Rule_Status_Line.parse(context);
    else if (rulename.equalsIgnoreCase("Status-Code")) rule = Rule_Status_Code.parse(context);
    else if (rulename.equalsIgnoreCase("Reason-Phrase")) rule = Rule_Reason_Phrase.parse(context);
    else if (rulename.equalsIgnoreCase("Message-Body")) rule = Rule_Message_Body.parse(context);
    else if (rulename.equalsIgnoreCase("chunk-line")) rule = Rule_chunk_line.parse(context);
    else if (rulename.equalsIgnoreCase("mid-chunk")) rule = Rule_mid_chunk.parse(context);
    else if (rulename.equalsIgnoreCase("chunk-size")) rule = Rule_chunk_size.parse(context);
    else if (rulename.equalsIgnoreCase("last-chunk")) rule = Rule_last_chunk.parse(context);
    else if (rulename.equalsIgnoreCase("OCTET")) rule = Rule_OCTET.parse(context);
    else if (rulename.equalsIgnoreCase("CHAR")) rule = Rule_CHAR.parse(context);
    else if (rulename.equalsIgnoreCase("UPALPHA")) rule = Rule_UPALPHA.parse(context);
    else if (rulename.equalsIgnoreCase("LOALPHA")) rule = Rule_LOALPHA.parse(context);
    else if (rulename.equalsIgnoreCase("ALPHA")) rule = Rule_ALPHA.parse(context);
    else if (rulename.equalsIgnoreCase("DIGIT")) rule = Rule_DIGIT.parse(context);
    else if (rulename.equalsIgnoreCase("CTL")) rule = Rule_CTL.parse(context);
    else if (rulename.equalsIgnoreCase("CR")) rule = Rule_CR.parse(context);
    else if (rulename.equalsIgnoreCase("LF")) rule = Rule_LF.parse(context);
    else if (rulename.equalsIgnoreCase("SP")) rule = Rule_SP.parse(context);
    else if (rulename.equalsIgnoreCase("HT")) rule = Rule_HT.parse(context);
    else if (rulename.equalsIgnoreCase("CRLF")) rule = Rule_CRLF.parse(context);
    else if (rulename.equalsIgnoreCase("LWS")) rule = Rule_LWS.parse(context);
    else if (rulename.equalsIgnoreCase("TEXT_NOCRLF")) rule = Rule_TEXT_NOCRLF.parse(context);
    else if (rulename.equalsIgnoreCase("TEXT_NOSP")) rule = Rule_TEXT_NOSP.parse(context);
    else if (rulename.equalsIgnoreCase("TEXT")) rule = Rule_TEXT.parse(context);
    else if (rulename.equalsIgnoreCase("HEX")) rule = Rule_HEX.parse(context);
    else if (rulename.equalsIgnoreCase("token")) rule = Rule_token.parse(context);
    else if (rulename.equalsIgnoreCase("comment")) rule = Rule_comment.parse(context);
    else if (rulename.equalsIgnoreCase("ctext")) rule = Rule_ctext.parse(context);
    else if (rulename.equalsIgnoreCase("quoted-string")) rule = Rule_quoted_string.parse(context);
    else if (rulename.equalsIgnoreCase("qdtext")) rule = Rule_qdtext.parse(context);
    else if (rulename.equalsIgnoreCase("quoted-pair")) rule = Rule_quoted_pair.parse(context);
    else throw new IllegalArgumentException("unknown rule");

    if (rule == null)
    {
      throw new ParserException(
        "rule \"" + (String)context.getErrorStack().peek() + "\" failed",
        context.text,
        context.getErrorIndex(),
        context.getErrorStack());
    }

    if (context.text.length() > context.index)
    {
      ParserException primaryError = 
        new ParserException(
          "extra data found",
          context.text,
          context.index,
          new Stack<String>());

      if (context.getErrorIndex() > context.index)
      {
        ParserException secondaryError = 
          new ParserException(
            "rule \"" + (String)context.getErrorStack().peek() + "\" failed",
            context.text,
            context.getErrorIndex(),
            context.getErrorStack());

        primaryError.initCause(secondaryError);
      }

      throw primaryError;
    }

    return rule;
  }

  static private Rule parse(String rulename, InputStream in, boolean trace)
  throws IllegalArgumentException,
         IOException,
         ParserException
  {
    if (rulename == null)
      throw new IllegalArgumentException("null rulename");
    if (in == null)
      throw new IllegalArgumentException("null input stream");

    int ch = 0;
    StringBuffer out = new StringBuffer();
    while ((ch = in.read()) != -1)
      out.append((char)ch);

    return parse(rulename, out.toString(), trace);
  }

  static private Rule parse(String rulename, File file, boolean trace)
  throws IllegalArgumentException,
         IOException,
         ParserException
  {
    if (rulename == null)
      throw new IllegalArgumentException("null rulename");
    if (file == null)
      throw new IllegalArgumentException("null file");

    BufferedReader in = new BufferedReader(new FileReader(file));
    int ch = 0;
    StringBuffer out = new StringBuffer();
    while ((ch = in.read()) != -1)
      out.append((char)ch);

    in.close();

    return parse(rulename, out.toString(), trace);
  }
}

/* -----------------------------------------------------------------------------
 * eof
 * -----------------------------------------------------------------------------
 */
