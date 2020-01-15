using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebControllers.Models
{
    public class ProdImageInfo
    {
        public string Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }

        public string HighPic_Image { get; set; }

        public ProdImageInfo() { }

        public ProdImageInfo(string id, string pname, string pdesc, byte[] img)
        {
            Product_ID = id;
            Product_Name = pname;
            Product_Description = pdesc;
            HighPic_Image = "data:image/jpg;base64," + Convert.ToBase64String(img);
        }

        
    }
}