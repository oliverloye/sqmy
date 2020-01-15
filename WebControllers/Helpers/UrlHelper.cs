using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebControllers.Helpers
{
    public class UrlHelper
    {
        public static string IndexFileEnglish()
        {
            return "C:/inetpub/sonywebshopsolutions/icecat_xml_files/files.index.xml";
        }
        public static string IndexFileDanish()
        {
            return "C:/inetpub/sonywebshopsolutions/icecat_xml_files/files.index.dk.xml";
        }
        public static string DailyEnglish()
        {
            return "https://data.Icecat.biz/export/freexml/EN/daily.index.xml";
        }
        public static string DailyDanish()
        {
            return "https://data.Icecat.biz/export/freexml/DK/daily.index.xml";
        }
        public static string CategoryXml()
        {
            //Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "icecat_xml_files/CategoriesList.xml");
            return "C:/inetpub/sonywebshopsolutions/icecat_xml_files/CategoriesList.xml";
        }
    }
}