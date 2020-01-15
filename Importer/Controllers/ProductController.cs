using ClassLibrary.Entity;
using Importer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Controllers
{
    public class ProductController
    {
        public void PostProductEntity(Product product) //Post the prodcut the the PProduct_Table 
        {
            using (var _dbcontext = new DatabaseContext())
            {
                _dbcontext.Products.AddOrUpdate(product);
                _dbcontext.SaveChanges();
                string prodStr = string.Format("Added Product: {0} > {1}", product.ProductId, product.ProductTitle);
                Debug.WriteLine(prodStr);
            }
        }
    }
}
