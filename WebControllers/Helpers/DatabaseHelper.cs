using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebControllers.Helpers
{
    public class DatabaseHelper
    {
        public bool IsCartEmpty()
        {
            if (HttpContext.Current.Session["Cart"] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsOrderInDataBase(int order_id)
        {

            string sql = "SELECT Order_ID FROM Order_Table WHERE Order_ID = @Order_ID";

            using (SqlConnection connection = new SqlConnection(ConnectionHelper.GetEnglishConnectionString()))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Order_ID", order_id);
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

        public bool IsProductInEnglishDataBase(string prod_id)
        {
            string sql = "SELECT Product_ID FROM PProduct_Table WHERE Product_ID = @Product_ID";

            using (SqlConnection connection = new SqlConnection(ConnectionHelper.GetEnglishConnectionString()))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Product_ID", prod_id);
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

        public bool IsProductInDansihDataBase(string prod_id)
        {
            string sql = "SELECT Product_ID FROM PProduct_Table WHERE Product_ID = @Product_ID";

            using (SqlConnection connection = new SqlConnection(ConnectionHelper.GetDanishConnectionString()))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Product_ID", prod_id);
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