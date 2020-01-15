using ClassLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace UmbracoWebDev.Models.Cart
{
    public class NewCartModel : RenderModel
    {

        public NewCartModel(IPublishedContent content) : base(content)
        { }

        public List<CartItem> cartItems { get; set; }
        public double TotalCartPrice { get; set; }        
    }
}