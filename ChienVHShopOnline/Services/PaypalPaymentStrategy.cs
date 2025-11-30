using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using PayPal.Api;

namespace ChienVHShopOnline.Services
{
    public class PaypalPaymentStrategy : IPaymentStrategy
    {
        private Payment payment;

        public ActionResult ProcessPayment(Controller controller, IList<Cart> cartItems)
        {
            if (cartItems == null || cartItems.Count == 0)
            {
                return controller.View("Failure");
            }
            []
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                string payerId = controller.Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = controller.Request.Url.Scheme + "://" +
                                     controller.Request.Url.Authority +
                                     "/ShoppingCart/PaymentWithPaypal?";

                    var guid = Convert.ToString(new Random().Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, cartItems);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }

                    controller.Session.Add(guid, createdPayment.id);
                    return controller.Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = controller.Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, controller.Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return controller.View("Failure");
                    }
                }
            }
            catch (Exception ex)
            {
                PaypalLogger.Log("Error: " + ex.Message);
                return controller.View("Failure");
            }

            return controller.View("Success");
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, IList<Cart> listCarts)
        {
            var listItems = new ItemList() { items = new List<Item>() };

            foreach (var cart in listCarts)
            {
                listItems.items.Add(new Item()
                {
                    name = cart.Product.ProductName,
                    currency = "USD",
                    price = cart.Product.Price.ToString(),
                    quantity = cart.Quantity.ToString(),
                    sku = "sku"
                });
            }

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = listCarts.Sum(x => x.Quantity * x.Product.Price).ToString()
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) +
                         Convert.ToDouble(details.shipping) +
                         Convert.ToDouble(details.subtotal)).ToString(),
                details = details
            };

            var transactionList = new List<Transaction>
            {
                new Transaction()
                {
                    description = "Chien Testing transaction description",
                    invoice_number = Convert.ToString(new Random().Next(100000)),
                    amount = amount,
                    item_list = listItems
                }
            };

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }
    }
}
