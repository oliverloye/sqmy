using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebControllers.Models
{
    public class ProdImage
    {
        public string Product_Id { get; set; }
        public string HighPic_Image { get; set; }
        public string LowPic_Image { get; set; }

        public string ThumbPic_Image { get; set; }

        public ProdImage()
        {
        }

        public ProdImage(byte[] hPic, byte[] lPic, byte[] tPic)
        {
            HighPic_Image = "data:image/jpg;base64," + Convert.ToBase64String(hPic);
            LowPic_Image = "data:image/jpg;base64," + Convert.ToBase64String(lPic);
            ThumbPic_Image = "data:image/jpg;base64," + Convert.ToBase64String(tPic);
        }

        public ProdImage(string id, byte[] hPic, byte[] lPic, byte[] tPic)
        {
            Product_Id = id;
            HighPic_Image = "data:image/jpg;base64," + Convert.ToBase64String(hPic);
            LowPic_Image = "data:image/jpg;base64," + Convert.ToBase64String(lPic);
            ThumbPic_Image = "data:image/jpg;base64," + Convert.ToBase64String(tPic);
        }
    }
}