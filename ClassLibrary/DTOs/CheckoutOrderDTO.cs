using ClassLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DTOs
{
    public class CheckoutOrderDTO
    {
        public Order Order { get; set; }
        public string PaymentMethod { get; set; }
        public PayPal.Api.CreditCard Card { get; set; }
    }
}
