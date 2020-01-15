using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebControllers.Helpers;

namespace ClassLibrary.Solr.Logic
{
    public class SolrQueryBuilder
    {
        private string query = ConnectionHelper.GetEnglishSolrConnection() + "select?q=";
        private string fquery = ConnectionHelper.GetEnglishSolrConnection() + "select?fq=";

        private string dkQuery = ConnectionHelper.GetDanishSolrConnection() + "select?q=";
        private string dkFquery = ConnectionHelper.GetDanishSolrConnection() + "select?fq=";

        public string BuildQuery(string filter)
        {
            query += filter;
            return query;
        }
        public string BuildDanishQuery(string filter)
        {
            dkQuery += filter;
            return dkQuery;
        }

        public string BuildFquery(string filter)
        {
            fquery += filter;
            return fquery;
        }
        public string BuildDanishFquery(string filter)
        {
            dkFquery += filter;
            return dkFquery;
        }
    }
}
