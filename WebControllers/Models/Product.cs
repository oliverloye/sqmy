using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebControllers.Helpers;

namespace WebControllers.Models
{
    public class Product
    {
        CustomStringCleaner cc = new CustomStringCleaner();

        public string Product_ID { get; set; }

        public string Product_Title { get; set; }

        public string Product_Name { get; set; }

        public string Product_HighPic { get; set; }
        public string Product_LowPic { get; set; }
        public string Product_ThumbPic { get; set; }
        public string Product_Description { get; set; }

        public int Category_ID { get; set; }
        public int Price { get; set; }

        public List<FeatureDTO> Features { get; set; }

        public Product()
        {
        }

        public Product(string product_id, string product_title, string product_name, string product_highpic, string product_lowpic,
                       string product_thumbpic, string product_description, int category_id, int price)
        {
            Product_ID = cc.CleanIdString(product_id);
            Product_Title = cc.CleanString(product_title);
            Product_Name = cc.CleanString(product_name);
            Product_HighPic = product_highpic;
            Product_LowPic = product_lowpic;
            Product_ThumbPic = product_thumbpic;
            Product_Description = cc.CleanString(product_description);
            Category_ID = category_id;
            Price = price;
     
    }

        public override string ToString()
        {
            return "Product_ID: " + Product_ID
                + " Product_Title: " + Product_Title
                + " Product_Name: " + Product_Name
                + " Product_HighPic: " + Product_HighPic
                + " Product_LowPIc: " + Product_LowPic
                + " Product_ThumbPic: " + Product_ThumbPic
                + " Product_Description: " + Product_Description
                + " Category_ID: " + Category_ID
                + " Price: " + Price;
        }
    }
}
