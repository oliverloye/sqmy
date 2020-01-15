using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace WebControllers.Helpers
{
    public class CustomStringCleaner
    {
        public string CleanString(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace(@"\n", " ");
            sb.Replace("<br>", string.Empty);
            sb.Replace("<br/>", string.Empty);
            sb.Replace("<b>", string.Empty);
            sb.Replace(@"\", string.Empty);
            //sb.Replace("&amp;", "and");
            //sb.Replace("&", "and");
            sb.Replace("</b>", string.Empty);
            return HttpUtility.HtmlDecode(sb.ToString());
        }

        public string CleanIdString(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace(@"\n", string.Empty);
            sb.Replace(@"/", string.Empty);
            sb.Replace("<br>", string.Empty);
            sb.Replace("<br/>", string.Empty);
            sb.Replace("<b>", string.Empty);
            sb.Replace(@"\", string.Empty);
            sb.Replace("</b>", string.Empty);
            sb.Replace(";", string.Empty);
            sb.Replace(":", string.Empty);
            sb.Replace("+", "-");
            return HttpUtility.HtmlDecode(sb.ToString());
        }
    }
}