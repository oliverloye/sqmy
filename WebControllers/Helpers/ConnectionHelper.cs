

namespace WebControllers.Helpers
{
    public class ConnectionHelper
    {
        public static string GetEnglishSolrConnection()
        {
            return "http://praktikantiis01:9999/solr/sonyproducts/";
        }

        public static string GetDanishSolrConnection()
        {
            return "http://praktikantiis01:9999/solr/sonyproductsdk/";
        }

        public static string GetEnglishConnectionString()
        {
            return "server=PRAKTIKANTSQL01\\MSSQLSERVER2017;database=icecat_db;user id = umbraco2; password='umbraco2'";
        }

        public static string GetDanishConnectionString()
        {
            return "server=PRAKTIKANTSQL01\\MSSQLSERVER2017;database=icecat_db_multi;user id = umbraco2; password='umbraco2'";
        }

        public static string GetFeatureDbConnection()
        {
            return "server=PRAKTIKANTSQL01\\MSSQLSERVER2017;database=icecate_features_db;user id = umbraco2; password='umbraco2'";
        }
    }
}