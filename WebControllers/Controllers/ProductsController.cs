using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using System.Web;
using WebControllers.Models;

namespace WebControllers.Controllers
{
    public class ProductsController : ApiController
    {
       
        public string GetProductNameByProductID(string product_ID, string connectionString)
        {
            string result = "";
            string sql = "Select Product_Name from PProduct_Table WHERE Product_ID = @Product_ID";

            using (SqlConnection databaseConnection = new SqlConnection(connectionString))
            {

                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(sql, databaseConnection))
                {

                    selectCommand.Parameters.AddWithValue("@Product_ID", product_ID);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = reader.GetString(0);
                            }
                        }
                    }
                }
                databaseConnection.Close();
            }

            return result;
        }


        // PUT: api/UpdateProduct/{id}
        [HttpPut]
        public void UpdateSameProduct([FromBody]Product product, string connectionString) //Update all fields in a product when needed
        {

            if (product.Product_ID.Equals("EDM5200"))
            {
                Console.WriteLine(product.ToString());
            }

            using(new TimeMeasure("UpdateSameProduct"))
            {
                string sql = "UPDATE PProduct_Table SET " +
                "Product_Title = @Product_Title, " +
                "Product_Name = @Product_Name, " +
                "Product_HighPic = @Product_HighPic, " +
                "Product_LowPic = @Product_LowPic, " +
                "Product_ThumbPic = @Product_ThumbPic, " +
                "Product_Description = @Product_Description, " +
                "Category_ID = @Category_ID, " +
                "Price = @Price " +
                "WHERE Product_ID = @Product_ID";

                ExecuteProductMethod(sql, product, connectionString); //Executes the Query

            }
        }

        // POST: api/Products
        [HttpPost]
        public void PostProduct([FromBody] Product product, string connectionString) //Post the prodcut the the PProduct_Table 
        {
 
            using (new TimeMeasure("PostProduct - Product"))
            {
                string sql = "INSERT into PProduct_Table (Product_ID, Product_Title, Product_Name, Product_HighPic, Product_LowPic, Product_ThumbPic, Product_Description, Category_ID, Price) " +
                "VALUES (@Product_ID, @Product_Title, @Product_Name, @Product_HighPic, @Product_LowPic, @Product_ThumbPic, @Product_Description, @Category_ID, @Price)";
                ExecuteProductMethod(sql, product, connectionString); //Executes the Query
            }

        }

        public void ExecuteProductMethod(string sqlQuery, Product product, string connectionString) //executes the sql (Mainly from ProstProdcut and UpdateProdcut)
        {
            using (new TimeMeasure("ExecuteProductMethod"))
            {

                SqlConnection connection = new SqlConnection(connectionString);
                using (SqlCommand insertCommand = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();


                    insertCommand.Parameters.AddWithValue("@Product_ID", product.Product_ID);
                    insertCommand.Parameters.AddWithValue("@Product_Title", product.Product_Title);
                    insertCommand.Parameters.AddWithValue("@Product_Name", product.Product_Name);
                    insertCommand.Parameters.AddWithValue("@Product_HighPic", product.Product_HighPic);
                    insertCommand.Parameters.AddWithValue("@Product_LowPic", product.Product_LowPic);
                    insertCommand.Parameters.AddWithValue("@Product_ThumbPic", product.Product_ThumbPic);
                    insertCommand.Parameters.AddWithValue("@Product_Description", product.Product_Description);
                    insertCommand.Parameters.AddWithValue("@Category_ID", product.Category_ID);
                    insertCommand.Parameters.AddWithValue("@Price", product.Price);
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        // GET: api/Products
        [HttpGet]
        public List<Product> GetProducts(string connectionString) //Get all prodcuts form the database and returns as an IEnumrebale (Sort of list)
        {
            
            using(new TimeMeasure("GetProducts"))
            {
                var result = new List<Product>();
                string sql = "select * from PProduct_Table";
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
                                    string product_ID = reader.GetString(0);
                                    string product_Title = reader.GetString(1);
                                    string product_Name = reader.GetString(2);
                                    string product_HighPic = reader.GetString(3);
                                    string product_LowPic = reader.GetString(4);
                                    string product_ThumbPic = reader.GetString(5);
                                    string product_Description = reader.GetString(6);
                                    int category_ID = reader.GetInt32(7);
                                    int price = reader.GetInt32(8);
                                    Product product = new Product()
                                    {
                                        Product_ID = product_ID,
                                        Product_Title = product_Title,
                                        Product_Name = product_Name,
                                        Product_HighPic = product_HighPic,

                                        Product_LowPic = product_LowPic,
                                        Product_ThumbPic = product_ThumbPic,
                                        Product_Description = product_Description,
                                        Category_ID = category_ID,
                                        Price = price

                                    };

                                    result.Add(product);
                                }
                            }
                        }
                    }
                    databaseConnection.Close();
                }

                return result;
            }
        }

        public bool CheckProductExist(string productId, string connectionString)
        {
            using (new TimeMeasure("Check if Product exist in database"))
            {
                string query = "SELECT * FROM PProduct_Table WHERE Product_ID = '" + productId + "'";
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

        // GET: api/Achievment/5
        [HttpGet]
        [Route("api/products/{id}")]
        public Product GetProductByID(string id, string connectionString) //Get a specific Product form the database searching on Prod_ID
        {
            using (new TimeMeasure("GetProductByID"))
            {
                string sql = "select * from PProduct_Table where Product_ID='" + id + "'";
                var returnProduct = new Product();
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
                                    string product_ID = reader.GetString(0);
                                    string product_Title = reader.GetString(1);
                                    string product_Name = reader.GetString(2);
                                    string product_HighPic = reader.GetString(3);
                                    string product_LowPic = reader.GetString(4);
                                    string product_ThumbPic = reader.GetString(5);
                                    string product_Description = reader.GetString(6);
                                    int category_ID = reader.GetInt32(7);
                                    int price = reader.GetInt32(8);
                                    Product product = new Product()
                                    {
                                        Product_ID = product_ID,
                                        Product_Title = product_Title,
                                        Product_Name = product_Name,
                                        Product_HighPic = product_HighPic,
                                        Product_LowPic = product_LowPic,
                                        Product_ThumbPic = product_ThumbPic,
                                        Product_Description = product_Description,
                                        Category_ID = category_ID,
                                        Price = price
                                    };


                                    returnProduct = product;
                                }
                            }
                        }
                    }
                    databaseConnection.Close();
                }

                return returnProduct;
            }
        }

        [HttpGet]
        [Route("api/solrproducts")]
        public IEnumerable<ProductDTO> GetSolrProducts(string connectionString)
        {
            using (new TimeMeasure("GetSolrProducts"))
            {
                var result = new List<ProductDTO>();
                string sql = "SELECT PProduct_Table.Product_Name, PProduct_Table.Product_Title, PProduct_Table.Price, PProduct_Table.Product_ID, Category_Table.Category_Name, Product_Description FROM PProduct_Table INNER JOIN Category_Table ON PProduct_Table.Category_ID = Category_Table.Category_ID;";
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

                                    string product_Name = reader.GetString(0);
                                    string product_Title = reader.GetString(1);
                                    int product_Price = reader.GetInt32(2);
                                    string product_ID = reader.GetString(3);
                                    string product_Category_Name = reader.GetString(4);
                                    string product_Description = reader.GetString(5);

                                    ProductDTO product = new ProductDTO(product_Name, product_Title, product_Price, product_ID, product_Category_Name, product_Description);
                                    result.Add(product);
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
