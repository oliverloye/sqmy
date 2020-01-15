using AlphaSolutions.Core.Diagnostics.TimeMeasures;
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
    public class UrlControllers
    {
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
                    Debug.WriteLine("Added Url: " + urlProduct.Url);
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
