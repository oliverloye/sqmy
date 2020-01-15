using ClassLibrary.Entity;
using ClassLibrary.Solr.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using UmbracoWebDev.Models.Cart;

namespace UmbracoWebDev.Models.Solr
{
    public class SolrProducts : RenderModel
    {
        public SolrProducts(IPublishedContent content) : base(content)
        { }

        public Product SingleProduct { get; set; }

        public string Query { get; set; }

        public CartItem CartItem { get; set; }

        public List<Product> ProductList { get; set; }
        public List<SolrFacet> RelevantCategories { get; set; }
    }
}