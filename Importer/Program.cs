using ClassLibrary.Controllers;
using ClassLibrary.DTOs;
using ClassLibrary.Entity;
using ClassLibrary.Solr;
using Importer.Facade.Logic;
using Importer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebControllers.Controllers;
using WebControllers.Helpers;

namespace Importer.Facade
{


    public class Program
    {
        FeatureController featureFacade = new FeatureController();
        ImageController imgFacade = new ImageController();
        UrlController urlFacade = new UrlController();
        OrderFacade orderFacade = new OrderFacade();
        CategoryFacade categoryFacade = new CategoryFacade();
        ProductController productFacade = new ProductController();
        ImageController imageFacade = new ImageController();
        ShoppingCartFacade shoppingCartFacade = new ShoppingCartFacade();
        SolrController solrFacade = new SolrController();
        SolrIndexer solrIndexer = new SolrIndexer();

        public static void Main(string[] args)
        {
        }

        public void PopulateWholeDatabase()
        {
            FetchIndex(UrlHelper.IndexFileEnglish());
            FetchCategories();
            FetchProducts();
            FetchImages();
            FetchFeatures();
        }

        public void InitializeAndRunDatabase()
        {
            using (var _dbcontext = new DatabaseContext())
            {
                Console.WriteLine("Initializing...");
                _dbcontext.Database.Initialize(true);
                Console.WriteLine("Finished...");
            }
        }
        public void CreateTestOrder()
        {
            using (var _dbcontext = new DatabaseContext())
            {
                try
                {
                    Order od = new Order()
                    {
                        OrderDate = DateTime.Now,
                        Total = 123,
                        PaymentTransactionId = orderFacade.GeneratePaymentTransactionId(),
                        HasBeenShipped = false,
                        CustomerID = "1-ACB",
                        FirstName = "TestS",
                        LastName = "Name",
                        Address = "Street 123",
                        City = "England",
                        PostalCode = "1234",
                        Country = "Denmark",
                        Phone = "12345678",
                        Email = "test@mail.com",
                        OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail(){Quantity = 1, ProductId = "10CDQ80N", UnitPrice = 400},
                        new OrderDetail(){Quantity = 1, ProductId = "5MCR-156A", UnitPrice = 552},
                    },
                    };

                    _dbcontext.Orders.Add(od);
                    _dbcontext.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        public void FetchFeatures()
        {
            using (var _dbcontext = new DatabaseContext())
            {
                foreach (var item in _dbcontext.ProductUrls)
                {
                    Console.WriteLine("Fetching Features");
                    featureFacade.PostFeatures(item.Url);
                }
            }
        }

        //Add products to the database
        public void FetchProducts()
        {
            foreach (var url in urlFacade.GetUrlsToHandle())
            {
                urlFacade.FetchProductsByUrl(url);
            }
            Debug.WriteLine("-- Fetch Products FINISHED --");
        }

        public void FetchImages()
        {
            foreach (var product in productFacade.GetAllProducts())
            {
                imgFacade.PostImageAsBlob(product.ProductId);
            }
            Debug.WriteLine("-- Fetch Images FINISHED --");
        }
        public void FetchIndex(string url)
        {
            urlFacade.ReadIndexFileEntity(url);
            Debug.WriteLine("-- Fetch Index Urls FINISHED --");
        }
        public void FetchCategories()
        {
            categoryFacade.ReadCategoryXML();
            Debug.WriteLine("-- Fetch Categories FINISHED --");
        }
    }
}