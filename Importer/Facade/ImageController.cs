using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using ClassLibrary.Entity;
using Importer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebControllers.Controllers;

namespace Importer.Facade
{
    class ImageController
    {

        //Saves image as blob into the Database
        public void PostImageAsBlob(string productId)
        {
            GetImagesByProductId(productId);
        }

        public byte[] TurnPictureToByte(string someUrl) //Turns the Picutre files into Byte data from the Image URLs
        {
            using (new TimeMeasure("TurnPictureToByte"))
            {
                byte[] imageBytes;
                using (var webClient = new WebClient())
                {
                    try
                    {
                        if (someUrl.Equals(null))
                        {
                            imageBytes = BitConverter.GetBytes(0);
                        }
                        else
                        {
                            imageBytes = webClient.DownloadData(someUrl);
                        }
                    }
                    catch (Exception)
                    {
                        imageBytes = null;
                    }
                }
                return imageBytes;
            }
        }


        public void GetImagesByProductId(string id) //Gets the image URLS using Prod_ID form the product and feeds them into the ImaegHndler with the prodcut ID
        {
            using (new TimeMeasure("GetImagesByProduct"))
            {
                try
                {
                    List<string> imageURL = new List<string>();
                    using (var _dbcontext = new DatabaseContext())
                    {
                        var q = _dbcontext.Products
                           .Where(p => p.ProductId == id)
                           .Select(p => new { p.ThumbPicUrl, p.HighPicUrl, p.LowPicUrl })
                           .FirstOrDefault();
                        imageURL.Add(q.HighPicUrl);
                        imageURL.Add(q.LowPicUrl);
                        imageURL.Add(q.ThumbPicUrl);
                    }
                    ImageHandler(imageURL, id); //Calls the imagehandler
                }
                catch (Exception)
                {
                    Console.WriteLine("Error in image controller");
                }
            }
        }

        public void ImageHandler(List<string> ImgUrls, string id) //Handles the process of turning urls into bytes and post them to the PImage_Table 
        {
            using (new TimeMeasure("imageHandler"))
            {
                if (CheckImgExist(id) != true)
                {
                    List<byte[]> ImagesToPost = new List<byte[]>();
                    byte[] HighPic = TurnPictureToByte(ImgUrls[0]);
                    byte[] LowPic = TurnPictureToByte(ImgUrls[1]);
                    byte[] ThumbPic = TurnPictureToByte(ImgUrls[2]);
                    ImagesToPost.Add(HighPic);
                    ImagesToPost.Add(LowPic);
                    ImagesToPost.Add(ThumbPic);
                    PostProductImg(ImagesToPost, id);
                }
                else return;
            }
        }

        public void PostProductImg(List<byte[]> imageBytes, string id) //Inserts the byted pictures into the PImage_table
        {


            using (new TimeMeasure("PostProductImg"))
            {
                using(var _dbcontext = new DatabaseContext())
                {
                    ProductImage prodImg = new ProductImage()
                    {
                        ProductId = id,
                        HighPic = imageBytes[0],
                        LowPic = imageBytes[1],
                        ThumbPic = imageBytes[2]
                    };

                    _dbcontext.ProductImages.AddOrUpdate(prodImg);
                    _dbcontext.SaveChanges();
                    Debug.WriteLine("Added Image to Product: " + id);
                }
            }
        }

        public bool CheckImgExist(string id)
        {
            using (new TimeMeasure("CheckIfImgExist"))
            {
                using (var _dbcontext = new DatabaseContext())
                {
                    var query = _dbcontext.ProductImages
                           .Where(s => s.ProductId == id)
                           .FirstOrDefault<ProductImage>();
                    return query != null ? true : false;
                }
            }
        }
    }
}
