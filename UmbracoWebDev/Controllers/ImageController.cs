using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using System.Data.SqlClient;
using Umbraco.Web.Mvc;
using WebControllers.Helpers;
using WebControllers.Controllers;
using System.Data;

namespace UmbracoWebDev.Controllers
{
    public class ImageController : SurfaceController
    {
        private static string en_SqlConnection = ConnectionHelper.GetEnglishConnectionString();

        public void GetImage(string prodId, string pictureQuality)
        {
            using (new TimeMeasure("GetProductImage"))
            {
                string quality = "";

                if (pictureQuality.Equals("high") || pictureQuality.Equals("low") || pictureQuality.Equals("thumb"))
                {
                    switch (pictureQuality)
                    {
                        case "high":
                            quality = "HighPic_Image";
                            break;

                        case "low":
                            quality = "LowPic_Image";
                            break;

                        case "thumb":
                            quality = "ThumbPic_Image";
                            break;


                    }
                }
                else
                {
                    quality = "ThumbPic_Image";
                }


                string sql = "Select " + quality + " FROM dbo.PImage_Table where Product_ID = @Product_ID";

                byte[] res = null;

                if (prodId != null)
                {
                    using (SqlConnection databaseConnection = new SqlConnection(en_SqlConnection))
                    {
                        databaseConnection.Open();
                        using (SqlCommand selectCommand = new SqlCommand(sql, databaseConnection))
                        {
                            selectCommand.Parameters.Add("@Product_ID", SqlDbType.NVarChar);
                            selectCommand.Parameters["@Product_ID"].Value = prodId;
                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        if (reader.IsDBNull(0))
                                        {
                                            res = GetProductNotFoundImage();
                                        }
                                        else
                                        {
                                            res = (byte[])reader[0];
                                        }
                                    }
                                }
                                else
                                {
                                    res = GetProductNotFoundImage();
                                }
                            }
                        }
                        databaseConnection.Close();
                        Response.ContentType = "image/jpeg";
                        Response.BinaryWrite(res);
                    }
                } else
                {
                    res = GetProductNotFoundImage();
                }
            }
        }

        private static byte[] GetProductNotFoundImage()
        {
            ProductImgController p = new ProductImgController();
            return p.TurnPictureToByte("http://shop.localhost/Media/1030/product-image_not_available.png");
        }
    }
}
