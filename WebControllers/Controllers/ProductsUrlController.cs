using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebControllers.Models;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;

namespace WebControllers.Controllers
{
    public class ProductsUrlController : ApiController
    {
        //private string connectionString = "server=PRAKTIKANTSQL01\\MSSQLSERVER2017;database=icecat_db;user id = umbraco2; password='umbraco2'";

        // GET: api/ProductsUrl
        [HttpGet]
        public List<string> GetUrlsToHandle(string connectionString) //Gets all URLS with the flag 0.
        {
            using (new TimeMeasure("GetTimeoutUrls"))
            {
                var result = new List<string>();
                string sql = "select Url from PUrl_Table WHERE Flag=0";
                using (SqlConnection databaseConnection = new SqlConnection(connectionString))
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
                                    string url = reader.GetString(0);

                                    result.Add(url);
                                }
                            }
                        }
                    }
                    databaseConnection.Close();
                }

                return result;
            }

        }
        // POST: api/ProductsUrl
        [HttpPost]
        public void PostUrl([FromBody] UrlProduct urlProduct, string connectionString) //Post the URL into the PUrl_table
        {
            using (new TimeMeasure("PostProduct - URL"))
            {

                string query = "INSERT into PUrl_Table(Url, Flag) VALUES (@Url, @Flag)";

                SqlConnection connection = new SqlConnection(connectionString);
                using (SqlCommand insertCommand = new SqlCommand(query, connection))
                {
                    connection.Open();
                    insertCommand.Parameters.AddWithValue("@Url", urlProduct.Url);
                    insertCommand.Parameters.AddWithValue("@Flag", urlProduct.Flag);
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }

        // PUT: api/ProductsUrl/5
        [HttpPut]
        public void PutUrl(string url, int flag, string connectionString) //Update the URL from flag 0 to 1.
        {
            using (new TimeMeasure("PutProductUrl"))
            {

                string sql = "UPDATE PUrl_Table SET Flag = @flag WHERE Url = @url";

                SqlConnection connect = new SqlConnection(connectionString);

                using (SqlCommand insertCommand = new SqlCommand(sql, connect))
                {
                    insertCommand.Parameters.AddWithValue("@flag", flag);
                    insertCommand.Parameters.AddWithValue("@url", url);
                    connect.Open();
                    insertCommand.ExecuteNonQuery();
                    connect.Close();
                }
            }

        }

        [HttpGet]
        public bool UrlExistInDatabase(string url, string connectionString)
        {
            using (new TimeMeasure("Check if URL exist in database"))
            {
                string query = "SELECT Url FROM PUrl_Table WHERE Url = '" + url + "'";
                using (SqlConnection databaseConnection = new SqlConnection(connectionString))
                {
                    databaseConnection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(query, databaseConnection))
                    {
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                return true;
                            }
                            else return false;
                        }
                    }
                }
            }
        }

        [HttpGet]
        public List<UrlProduct> GetAllUrls(string connectionString) //Get all URLS in the PURL table
        {
            using (new TimeMeasure("GetAllUrlProducts"))
            {
                var result = new List<UrlProduct>();
                string sql = "select * from PUrl_Table";
                using (SqlConnection databaseConnection = new SqlConnection(connectionString))
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
                                    string url = reader.GetString(0);
                                    int flag = reader.GetInt32(1);

                                    UrlProduct urlProduct = new UrlProduct()
                                    {
                                        Url = url,
                                        Flag = flag
                                    };

                                    result.Add(urlProduct);
                                }
                            }
                        }
                    }
                    databaseConnection.Close();
                }

                return result;
            }
        }
    }
}
