using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Xml;
using WebControllers.Controllers;
using WebControllers.Models;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using System.Web;
using WebControllers.Helpers;
using Importer.Facade;

namespace ClassLibrary.Solr
{
    public class SolrIndexer
    {
        List<ProductDTO> ProductList = new List<ProductDTO>();
        List<ClassLibrary.DTOs.ProductDTO> ProdListNew = new List<DTOs.ProductDTO>();
        ProductsController Controller = new ProductsController();
        SolrController solrFacade = new SolrController();
        XmlDocument Result = new XmlDocument();

        


        public XmlDocument MakeProductToXML(DTOs.ProductDTO p)
        {
            using (new TimeMeasure("MakeProductToXML - solrIndexer"))
            {

                XmlDocument doc = new XmlDocument();
                string product_ID = "<field name=\"id\">" + SecurityElement.Escape(p.ProductId) + "</field>";
                string product_Title = "<field name=\"Product_Title\">" + SecurityElement.Escape(p.ProductTitle) + "</field>";
                string product_Name = "<field name=\"Product_Name\">" + SecurityElement.Escape(p.ProductName) + "</field>";
                string product_Description = "<field name=\"Product_Description\">" + SecurityElement.Escape(p.ProductDescription) + "</field>";
                string price = "<field name=\"Price\">" + p.UnitPrice + "</field>";
                string product_Category_Name = "<field name=\"Product_Category_Name\">" + SecurityElement.Escape(p.ProductCategoryName) + "</field>";
                doc.LoadXml("<add><doc>" + product_ID + product_Title + product_Name + price + product_Category_Name + product_Description + "</doc></add>");
                //Console.WriteLine(doc.InnerXml);


                doc.LoadXml("<add><doc>" + product_ID + product_Title + product_Name + price + product_Category_Name + product_Description + "</doc></add>");
                //Console.WriteLine(doc.InnerXml);

                return doc;
            }
        }

        private void Indexer(XmlDocument doc)
        {
            using (new TimeMeasure("Indexer - solrIndexer"))
            {
                HttpClient client = new HttpClient();
                string url =  ConnectionHelper.GetEnglishSolrConnection()  + "update";
                var xmlContent = new StringContent(doc.InnerXml, Encoding.UTF8, "text/xml");
                client.PostAsync(url, xmlContent).Wait();
                client.GetAsync(url + "?commit=true").Wait();
            }

        }

        private void IndexerDanish(XmlDocument doc)
        {
            using (new TimeMeasure("Indexer Danish - solrIndexer"))
            {
                HttpClient client = new HttpClient();
                string url = ConnectionHelper.GetDanishSolrConnection() + "update";
                var xmlContent = new StringContent(doc.InnerXml, Encoding.UTF8, "text/xml");
                client.PostAsync(url, xmlContent).Wait();
                client.GetAsync(url + "?commit=true").Wait();
            }

        }

        //Denne burde virke med den nye db
        public void RunIndexing()
        {
            using (new TimeMeasure("RunIndexing"))
            {

                int count = 1;
                var plist = new List<DTOs.ProductDTO>();
                plist = solrFacade.GetSolrProducts().ToList();
                //ProdListNew = solrFacade.GetSolrProducts().ToList();
                foreach (var item in plist)
                {

                    Result = MakeProductToXML(item);
                    System.Diagnostics.Debug.WriteLine("{0} : indexing:\t{1}", count, item.ProductId +  " > " + item.ProductTitle);
                    count++;
                    Indexer(Result);
                }
            }
        }
        //public void RunIndexingDanish(string connectionString)
        //{
        //    using (new TimeMeasure("RunIndexing Danish"))
        //    {

        //        int count = 1;
        //        ProductList = Controller.GetSolrProducts(connectionString).ToList();
        //        foreach (var item in ProductList)
        //        {

        //            Result = MakeProductToXML(item);
        //            System.Diagnostics.Debug.WriteLine("{0} : indexing:\t{1}", count, item);
        //            count++;
        //            IndexerDanish(Result);
        //        }
        //    }
        //}
    }
}
