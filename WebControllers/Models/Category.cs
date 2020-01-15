using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebControllers.Controllers;
using WebControllers.Helpers;

namespace WebControllers.Models
{
    public class Category
    {
        CustomStringCleaner cc = new CustomStringCleaner();

        public int Category_ID { get; set; }

        public string Category_Name { get; set; }

        public string Category_imgUrl { get; set; }


        public Category()
        {

        }

        public Category(int category_id, string category_name)
        {
            Category_ID = category_id;
            Category_Name = cc.CleanString(category_name);
        }


        public Category(int category_id, string category_imgUrl, string category_name)
        {
            Category_ID = category_id;
            Category_Name = cc.CleanString(category_name);
            Category_imgUrl = category_imgUrl;
        }

        public override string ToString()
        {
            return "Category_ID: " + Category_ID
                + " Category_Name: " + Category_Name;
        }
    }
}
