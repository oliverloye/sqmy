using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace WebControllers.Helpers
{
    public class CustomXMLValueParser
    {
        public static string UnescapeXML(string xmlString)
        {
            if (xmlString == null)
                throw new ArgumentNullException("xmlString");
            return xmlString.Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");
        }

        public static string EscapeXML(string xmlString)
        {

            if (xmlString == null)
                throw new ArgumentNullException("xmlString");
            return xmlString.Replace("'", "&apos;").Replace("\"", "&quot;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;");
        }
    }
}