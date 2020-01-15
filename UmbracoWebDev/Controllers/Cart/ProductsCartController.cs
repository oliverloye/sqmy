using System.Data.SqlClient;
using System.Web.Http;
using WebControllers.Models;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using WebControllers.Models.Order;
using WebControllers.Controllers;
using System;
using System.Text;
using System.Web;

namespace UmbracoWebDev.Controllers.Cart
{
    public class ProductsCartController : ApiController
    {


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
    }
}