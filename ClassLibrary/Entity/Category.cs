using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace ClassLibrary.Entity
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        //public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required, StringLength(100)]
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
