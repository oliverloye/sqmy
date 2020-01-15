using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using ClassLibrary.Controllers;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using ClassLibrary.Entity;
using Importer.Models;

namespace Importer.Facade
{
    class UrlController
    {
        ImageController imgFacade = new ImageController();
        FeatureController featureFacade = new FeatureController();
        ProductController productFacade = new ProductController();

        public void FetchProductsByUrl(string productUrl)
        {
            using (new TimeMeasure("UrlFacade - Read New Product Urls"))
            {
                try
                {
                    Product productToHandle = productFacade.ReadProductXml(productUrl);
                    if (!productFacade.CheckProductExist(productToHandle.ProductId))
                    {
                        productFacade.PostProductEntity(productToHandle);
                        PostUrl(new ProductUrl(productUrl, 1));
                    }
                    else
                    {
                        productFacade.PostProductEntity(productToHandle);
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        public void ReadIndexFileEntity(string filePath)
        {
            using (var _dbcontext = new DatabaseContext())
            {
                var url = "http://data.icecat.biz/"; //The Root URL for the Product
                WebResponse response = null;
                Uri myDailyUri = new Uri(filePath, UriKind.Absolute);
                WebRequest dailyRequest = WebRequest.Create(myDailyUri);
                dailyRequest.Credentials = new NetworkCredential("alphaslo", "KJ6j1c9y8c2YwMq8GTjc"); //Login credentials for IceCat
                XmlReaderSettings dailySettings = new XmlReaderSettings();
                dailySettings.XmlResolver = null;
                dailySettings.DtdProcessing = DtdProcessing.Parse;
                using (response = dailyRequest.GetResponse())
                {
                    using (XmlReader reader = XmlReader.Create(response.GetResponseStream(), dailySettings))
                    {
                        while (reader.ReadToFollowing("file"))
                        {
                            //Extract the value of the Name attribute
                            if (reader.GetAttribute("Supplier_id") == "5") //Runs through the XML file and picks out all products with the Supplier_Id 5 (5 = sony)
                            {
                                var urlToPost = url + reader.GetAttribute("path");

                                if (!UrlExistInDatabase(url + reader.GetAttribute("path")))
                                {
                                    PostUrl(new ProductUrl(urlToPost, 0));
                                    Debug.WriteLine("Added Url: " + urlToPost);
                                }
                                else
                                {
                                    PostUrl(new ProductUrl(urlToPost, 0));
                                    Debug.WriteLine("Updated Url: " + urlToPost);
                                }
                            }
                        }
                    }

                }
                response.Close();
            }
        }

        public bool UrlExistInDatabase(string url)
        {
            using (new TimeMeasure("Check if URL exist in database"))
            {
                using (var _dbcontext = new DatabaseContext())
                {
                    var query = _dbcontext.ProductUrls
                           .Where(s => s.Url == url)
                           .FirstOrDefault<ProductUrl>();
                    return query != null ? true : false;
                }
            }
        }

        public void PostUrl(ProductUrl urlProduct) //Post the URL into the PUrl_table
        {
            using (new TimeMeasure("PostProduct - URL"))
            {
                using (var _dbcontext = new DatabaseContext())
                {
                    _dbcontext.ProductUrls.AddOrUpdate(urlProduct);
                    _dbcontext.SaveChanges();
                    //Debug.WriteLine("Added Url: " + urlProduct.Url);
                }
            }
        }

        public List<string> GetUrlsToHandle() //Gets all URLS with the flag 0.
        {
            using (new TimeMeasure("GetTimeoutUrls"))
            {
                using (var _dbcontext = new DatabaseContext())
                {
                    var result = new List<string>();
                    var query = _dbcontext.ProductUrls
                           .Where(p => p.Flag == 0);

                    foreach (var item in query)
                    {
                        result.Add(item.Url);
                    }
                    return result;
                }
            }
        }
    }
}
