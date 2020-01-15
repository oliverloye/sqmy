using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Umbraco.Web.Models;
using UmbracoWebDev.Models.Solr;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Umbraco.Web.Mvc;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using WebControllers.Helpers;
using WebControllers.Controllers;
using System.Web;
using ClassLibrary.Entity;

namespace UmbracoWebDev.Controllers.SolrControllers
{
    public class SolrProductItemController : RenderMvcController
    {
        static readonly HttpClient client = new HttpClient();
        readonly FeatureController fc = new FeatureController();

        public async Task<ActionResult> SolrProductItem(RenderModel model, string id)
        {
            using (new TimeMeasure("SolrProductItem"))
            {
                try
                {
                    string decodeId = HttpUtility.UrlDecode(id);
                    string encodedId = HttpUtility.UrlEncode(decodeId);
                    var myProducts = new SolrProducts(model.Content);
                    myProducts.SingleProduct = await GetSolrProduct(encodedId);
                    return CurrentTemplate(myProducts);
                } catch
                {
                    return View("ProductNotFound");
                }
            }
        }

        public async Task<Product> GetSolrProduct(string query)
        {
            using (new TimeMeasure("GetSolrProduct"))
            {
                try
                {
                    HttpResponseMessage response;
                    if (Request.Url.AbsoluteUri.Contains("/butik/"))
                    {
                        response = await client.GetAsync(ConnectionHelper.GetDanishSolrConnection() + "select?q=id%3A" + HttpUtility.UrlEncode(query));
                    }
                    else
                    {
                        response = await client.GetAsync(ConnectionHelper.GetEnglishSolrConnection() + "select?q=id%3A" + HttpUtility.UrlEncode(query));
                    }
                    JObject responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                    JArray jProducts = (JArray)responseJson["response"]["docs"];
                    Product result = Converter(jProducts[0]);
                    //result.Features = fc.GetFeatureByProdId(result.Product_Id, ConnectionHelper.GetFeatureDbConnection());

                    return result;
                }
                catch (Exception)
                {
                    throw null;
                }
            }
        }


        private Product Converter(JToken doc)
        {
            using (new TimeMeasure("Converter - solrProductitemController"))
            {


                //ProdDTO p = new ProdDTO((string)doc["Product_Name"].ToString(), (string)doc["Product_Title"].ToString(), (string)doc["Price"].ToString(), (string)doc["id"].ToString());
                Product prod = new Product()
                {
                    ProductName = doc["Product_Name"].ToString(),
                    ProductTitle = doc["Product_Title"].ToString(),
                    UnitPrice = (int)doc["Price"],
                    ProductId = doc["id"].ToString(),
                    //Product_Category_Name = doc["Product_Category_Name"].ToString()
                };
                try
                {
                    prod.ProductDescription = doc["Product_Description"].ToString();
                }
                catch (Exception)
                {
                    prod.ProductDescription = "No description available";
                }
                return prod;
            }
        }
    }
}

