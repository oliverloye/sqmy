using ClassLibrary.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ClassLibrary.DTOs
{
    public class ProductDTO
    {
        public string ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double? UnitPrice { get; set; }
        public string ProductCategoryName { get; set; }
        public List<ProductImage> ProductImages { get; set; }

        public ProductDTO()
        {
        }

        public ProductDTO(string productId, string productTitle, string productName, string productDescription, double unitPrice, string productCategoryName)
        {
            ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
            ProductTitle = productTitle ?? throw new ArgumentNullException(nameof(productTitle));
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            ProductDescription = productDescription ?? throw new ArgumentNullException(nameof(productDescription));
            UnitPrice = unitPrice;
            ProductCategoryName = productCategoryName ?? throw new ArgumentNullException(nameof(productCategoryName));
        }

        public ProductDTO(string productId, string productTitle, string productName, string productDescription, double unitPrice, string productCategoryName, List<ProductImage> productImages)
        {
            ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
            ProductTitle = productTitle ?? throw new ArgumentNullException(nameof(productTitle));
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            ProductDescription = productDescription ?? throw new ArgumentNullException(nameof(productDescription));
            UnitPrice = unitPrice;
            ProductCategoryName = productCategoryName ?? throw new ArgumentNullException(nameof(productCategoryName));
            ProductImages = productImages ?? throw new ArgumentNullException(nameof(productImages));
        }
    }
}
