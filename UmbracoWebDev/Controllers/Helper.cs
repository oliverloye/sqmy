using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebControllers.Models;

using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using WebControllers.Helpers;
using System.Text;

namespace UmbracoWebDev.Controllers
{
    public class Helper
    {
        public string CleanString(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace(@"\n", "<br/>");
            return sb.ToString();
        }
    }
}