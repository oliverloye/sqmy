using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebControllers.Helpers;
using WebControllers.Models.Order;


namespace WebControllers.Controllers
{
    public class OrderController
    {
        public OrderDTO GetSingleOrderFromID(int order_id)
        {
            string sql = "SELECT Order_ID, Member_ID, Order_Status, CreatedOn FROM Order_Table WHERE Order_ID = @Order_ID";
            OrderDTO result = new OrderDTO(); 
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionHelper.GetEnglishConnectionString()))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(sql, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@Order_ID", order_id);
                    //selectCommand.Parameters.AddWithValue("@Member_ID", memeber_id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int member_id = reader.GetInt32(1);
                                string order_status = reader.GetString(2);
                                DateTime createdon = reader.GetDateTime(3);
                                result = new OrderDTO(order_id, member_id, order_status, createdon);
                            }
                        }
                    }
                }
                databaseConnection.Close();
                return result;
            }
        }

        public List<OrderlineDTO> GetListOfOrderLineDTOs(int order_id)
        {
            var result = new List<OrderlineDTO>();

            string sql = "SELECT fkProduct_ID, Product_Amount, Product_Price FROM Order_Table " +
                         "JOIN Orderline_Table ON Order_Table.Order_ID = Orderline_Table.fkOrder_ID " +
                         "WHERE Order_Table.Order_ID =" + order_id;
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionHelper.GetEnglishConnectionString()))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(sql, databaseConnection))
                {
                    //selectCommand.Parameters.AddWithValue("@Member_ID", memeber_id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string Prodcut_ID = reader.GetString(0);
                                int Product_Amount = reader.GetInt32(1);
                                int Product_Price = reader.GetInt32(2);
                                OrderlineDTO orderLine = new OrderlineDTO(Prodcut_ID, Product_Amount, Product_Price);
                                result.Add(orderLine);
                            }
                        }
                    }
                }
                databaseConnection.Close();
                return result;
            }
        }

        public void PostBillingAdress(BillingInfoDTO info, string connectionString)
        {

            string sql = "INSERT INTO Billinginfo_Table"
                         + "(fkOrder_ID, Email, Firstname, Lastname, Streetname, Zipcode, City, Country, Phone)"
                         + " VALUES (@fkOrder_ID, @Email, @Firstname, @Lastname, @Streetname, @Zipcode, @City, @Country, @Phone)";

            SqlConnection connection = new SqlConnection(connectionString);

            using (SqlCommand insertCommand = new SqlCommand(sql, connection))
            {
                connection.Open();
                insertCommand.Parameters.AddWithValue("@fkOrder_ID", info.fkOrder_ID);
                insertCommand.Parameters.AddWithValue("@Email", info.Email);
                insertCommand.Parameters.AddWithValue("@Firstname", info.Firstname);
                insertCommand.Parameters.AddWithValue("@Lastname", info.Lastname);
                insertCommand.Parameters.AddWithValue("@Streetname", info.Address);
                insertCommand.Parameters.AddWithValue("@Zipcode", info.Zipcode);
                insertCommand.Parameters.AddWithValue("@City", info.City);
                insertCommand.Parameters.AddWithValue("@Country", info.Country);
                insertCommand.Parameters.AddWithValue("@Phone", info.Phone);
                insertCommand.ExecuteNonQuery();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

            }

        }

        public BillingInfoDTO GetBillingInfoForOrder(int order_id)
        {
            string sql = "SELECT Email, Firstname, Lastname, Streetname, Zipcode, City, Country, Phone FROM Billinginfo_Table where fkOrder_ID = @fkOrder_ID";
            BillingInfoDTO result = new BillingInfoDTO();
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionHelper.GetEnglishConnectionString()))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(sql, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@fkOrder_ID", order_id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string email = reader.GetString(0);
                                string firstname = reader.GetString(1);
                                string lastname = reader.GetString(2);
                                string address = reader.GetString(3);
                                int zip = reader.GetInt32(4);
                                string city = reader.GetString(5);
                                string country = reader.GetString(6);
                                string phone = reader.GetString(7);
                                result = new BillingInfoDTO(firstname, lastname, address, zip, city, country, phone, email);
                            }
                        }
                    }
                }
                databaseConnection.Close();
                return result;
            }
        }

        [HttpPost]
        public int PostOrderAndReturnID(OrderDTO order, string connectionString)
        {
            int newOrderId;

            using (new TimeMeasure("PostCart"))
            {

                ProductsController PC = new ProductsController();

                string sql_order = "INSERT INTO Order_Table(Member_ID, Order_Status, CreatedOn, UpdatedOn) VALUES(@MemberId, @OrderStatus, @CreateDate, @UpdateDate) ;SELECT CAST(scope_identity() AS int)";

                SqlConnection connection = new SqlConnection(connectionString);

                using (SqlCommand insertCommand = new SqlCommand(sql_order, connection))
                {
                    connection.Open();

                    insertCommand.Parameters.AddWithValue("@MemberId", order.Member_ID);
                    insertCommand.Parameters.AddWithValue("@OrderStatus", order.Order_Status);
                    insertCommand.Parameters.AddWithValue("@CreateDate", System.DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@UpdateDate", System.DateTime.Now);

                    newOrderId = (int)insertCommand.ExecuteScalar();
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
            return newOrderId;
        }

        internal void PostOrderLine(OrderlineDTO orderLine, string connectionString)
        {
            using (new TimeMeasure("PostCart"))
            {

                ProductsController PC = new ProductsController();

                string sql_order = "INSERT INTO Orderline_Table(fkOrder_ID, fkProduct_ID, Product_Amount, Product_Price) VALUES (@fkOrder_ID, @fkProduct_ID, @Product_Amount, @Product_Price);";

                SqlConnection connection = new SqlConnection(connectionString);

                using (SqlCommand insertCommand = new SqlCommand(sql_order, connection))
                {
                    connection.Open();

                    insertCommand.Parameters.AddWithValue("@fkOrder_ID", orderLine.fkOrder_ID);
                    insertCommand.Parameters.AddWithValue("@fkProduct_ID", orderLine.fkProduct_ID);
                    insertCommand.Parameters.AddWithValue("@Product_Amount", orderLine.Product_Amount);
                    insertCommand.Parameters.AddWithValue("@Product_Price", orderLine.Product_Price);
                    insertCommand.ExecuteNonQuery();

                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        public List<OrderDTO> GetListOfOrderDTOs(int member_id)
        {
            string sql = "SELECT Order_ID, Order_Status, CreatedOn FROM Order_Table WHERE Member_ID =" + member_id;
            var result = new List<OrderDTO>();

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionHelper.GetEnglishConnectionString()))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(sql, databaseConnection))
                {
                    //selectCommand.Parameters.AddWithValue("@Member_ID", memeber_id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int order_id = reader.GetInt32(0);
                                string order_status = reader.GetString(1);
                                DateTime createdon = reader.GetDateTime(2);
                                OrderDTO order = new OrderDTO(order_id, order_status, createdon);
                                result.Add(order);
                            }
                        }
                    }
                }
                databaseConnection.Close();
                return result;
            }
        }

        public void RemoveFailedOrder(int order_id)
        {

            string sql = "Delete from OrderLine_Table WHERE fkOrder_ID = @Order_ID;" +
                         "Delete from Order_Table Where Order_ID = @Order_ID";

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionHelper.GetEnglishConnectionString()))
            {
                databaseConnection.Open();
                using (SqlCommand deletecommand = new SqlCommand(sql, databaseConnection))
                {
                    deletecommand.Parameters.AddWithValue("@Order_ID", order_id);
                    deletecommand.ExecuteNonQuery();
                }
                databaseConnection.Close();
            }
        }
    }
}