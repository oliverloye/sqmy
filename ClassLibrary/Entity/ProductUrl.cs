using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entity
{
    [Table("ProductUrls")]
    public class ProductUrl
    {
        [Key]
        public string Url { get; set; }
        public int Flag { get; set; }

        public ProductUrl()
        {

        }
        public ProductUrl(string url, int flag)
        {
            Url = url ?? throw new ArgumentNullException(nameof(url));
            Flag = flag;
        }
    }
}
