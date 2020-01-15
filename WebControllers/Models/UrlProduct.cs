using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebControllers.Models
{
    public class UrlProduct
    {
        public string Url { get; set; }

        public int Flag { get; set; }

        public UrlProduct()
        {

        }

        public UrlProduct(string url, int flag)
        {
            Url = url;
            Flag = flag;
        }

        public override string ToString()
        {
            return "Product_Url: " + Url + " Product Flag : " + Flag;
        }


    }
}
