using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using WebControllers.Helpers;

namespace WebControllers.Controllers
{
    public class ProductImgController : ApiController
    {
        private readonly string en_SqlConnection = ConnectionHelper.GetEnglishConnectionString();
        private readonly string dk_SqlConnection = ConnectionHelper.GetDanishConnectionString();
        public void imageHandler(List<string> ImgUrls, string id) //Handles the process of turning urls into bytes and post them to the PImage_Table 
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

        // GET: api/Products/{id]/img
        [HttpGet]
        [Route("api/products/img/{id}")]

        public void GetImagesByProductId(string id) //Gets the image URLS using Prod_ID form the product and feeds them into the ImaegHndler with the prodcut ID
        {
            using (new TimeMeasure("GetImagesByProduct"))
            {
                try
                {
                    List<string> imageURL = new List<string>();
                    string sql = "SELECT Product_HighPic, Product_LowPic, Product_ThumbPic FROM PProduct_Table WHERE Product_ID = '" + id + "';";
                    using (SqlConnection databaseConnection = new SqlConnection(en_SqlConnection))
                    {
                        databaseConnection.Open();
                        using (SqlCommand selectCommand = new SqlCommand(sql, databaseConnection))
                        {
                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        string HighPic = reader.GetString(0);
                                        string LowPic = reader.GetString(1);
                                        string ThumbPic = reader.GetString(2);

                                        imageURL.Add(HighPic);
                                        imageURL.Add(LowPic);
                                        imageURL.Add(ThumbPic);
                                    }
                                }
                            }
                        }
                        databaseConnection.Close();
                    }
                    imageHandler(imageURL, id); //Calls the imagehandler
                }
                catch (Exception)
                {
                    List<string> imageURL = new List<string>();
                    imageURL.Add(null);
                    imageURL.Add(null);
                    imageURL.Add(null);
                    imageHandler(imageURL, id);
                    Console.WriteLine("Catched error in image and post images as nulls");
                }

            }

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



        // POST: api/Products/img
        [HttpPost]
        public void PostProductImg(List<byte[]> imageBytes, string id) //Inserts the byted pictures into the PImage_table
        {


            using (new TimeMeasure("PostProductImg"))
            {
                string sql = "INSERT into PImage_Table (Product_ID, HighPic_Image, LowPic_Image, ThumbPic_Image) " +
                "VALUES (@Product_ID, @HighPic_Image, @LowPic_Image, @ThumbPic_Image)";


                SqlConnection connection = new SqlConnection(en_SqlConnection);
                using (SqlCommand insertCommand = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    insertCommand.Parameters.AddWithValue("@Product_ID", id);
                    insertCommand.Parameters.AddWithValue("@HighPic_Image", imageBytes[0]);
                    insertCommand.Parameters.AddWithValue("@LowPic_Image", imageBytes[1]);
                    insertCommand.Parameters.AddWithValue("@ThumbPic_Image", imageBytes[2]);
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public bool CheckImgExist(string id)
        {
            using (new TimeMeasure("CheckIfImgExist"))
            {
                using (SqlConnection connection = new SqlConnection(en_SqlConnection))
                {

                    string sql = "SELECT * FROM PImage_Table WHERE Product_ID=@id";
                    SqlCommand selectCommand = new SqlCommand(sql, connection);
                    selectCommand.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return true;

                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
    }
}
