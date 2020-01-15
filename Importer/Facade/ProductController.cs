using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using ClassLibrary.Entity;
using Importer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Xml;

namespace Importer.Facade
{
    public class ProductController
    {
        //Method returns the Product object after reading xml file
        public Product ReadProductXml(string productUrl)
        {
            using (new TimeMeasure("ProductFacade - Get Product With Url"))
            {
                string id = "";
                int catId = 0;
                string title = "";
                string name = "";
                string highpic = "";
                string lowpic = "";
                string thumbpic = "";
                string description = "";

                WebResponse response = null;
                Uri myUri = new Uri(productUrl, UriKind.Absolute);
                WebRequest request = WebRequest.Create(myUri);
                request.Credentials = new NetworkCredential("alphaslo", "KJ6j1c9y8c2YwMq8GTjc"); //Login credentials for IceCat
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.XmlResolver = null;
                settings.DtdProcessing = DtdProcessing.Parse;
                using (response = request.GetResponse())
                {
                    using (XmlReader reader = XmlReader.Create(response.GetResponseStream(), settings))
                    {

                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                //return only when you have START tag  
                                switch (reader.Name.ToString())
                                {
                                    //Runs through the XML file and looks for the nodes "Product" - "ProductDecription" - "Catagory"
                                    //Then adds the respective data to the variable
                                    case "Product":
                                        if (id == "")
                                        {
                                            id = reader.GetAttribute("Prod_id");
                                            title = reader.GetAttribute("Title");
                                            name = reader.GetAttribute("Name");
                                            highpic = reader.GetAttribute("HighPic");
                                            lowpic = reader.GetAttribute("LowPic");
                                            thumbpic = reader.GetAttribute("ThumbPic");
                                        }
                                        break;
                                    case "ProductDescription":
                                        if (null == reader.GetAttribute("LongDesc"))
                                        {
                                            description = "";
                                        }
                                        else
                                        {
                                            description = reader.GetAttribute("LongDesc");
                                        }
                                        break;
                                    case "Category":
                                        if (catId == 0)
                                        {
                                            catId = Int32.Parse(reader.GetAttribute("ID"));
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                Random r = new Random();
                float price = r.Next(50, 1001);
                return new Product()
                {
                    ProductId = id,
                    ProductName = name,
                    ProductTitle = title,
                    UnitPrice = price,
                    ProductDescription = description,
                    CategoryId = catId,
                    LowPicUrl = lowpic,
                    HighPicUrl = highpic,
                    ThumbPicUrl = thumbpic
                };
            }
        }

        public void PostProductEntity(Product product) //Post the prodcut the the PProduct_Table 
        {
            using (var _dbcontext = new DatabaseContext())
            {
                try
                {
                    _dbcontext.Products.AddOrUpdate(product);
                    _dbcontext.SaveChanges();
                    string prodStr = string.Format("Added Product: {0} > {1}", product.ProductId, product.ProductTitle);
                    Debug.WriteLine(prodStr);
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine(@"Entity of type ""{0}"" in state ""{1}"" 
                   has the following validation errors:",
                            eve.Entry.Entity.GetType().Name,
                            eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine(@"- Property: ""{0}"", Error: ""{1}""",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch (DbUpdateException e)
                {
                    Debug.WriteLine(e);
                    throw e;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public bool CheckProductExist(string productId)
        {
            using (new TimeMeasure("Check if Product exist in database"))
            {
                using (var _dbcontext = new DatabaseContext())
                {
                    var query = _dbcontext.Products
                           .Where(p => p.ProductId == productId)
                           .FirstOrDefault<Product>();
                    return query != null ? true : false;
                }
            }
        }

        public List<Product> GetAllProducts()
        {
            using (var _dbcontext = new DatabaseContext())
            {
                return _dbcontext.Products.ToList();
            }
        }

        public Product GetProductWithImage(string id)
        {
            Product product = new Product();
            using (var _dbcontext = new DatabaseContext())
            {
                product = _dbcontext.Products.Find(id);
                product.ProductImage = _dbcontext.ProductImages.Find(id);
            }
            return product;
        }

        public double GetProductPriceById(string productId)
        {
            using (var _dbcontext = new DatabaseContext())
            {
                var prodPrice = _dbcontext.Products
                    .Where(x => x.ProductId == productId)
                    .Select(x => x.UnitPrice)
                    .SingleOrDefault();

                return (double)prodPrice;
            }
        }
    }
}
