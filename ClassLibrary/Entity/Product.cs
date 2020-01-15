using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Entity
{
    [Table("Product")]
    public class Product
    {
        [Key, ScaffoldColumn(false)]
        public string ProductId { get; set; }

        [Required, StringLength(600), Display(Name = "Title")]
        public string ProductTitle { get; set; }
        [Required, StringLength(200), Display(Name = "Name")]
        public string ProductName { get; set; }

        [StringLength(int.MaxValue), Display(Name = "Product Description"), DataType(DataType.MultilineText)]
        public string ProductDescription { get; set; }

        [Display(Name = "Price")]
        public double? UnitPrice { get; set; }

        public int CategoryId { get; set; }
        public string ThumbPicUrl { get; set; }
        public string HighPicUrl { get; set; }
        public string LowPicUrl { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Category Category { get; set; }
        public virtual ProductImage ProductImage { get; set; }
    }
}

