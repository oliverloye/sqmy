using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Umbraco.Web.Models;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Umbraco.Web.Mvc;
using System.Data.SqlClient;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using WebControllers.Helpers;
using ClassLibrary.Entity;
using ClassLibrary.Solr;
using ClassLibrary.Solr.Logic;
using ClassLibrary.Solr.Models;
using UmbracoWebDev.Models.Solr;

namespace UmbracoWebDev.Controllers.SolrControllers
{
    public class SolrProductsController : RenderMvcController
    {
        static readonly HttpClient client = new HttpClient();

        private static string en_SqlConnection = ConnectionHelper.GetEnglishConnectionString();
        private static string Dk_SqlConnection = ConnectionHelper.GetDanishConnectionString();

        public static int NumberOfSearchResults = 0;

        public async Task<ActionResult> SolrProducts(RenderModel model, string query)
        {
            using (new TimeMeasure("SolrProducts"))
            {
                if (query != null)
                {
                    var test = query;
                    var myProducts = new SolrProducts(model.Content);
                    myProducts.ProductList = await GetSolrProduct(query);
                    if (Request.Url.AbsoluteUri.Contains("/shop/"))
                    {
                        myProducts.RelevantCategories = await GetAllFacets(ConnectionHelper.GetEnglishSolrConnection());
                    }
                    if (Request.Url.AbsoluteUri.Contains("/butik/"))
                    {
                        myProducts.RelevantCategories = await GetAllFacets(ConnectionHelper.GetDanishSolrConnection());
                    }
                    

                    return CurrentTemplate(myProducts);
                }
                else
                {
                    var myProducts = new SolrProducts(model.Content);
                    myProducts.ProductList = await GetSolrProduct("*%3A*&rows=30&start=0");
                    if (Request.Url.AbsoluteUri.Contains("/shop/"))
                    {
                        myProducts.RelevantCategories = await GetAllFacets(ConnectionHelper.GetEnglishSolrConnection());
                    }
                    if (Request.Url.AbsoluteUri.Contains("/butik/"))
                    {
                        myProducts.RelevantCategories = await GetAllFacets(ConnectionHelper.GetDanishSolrConnection());
                    }

                    return CurrentTemplate(myProducts);
                }
            }
        }
        public async Task<List<Product>> GetSolrProduct(string query)
        {
            using (new TimeMeasure("GetSolrProduct"))
            {

                try
                {
                    SolrQueryBuilder builder = new SolrQueryBuilder();
                    HttpResponseMessage response = null;
                    String strPath = HttpContext.Request.Url.PathAndQuery;
                    string[] test = query.Split(new[] { "&rows=" }, StringSplitOptions.None);
                    var count = test[0].Count(x => x == '&');

                    if (strPath.EndsWith("/") == false && query.Contains("TO")==false) {

                        string[] splitting = query.Split(new[] { "&rows=" }, StringSplitOptions.None);
                        string a = Server.UrlEncode(splitting[0]);
                        string b = "&rows=" + splitting[1];
                        query = a + b;
                    }
                    else if (test[0].Contains("&") == true && count>1)
                    {
                        string[] split1 = query.Split(new[] { "&rows=" }, StringSplitOptions.None);
                        string[] split2 = split1[0].Split(new[] { "q=" }, StringSplitOptions.None);
                        string sanitize = Server.UrlEncode(split2[1]);
                        string old_split = "&rows=" + split1[1];
                        query = split2[0]+"q="+sanitize+old_split;
                    }

                    if (Request.Url.AbsoluteUri.Contains("/butik/") && query.Contains("TO").Equals(true))
                    {
                        response = await client.GetAsync(builder.BuildDanishFquery(query));
                    }
                    else if (Request.Url.AbsoluteUri.Contains("/butik/") && query.Contains("TO").Equals(false))
                    {
                        response = await client.GetAsync(builder.BuildDanishQuery(query));
                    }
                    else if (Request.Url.AbsoluteUri.Contains("/shop/") && query.Contains("TO").Equals(true))
                    {
                        response = await client.GetAsync(builder.BuildFquery(query));
                    }
                    else if (Request.Url.AbsoluteUri.Contains("/shop/") && query.Contains("TO").Equals(false))
                    {
                        response = await client.GetAsync(builder.BuildQuery(query));
                    }
                    JObject responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                    JArray jProducts = (JArray)responseJson["response"]["docs"];
                    NumberOfSearchResults = (int)responseJson["response"]["numFound"];
                    List<Product> result = jProducts.Select(p => Converter(p)).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public async Task<List<SolrFacet>> GetAllFacets(string connectionString)
        {
            using (new TimeMeasure("GetAllFacets"))
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(connectionString + "select?facet.field=Product_Category_Name&facet=on&q=*%3A*");
                    JObject responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                    JArray jProducts = (JArray)responseJson["facet_counts"]["facet_fields"]["Product_Category_Name"];
                    List<SolrFacet> result = new List<SolrFacet>();
                    for (int i = 0; i < jProducts.Count; i = i + 2)
                    {
                        SolrFacet FacetRes = new SolrFacet
                        {
                            Category_Name = jProducts[i].ToString(),
                            Facet = (int)jProducts[i + 1]
                        };
                        result.Add(FacetRes);
                    }
                    return result;
                }
                catch (Exception e)
                {
                    throw null;
                }
            }
        }
        private Product Converter(JToken doc)
        {
            using (new TimeMeasure("Converter - solr productsController"))
            {


                //ProdDTO p = new ProdDTO((string)doc["Product_Name"].ToString(), (string)doc["Product_Title"].ToString(), (string)doc["Price"].ToString(), (string)doc["id"].ToString());
                Product prod = new Product()
                {
                    ProductName = doc["Product_Name"].ToString(),
                    ProductTitle = doc["Product_Title"].ToString(),
                    UnitPrice = (int)doc["Price"],
                    ProductId = doc["id"].ToString(),
                    // = doc["Product_Category_Name"].ToString(),
                };
                try
                {
                    prod.ProductDescription = doc["Product_Description"].ToString();
                }
                catch (Exception e)
                {
                    prod.ProductDescription = "No description available";
                }
                return prod;
            }
        }
    }
}
