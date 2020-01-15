using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Entity
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public double? UnitPrice { get; set; }

        public virtual Product Product { get; set; }

        //public int OrderDetailID { get; set; }
        //public int OrderID { get; set; }
        //public string ProductID { get; set; }
        //public int Quantity { get; set; }
        //public double PurchasePrice { get; set; }
        //public virtual Order Order { get; set; }
        //public virtual Product Product { get; set; }
    }
}