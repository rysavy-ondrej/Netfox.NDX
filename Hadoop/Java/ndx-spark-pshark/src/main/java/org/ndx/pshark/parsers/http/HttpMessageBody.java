package org.ndx.pshark.parsers.http;

import java.util.Stack;
import java.util.ArrayList;
import java.util.Properties;
import java.io.File;
import java.io.FileReader;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.IOException;


public class HttpMessageBody extends Rule
{
    public HttpMessageBody(String spelling, ArrayList<Rule> rules)
    {
      super(spelling, rules);
    }
    public Object accept(Visitor visitor)
    {
      return visitor.visit(this);
    }
  
    public static HttpMessageBody parse(ParserContext context)
    {
        return null;
    }    
}