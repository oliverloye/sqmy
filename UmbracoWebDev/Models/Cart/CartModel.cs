using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UmbracoWebDev.Models.Cart
{
    public class CartModel
    {
        [Required]
        public int Product_Price { get; set; }
        [Required]
        public string Product_Name { get; set; }
        [Required]
        public string Product_Id { get; set; }
        [Required]
        public int Product_Quantity { get; set; }
        [Required]
        public int Cart_ID { get; set; }
        [Required]
        public int User_ID { get; set; }



        public CartModel()
        {

        }

        public CartModel(int price, string name, string id,int quantity)
        {
            Product_Price = price;
            Product_Name = name;
            Product_Id = id;
            Product_Quantity = quantity;
        }

        public CartModel(int price, string name, string id)
        {
            Product_Price = price;
            Product_Name = name;
            Product_Id = id;
        }

        public CartModel( int user_id, string id)
        {
            
            User_ID = user_id;
            Product_Id = id;
            
        }

    }
}