using Importer.Facade.Logic;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using UmbracoWebDev.Models.Account;

namespace UmbracoWebDev.Controllers.AccountControllers
{
    public class MemberController : SurfaceController
    {
        ShoppingCartFacade shoppingCartFacade = new ShoppingCartFacade();

        public ActionResult RenderLogin()
        {
            return PartialView("~/Views/Partials/Account/_Login.cshtml", new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateUmbracoFormRouteString]
        public ActionResult HandleLogin([Bind(Prefix = "loginModel")] LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Username, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    UrlHelper myHelper = new UrlHelper(HttpContext.Request.RequestContext);

                    //Migrate the card to the logged in user
                    shoppingCartFacade.MigrateCart(shoppingCartFacade.GetCartId(), model.Username);

                    if (myHelper.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToCurrentUmbracoPage(); //What page to redirect to on successfull login
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The username or password provided is incorrect.");
                }
            }
            return CurrentUmbracoPage();
        }

        public ActionResult SubmitLogout()
        {
            TempData.Clear();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToCurrentUmbracoPage();
        }
    }
}
