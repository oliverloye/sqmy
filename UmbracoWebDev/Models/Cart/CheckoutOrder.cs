using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary.Entity;

namespace UmbracoWebDev.Models.Cart
{
    public class CheckoutOrder
    {
        public Order Order { get; set; }
        public string PaymentMethod { get; set; }
    }
}