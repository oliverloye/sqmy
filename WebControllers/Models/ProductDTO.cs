using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebControllers.Helpers;

namespace WebControllers.Models
{
    public class ProductDTO
    {
        CustomStringCleaner cc = new CustomStringCleaner();

        //General Properties
        public string Product_Name { get; set; }
        public string Product_Title { get; set; }
        public int Product_Price { get; set; }
        public string Product_Id { get; set; }
        public string Product_Category_Name { get; set; }
        public string Product_Description { get; set; }
        public int Product_MaxQuantity { get; set; }
        public int Product_SelectedQuantity { get; set; }


        //Image Properteis
        public string Product_ThumbPic { get; set; }
        public string Product_HighPic { get; set; }
        public string Product_LowPic { get; set; }

        //Feature Properties
        public List<FeatureDTO> Features { get; set; }

        public ProductDTO()
        {

        }

        public ProductDTO(string productName, string productTitle, int productPrice, string productId, string productCat, string prodDesc, List<FeatureDTO> features)
        {
            Product_Name = cc.CleanString(productName);
            Product_Id = productId;
            Product_Title = cc.CleanString(productTitle);
            Product_Price = productPrice;
            Product_Category_Name = cc.CleanString(productCat);
            Product_Description = cc.CleanString(prodDesc);
            Features = features;
        }


        public ProductDTO(string productName, string productTitle, int productPrice, string productId, string productCat, string prodDesc)
        {
            Product_Name = cc.CleanString(productName);
            Product_Id = productId;
            Product_Title = cc.CleanString(productTitle);
            Product_Price = productPrice;
            Product_Category_Name = cc.CleanString(productCat);
            Product_Description = cc.CleanString(prodDesc);
        }

        public override string ToString()
        {
            return "["+Product_Id+"] - " + Product_Title;
        }
    }
}