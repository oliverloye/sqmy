using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebControllers.Models.Order
{
    public class OrderlineDTO
    {
        public int OrderLIne_ID { get; set; }
        public int fkOrder_ID { get; set; }
        public string fkProduct_ID { get; set; }
        public int Product_Amount { get; set; }
        public int Product_Price { get; set; }
            
        public OrderlineDTO(string fkProduct_ID, int product_Amount, int product_Price)
        {
            this.fkProduct_ID = fkProduct_ID;
            Product_Amount = product_Amount;
            Product_Price = product_Price;
        }

        public OrderlineDTO(int fkORder_ID, string fkProduct_ID, int product_Amount, int product_Price)
        {
            this.fkOrder_ID = fkORder_ID;
            this.fkProduct_ID = fkProduct_ID;
            Product_Amount = product_Amount;
            Product_Price = product_Price;
        }


    }
}