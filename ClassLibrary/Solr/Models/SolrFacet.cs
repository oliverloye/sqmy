using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Solr.Models
{
    public class SolrFacet
    {
        public string Category_Name { get; set; }
        public int Facet { get; set; }

        public SolrFacet()
        {

        }

        public SolrFacet(string cat, int facet)
        {
            Category_Name = cat;
            Facet = facet;
        }
    }
}
