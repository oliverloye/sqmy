using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebControllers.Controllers;
using WebControllers.Helpers;
using WebControllers.Models;

namespace UmbracoWebDev.Controllers.SolrControllers
{
    public class ProductFeatureController : SurfaceController
    {
        FeatureController fc = new FeatureController();

        public ActionResult GetFeatureForProduct(string product_id)
        {
            var result = fc.GetFeatureByProdId(product_id, ConnectionHelper.GetFeatureDbConnection());
            return PartialView(result);
        }
    }
}