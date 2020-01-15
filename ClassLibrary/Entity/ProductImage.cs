using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entity
{
    [Table("ProductImages")]
    public class ProductImage
    {
        [Key]
        public string ProductId { get; set; }
        public byte[] HighPic { get; set; }
        public byte[] LowPic { get; set; }
        public byte[] ThumbPic { get; set; }
        public virtual Product Product { get; set; }

 
    }
}
