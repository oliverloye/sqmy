using ClassLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace UmbracoWebDev.Models.Cart
{
    public class CheckoutModel : RenderModel
    {
        public CheckoutModel(IPublishedContent content) : base(content)
        { }
        public Order order { get; set; }
    }
}