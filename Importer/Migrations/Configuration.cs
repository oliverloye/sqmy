namespace Importer.Migrations
{
    using ClassLibrary.Entity;
    using Importer.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DatabaseContext _dbcontext)
        {
            //GetCategories().ForEach(c => context.Categories.AddOrUpdate(c));
            //GetProducts().ForEach(p => context.Products.AddOrUpdate(p));
        }

        private static List<Category> GetCategories()
        {
            var categories = new List<Category> {
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Cars"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Planes"
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Trucks"
                },
                new Category
                {
                    CategoryId = 4,
                    CategoryName = "Boats"
                },
                new Category
                {
                    CategoryId = 5,
                    CategoryName = "Rockets"
                },
            };

            return categories;
        }

        private static List<Product> GetProducts()
        {
            var products = new List<Product> {
                new Product
                {
                    ProductId = "1-A",
                    ProductTitle = "A red convertible car with 460HP",
                    ProductName = "Convertible Car",
                    ProductDescription = "This convertible car is fast! The engine is powered by a neutrino based battery (not included)." +
                                  "Power it up and let it go!",
                    UnitPrice = 22.50,
                    CategoryId = 1
               },
                new Product
                {
                    ProductId = "2-B",
                    ProductTitle = "And old shit car mate",
                    ProductName = "Old-time Car",
                    ProductDescription = "There's nothing old about this toy car, except it's looks. Compatible with other old toy cars.",
                    UnitPrice = 15.95,
                     CategoryId = 1
               },
                new Product
                {
                    ProductId = "3-C",
                    ProductName = "Fast Car",
                    ProductTitle = "Pretty fast tbh",
                    ProductDescription = "Yes this car is fast, but it also floats in water.",
                    UnitPrice = 32.99,
                    CategoryId = 1
                },
            };

            return products;
        }
    }
}
