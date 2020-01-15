using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using UmbracoWebDev.Models.Cart;
using WebControllers.Helpers;
using WebControllers.Models.Order;
using ClassLibrary.Entity;
using Importer.Facade.Logic;
using System.Collections.Generic;
using Importer.Models;
using Importer.Facade;
using ClassLibrary.DTOs;
using System.Linq;
using System.Diagnostics;
using PayPal.Api;
using UmbracoWebDev.Models.Paypal;
using System.Web.Security;

namespace UmbracoWebDev.Controllers.Cart
{
    public class CartController : SurfaceController
    {
        ShoppingCartFacade shoppingCartFacade = new ShoppingCartFacade();
        OrderFacade orderFacade = new OrderFacade();
        ProductController productFacade = new ProductController();
        CartContentController cartContentController = new CartContentController();

        [ChildActionOnly]
        public ActionResult RenderAddToCart(CartItem model)
        {
            return PartialView("~/Views/Partials/Cart/_AddToCartNew.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(CartItem cartItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    shoppingCartFacade.AddToCart(cartItem.ProductId, cartItem.Quantity);
                    TempData.Add("AddedToCart", true);
                    return RedirectToCurrentUmbracoPage("?id=" + cartItem.ProductId);
                }

                TempData.Add("AddedToCart", false);
                return RedirectToCurrentUmbracoPage("?id=" + cartItem.ProductId);
            }
            catch (Exception)
            {
                TempData.Add("AddedToCart", false);
                return RedirectToCurrentUmbracoPage("?id=" + cartItem.ProductId);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessAndSaveOrder([Bind(Prefix = "orderInfo")] CheckoutOrderDTO orderInfo)
        {
            if (orderInfo.PaymentMethod.ToLower() == "paypal")
            {
                //Remove the Credit Card form the modelstate, as we don't need it.
                foreach (var key in ModelState.Keys.Where(m => m.StartsWith("orderInfo.Card")).ToList())
                    ModelState.Remove(key);

                if (ModelState.IsValid)
                {
                    var checkoutPage = Umbraco.Content(1199);
                    TempData.Add("orderInfoTemp", orderInfo);
                    return Redirect(checkoutPage.Url + "/processpayment");
                }
            }
            if (orderInfo.PaymentMethod.ToLower() == "card")
            {
                if (ModelState.IsValid)
                {
                    if (orderInfo.Card != null)
                    {
                        return cartContentController.PayWithCard(orderInfo);
                    }
                }
            }
            return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClearCart()
        {
            if (ModelState.IsValid)
            {
                //CartSession.ClearCart();
                shoppingCartFacade.EmptyCart();
                TempData["AddedSuccess"] = true;
                return RedirectToCurrentUmbracoPage();
            }
            return RedirectToCurrentUmbracoPage();
        }

        public List<CartItem> GetWholeCartItems()
        {
            return shoppingCartFacade.GetCartItems();
        }

        public int GetCartCount()
        {
            return shoppingCartFacade.GetCount();
        }
    }
}
