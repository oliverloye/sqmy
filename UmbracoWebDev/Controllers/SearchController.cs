using Umbraco.Web.Mvc;
using UmbracoWebDev.Models;
using System.Web.Http;
using System.Web.Mvc;

namespace UmbracoWebDev.Controllers
{
    public class SearchController : SurfaceController
    {
        public ActionResult RenderSearchField(SearchModel model)
        {
            return PartialView("~/Views/Partials/SearchView/_SearchPage.cshtml", model);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Submit(SearchModel model)
        {
            if (ModelState.IsValid)
            {
                return Redirect("/shop/?query=Product_Title%3A"+model.Query+"OR Product_Description%3A"+model.Query+"%26rows=30%26start=0");
            }

            return Redirect("/shop/");
        }
    }
}
