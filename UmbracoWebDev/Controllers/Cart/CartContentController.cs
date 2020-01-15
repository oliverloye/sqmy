using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using UmbracoWebDev.Models.Cart;
using Importer.Facade.Logic;
using System.Web;
using PayPal.Api;
using UmbracoWebDev.Models.Paypal;
using System;
using System.Collections.Generic;
using ClassLibrary.Entity;
using System.Web.Security;
using Importer.Models;
using ClassLibrary.DTOs;
using Importer.Facade;

namespace UmbracoWebDev.Controllers.Cart
{
    public class CartContentController : RenderMvcController
    {

        ShoppingCartFacade shoppingCartFacade = new ShoppingCartFacade();
        ProductController productFacade = new ProductController();
        OrderFacade orderFacade = new OrderFacade();

        public ActionResult CartContent(RenderModel model)
        {
            var items = new NewCartModel(model.Content);
            items.cartItems = shoppingCartFacade.GetCartItems();
            items.TotalCartPrice = (double)shoppingCartFacade.GetTotal();
            return CurrentTemplate(items);
        }

        public ActionResult Checkout(RenderModel model)
        {
            var items = new NewCartModel(model.Content);
            items.cartItems = shoppingCartFacade.GetCartItems();
            items.TotalCartPrice = (double)shoppingCartFacade.GetTotal();
            return CurrentTemplate(items);
        }


        public ActionResult PayWithCard(CheckoutOrderDTO orderInfo)
        {
            var cartList = shoppingCartFacade.GetCartItems();
            var itemList = new ItemList() { items = new List<Item>() };

            //The item's in the cart, and the items we're sending to Paypal is a little different, so we have to pass the cartitems into the paypal item's
            //We do this with a foreach method
            foreach (var x in cartList)
            {
                var pItem = new Item()
                {
                    name = x.Product.ProductName,
                    currency = "USD",
                    price = x.Product.UnitPrice.ToString(),
                    quantity = x.Quantity.ToString(),
                    sku = x.Product.ProductId
                };
                itemList.items.Add(pItem);
            }



            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.city = orderInfo.Order.City;
            billingAddress.country_code = "US"; //these should not be hard coded
            billingAddress.line1 = orderInfo.Order.Address;
            billingAddress.postal_code = orderInfo.Order.PostalCode;
            billingAddress.state = "NY"; //these should not be hard coded


            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = orderInfo.Card.cvv2;  //card cvv2 number
            crdtCard.expire_month = orderInfo.Card.expire_month; //card expire date
            crdtCard.expire_year = orderInfo.Card.expire_year; //card expire year
            crdtCard.first_name = orderInfo.Card.first_name;
            crdtCard.last_name = orderInfo.Card.last_name;
            crdtCard.number = orderInfo.Card.number; //enter your credit card number here
            crdtCard.type = orderInfo.Card.type; //credit card type here paypal allows 4 types

            // similar as we did for credit card, do here and create details object
            decimal cartTotal = shoppingCartFacade.GetTotal();
            decimal taxCalc = ((10 * cartTotal) / 100); //We pay 10% in tax...
            decimal shipCost = 0; //Free shipping
            decimal priceTotal = (cartTotal + taxCalc + shipCost);

            // Specify details of your payment amount.
            var details = new Details()
            {
                tax = taxCalc.ToString(),
                subtotal = cartTotal.ToString(),
                shipping = shipCost.ToString(),
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = priceTotal.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };


            // Now make a transaction object and assign the Amount object
            Transaction tran = new Transaction();
            tran.amount = amount;
            tran.description = "SQMY Webshop Stransaction";
            tran.item_list = itemList;
            tran.invoice_number = Convert.ToString((new Random()).Next(100000));

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            try
            {
                //getting context from the paypal
                //basically we are sending the clientID and clientSecret key in this function
                //to the get the context from the paypal API to make the payment
                //for which we have created the object above.

                //Basically, apiContext object has a accesstoken which is sent by the paypal
                //to authenticate the payment to facilitator account.
                //An access token could be an alphanumeric string

                APIContext apiContext = Configuration.GetAPIContext();

                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. The function is passed with the ApiContext
                //which we received above.

                Payment createdPayment = pymnt.Create(apiContext);

                //if the createdPayment.state is "approved" it means the payment was successful else not
                if (createdPayment.state.ToLower() == "approved")
                {
                    try
                    {
                        using (var _dbContext = new DatabaseContext())
                        {
                            MembershipUser customer = Membership.GetUser();

                            //We have to move the CartItem's into a OrderDetails list, like we did with the Api.PayPal item's
                            var OrderDetailList = new List<OrderDetail>();
                            foreach (var x in cartList)
                            {
                                var orderDetail = new OrderDetail()
                                {
                                    ProductId = x.ProductId,
                                    Quantity = x.Quantity,
                                    UnitPrice = productFacade.GetProductPriceById(x.ProductId)
                                };
                                OrderDetailList.Add(orderDetail);
                            }

                            //Both the Paypal.API and ClassLibrary have a class named Order(), so we have the specify the namespace
                            var CustomerOrder = orderInfo.Order;
                            CustomerOrder.PaymentTransactionId = orderFacade.GeneratePaymentTransactionId();
                            CustomerOrder.CustomerID = customer.ProviderUserKey.ToString();
                            CustomerOrder.OrderDetails = OrderDetailList;
                            CustomerOrder.Total = priceTotal;
                            CustomerOrder.OrderDate = DateTime.Now;

                            _dbContext.Orders.Add(CustomerOrder);
                            _dbContext.SaveChanges();

                            //Clear the cart and send us to successview
                            shoppingCartFacade.EmptyCart();
                            return View("CheckoutSuccess");
                        }
                    }
                    catch (Exception ex)
                    {
                        Models.Paypal.Logger.Log("Error: " + ex.Message);
                        return PartialView("~/views/partials/ErrorPage.cshtml", new Models.ErrorPageModel { ErrorMessage = "Something went wrong..." });
                    }
                }
                else
                {
                    return PartialView("~/views/partials/ErrorPage.cshtml", new Models.ErrorPageModel { ErrorMessage = "Payment Failed" });
                }
            }
            catch (PayPal.PaymentsException ex)
            {
                Models.Paypal.Logger.Log("Error: " + ex.Message);
                return PartialView("~/views/partials/ErrorPage.cshtml", new Models.ErrorPageModel { ErrorMessage = ex.Details.message });
            }
        }

        public ActionResult ProcessPayment()
        {
            //If the cart is empty, return an error view
            var cardItems = shoppingCartFacade.GetCartItems();
            var orderInfo = (CheckoutOrderDTO)TempData["orderInfoTemp"];

            if (cardItems.Count <= 0)
            {
                return PartialView("~/views/partials/ErrorPage.cshtml", new Models.ErrorPageModel { ErrorMessage = "Cart is empty" });
            }

            APIContext apiContext = Configuration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    return PayWithPaypal();
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create
                    // Executing a payment
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() == "approved")
                    {
                        try
                        {
                            using (var _dbContext = new DatabaseContext())
                            {
                                MembershipUser customer = Membership.GetUser();

                                //We have to move the CartItem's into a OrderDetails list, like we did with the Api.PayPal item's
                                var OrderDetailList = new List<OrderDetail>();
                                foreach (var x in shoppingCartFacade.GetCartItems())
                                {
                                    var orderDetail = new OrderDetail()
                                    {
                                        ProductId = x.ProductId,
                                        Quantity = x.Quantity,
                                        UnitPrice = productFacade.GetProductPriceById(x.ProductId)
                                    };
                                    OrderDetailList.Add(orderDetail);
                                }


                                // similar as we did for credit card, do here and create details object
                                decimal cartTotal = shoppingCartFacade.GetTotal();
                                decimal taxCalc = ((10 * cartTotal) / 100); //We pay 10% in tax...
                                decimal shipCost = 0; //Free shipping
                                decimal priceTotal = (cartTotal + taxCalc + shipCost);

                                //Both the Paypal.API and ClassLibrary have a class named Order(), so we have the specify the namespace
                                var CustomerOrder = orderInfo.Order;
                                CustomerOrder.PaymentTransactionId = orderFacade.GeneratePaymentTransactionId();
                                CustomerOrder.CustomerID = customer.ProviderUserKey.ToString();
                                CustomerOrder.OrderDetails = OrderDetailList;
                                CustomerOrder.Total = priceTotal;
                                CustomerOrder.OrderDate = DateTime.Now;

                                _dbContext.Orders.Add(CustomerOrder);
                                _dbContext.SaveChanges();

                                //Clear the cart and send us to successview
                                shoppingCartFacade.EmptyCart();
                                return View("CheckoutSuccess");
                            }
                        }
                        catch (Exception ex)
                        {
                            Models.Paypal.Logger.Log("Error: " + ex.Message);
                            return PartialView("~/views/partials/ErrorPage.cshtml", new Models.ErrorPageModel { ErrorMessage = "Something went wrong..." });
                        }
                    }
                    else
                    {
                        return PartialView("~/views/partials/ErrorPage.cshtml", new Models.ErrorPageModel { ErrorMessage = "Something went wrong..." });
                    }
                }
            }
            catch (PayPal.PaymentsException ex)
            {
                Models.Paypal.Logger.Log("Error" + ex.Message);
                return PartialView("~/views/partials/ErrorPage.cshtml", new Models.ErrorPageModel { ErrorMessage = ex.Details.message });
            }
        }


        /* PAYPAL STUFF */
        private Payment payment;

        public ActionResult PayWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();
            try
            {
                //this section will be executed first because PayerID doesn't exist
                //it is returned by the create function call of the payment class
                // Creating a payment
                // baseURL is the url on which paypal sendsback the data.
                // So we have provided URL of this controller only
                string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/cart_page/processpayment?";

                //guid we are generating for storing the paymentID received in session
                //after calling the create function and it is used in the payment execution
                var guid = Convert.ToString((new Random()).Next(100000));

                //CreatePayment function gives us the payment approval url
                //on which payer is redirected ffor paypal acccount payment
                var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, shoppingCartFacade.GetCartItems());

                //get links returned from paypal in response to Create function call
                var links = createdPayment.links.GetEnumerator();
                string paypalRedirectUrl = null;

                while (links.MoveNext())
                {
                    Links lnk = links.Current;

                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment
                        paypalRedirectUrl = lnk.href;
                    }
                }
                // saving the paymentID in the key guid
                Session.Add(guid, createdPayment.id);
                return Redirect(paypalRedirectUrl);
            }
            catch (PayPal.PaymentsException ex)
            {
                Models.Paypal.Logger.Log("Error" + ex.Message);
                return PartialView("~/views/partials/ErrorPage.cshtml", new Models.ErrorPageModel { ErrorMessage = ex.Details.message });
            }
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, List<CartItem> cartItems)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            //The item's in the cart, and the items we're sending to Paypal is a little different, so we have to pass the cartitems into the paypal item's
            //We do this with a foreach method
            foreach (var item in cartItems)
            {
                var pItem = new Item()
                {
                    name = item.Product.ProductName,
                    currency = "USD",
                    price = item.Product.UnitPrice.ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = item.Product.ProductId
                };
                itemList.items.Add(pItem);
            }

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            decimal cartTotal = shoppingCartFacade.GetTotal();
            decimal taxCalc = ((10 * cartTotal) / 100); //We pay 10% in tax...
            decimal shipCost = 0; //Free shipping
            decimal priceTotal = (cartTotal + taxCalc + shipCost);

            var details = new Details()
            {
                tax = taxCalc.ToString(),
                subtotal = cartTotal.ToString(),
                shipping = shipCost.ToString(),
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = priceTotal.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            string invoiceNr = Convert.ToString((new Random()).Next(100000));

            transactionList.Add(new Transaction()
            {
                description = "SQMY Transaction.",
                invoice_number = invoiceNr,
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

    }
}