using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebControllers.Models.Order
{
    public class BillingInfoDTO
    {
        public int Address_ID { get; set; }
        [Required]
        public int fkOrder_ID{ get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Zipcode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Phone { get; set; }

        public BillingInfoDTO()
        {
        }

        public BillingInfoDTO(int Order_ID, string email, string firstname, string lastname, string address, int zipcode, string city, string country, string phone)
        {
            fkOrder_ID = Order_ID;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Address = address;
            Zipcode = zipcode;
            City = city;
            Country = country;
            Phone = phone;
        }

        public BillingInfoDTO(string firstname, string lastname, string address, int zipcode, string city, string country, string phone, string email)
        {
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
            Address = address;
            Zipcode = zipcode;
            City = city;
            Country = country;
            Phone = phone;
        }

    }

    
}