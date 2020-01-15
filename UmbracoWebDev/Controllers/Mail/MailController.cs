using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core;
using Umbraco.Web.Security;
using UmbracoWebDev.Models;
using WebControllers.Controllers;
using WebControllers.Helpers;
using WebControllers.Models.Order;

namespace UmbracoWebDev.Controllers.Mail
{
    public class MailController
    {
        OrderController oc = new OrderController();
        ProductsController pc = new ProductsController();

        public void SendOrderConfirmationMail(int orderID)
        {
            OrderDTO order = oc.GetSingleOrderFromID(orderID);
            List<OrderlineDTO> orderLines = oc.GetListOfOrderLineDTOs(orderID);
            BillingInfoDTO billingInfo = oc.GetBillingInfoForOrder(orderID);
            MailAddress to = new MailAddress(billingInfo.Email, billingInfo.Firstname);
            MailAddress from = new MailAddress("GengangereneHangFire@gmail.com", "SQMY");

            MailMessage message = new MailMessage(from, to);
            message.Subject = string.Format("Hygge Hejsa");
            message.Body = CreateOrderEmailBody(order, orderLines, billingInfo);
            message.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true
            })

                try
                {
                    NetworkCredential nc = new NetworkCredential("GengangereneHangFire@gmail.com", "Pr4ktikH4ngFir3");
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    client.Credentials = nc;
                    client.Send(message);
                }
                catch (Exception)
                {

                }


        }

        private string CreateOrderEmailBody(OrderDTO order, List<OrderlineDTO> orderLines, BillingInfoDTO billingInfo)
        {
            string body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Views/Templates/OrderConfirmationEmailTemplate.html"));
            string prodIdListString = "";
            string prodAmountListString = "";
            string prodPricesListString = "";
            string prodPricesTotalListString = "";
            float orderTotalPrice = 0;
            string prodNameListString = "";


            Random rnd = new Random();


            DateTime datetime = DateTime.Now;
            var shippingDate = datetime.Date.AddDays(rnd.Next(1,6)); 
            
           


            foreach (var item in orderLines)
            {
                prodIdListString += string.Format("<p>{0}</p>", item.fkProduct_ID);
                prodAmountListString += string.Format("<p>{0}</p>", item.Product_Amount);
                prodPricesListString += string.Format("<p>{0}</p>",  item.Product_Price);
                prodPricesTotalListString += string.Format("<p>{0}</p>", (item.Product_Price * item.Product_Amount));
                orderTotalPrice += (item.Product_Price * item.Product_Amount);
                prodNameListString += string.Format("<p>{0}</p>", pc.GetProductNameByProductID(item.fkProduct_ID, ConnectionHelper.GetEnglishConnectionString()));
            }

            //Insert member infos
            body = body.Replace("{member_name}", "NAME");
            body = body.Replace("{member_address}", "Minvej 2");
            body = body.Replace("{member_city}", "Minby 2");
            body = body.Replace("{member_country}", "Denmark");

            //Insert shipment infos
            body = body.Replace("{shipping_name}", string.Format("{0} {1}", billingInfo.Firstname, billingInfo.Lastname));
            body = body.Replace("{shipping_address}", string.Format("{0}", billingInfo.Address));
            body = body.Replace("{shipping_city}", string.Format("{0} {1}", billingInfo.Zipcode ,billingInfo.City));
            body = body.Replace("{shipping_country}", string.Format("{0}", billingInfo.Country));
            body = body.Replace("{shipping_email}", string.Format("{0}", billingInfo.Email));
            body = body.Replace("{shipping_phone}", string.Format("{0}", billingInfo.Phone));
            body = body.Replace("{arrival_date}", string.Format("{0}", shippingDate.ToString("dd-MM-yyyy")));

            //OrderInfos
            body = body.Replace("{order_id}", order.Order_ID.ToString());
            body = body.Replace("{member_id}", order.Member_ID.ToString());
            body = body.Replace("{product_names}", string.Format("{0}", prodNameListString));
            body = body.Replace("{product_ids}", string.Format("{0}", prodIdListString));
            body = body.Replace("{product_amounts}",prodAmountListString);
            body = body.Replace("{product_prices}",prodPricesListString);
            body = body.Replace("{product_prices_total}",prodPricesTotalListString);
            body = body.Replace("{order_total}", orderTotalPrice.ToString());
            body = body.Replace("{order_date}", order.CreatedOn.ToString());


            return body;
        }

        public void SendContactMail(ContactModel model)
        {

            MailAddress from = new MailAddress(model.EmailAddress, (model.FirstName + " " + model.LastName));
            MailAddress to = new MailAddress("GengangereneHangFire@gmail.com", "SQMY");


            MailMessage message = new MailMessage(from, to);
            message.Subject = string.Format("Enquiry form {0} {1} - {2}", model.FirstName, model.LastName, model.Subject);
            message.Body = string.Format("Message from {0} \n{1}", model.EmailAddress, model.Message);


            using (SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true
            })

                try
                {
                    NetworkCredential nc = new NetworkCredential("GengangereneHangFire@gmail.com", "Pr4ktikH4ngFir3");
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    client.Credentials = nc;
                    client.Send(message);
                    ContactMailAutoReply(model);
                }
                catch (Exception)
                {

                }
        }
        
        public void ContactMailAutoReply(ContactModel model)
        {
            MailAddress to = new MailAddress(model.EmailAddress, (model.FirstName + " " + model.LastName));
            MailAddress from = new MailAddress("GengangereneHangFire@gmail.com", "SQMY");


            MailMessage message = new MailMessage(from, to);
            message.Subject = string.Format("Thank you for your email regarding - {0}", model.Subject);
            message.Body = string.Format("We have recieved your email regarding - {0}, \nWe will try our best to reply within 5 workdays", model.Subject);


            using (SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true
            })

                try
                {
                    NetworkCredential nc = new NetworkCredential("GengangereneHangFire@gmail.com", "Pr4ktikH4ngFir3");
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    client.Credentials = nc;
                    client.Send(message);
                }
                catch (Exception)
                {

                }


        }

    }
}