using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using ClassLibrary.DTOs;
using ClassLibrary.Entity;
using Importer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.Facade
{
    public class SolrController
    {
        //Returns a list of all products in the database as ProductDTO's 
        public IEnumerable<ProductDTO> GetSolrProducts()
        {
            var results = new List<ProductDTO>();
            try
            {
                using (var _dbcontext = new DatabaseContext())
                {
                    //Inner join query that joins the Product and Category table on the CategoryId
                    var query = (from p in _dbcontext.Products
                                 join c in _dbcontext.Categories
                                 on p.CategoryId equals c.CategoryId
                                 select new { c, p }).ToList();

                    foreach (var item in query)
                    {
                        ProductDTO prodDTO = new ProductDTO(item.p.ProductId, item.p.ProductTitle, item.p.ProductName, item.p.ProductDescription, (double)item.p.UnitPrice, item.c.CategoryName);
                        results.Add(prodDTO);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return results;
        }
    }
}
