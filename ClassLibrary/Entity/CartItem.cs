using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entity
{
    public class CartItem
    {
        [Key]
        public string ItemId { get; set; }

        public string CartId { get; set; }

        public int Quantity { get; set; }

        public DateTime DateCreated { get; set; }

        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public CartItem()
        {
        }

        public CartItem(string productId, int quantity)
        {
            ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
            Quantity = quantity;
        }
    }
}
