using GroupProject.Data;
using GroupProject.Models;
using GroupProject.ViewModel;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    public class PayPalController : Controller
    {
        readonly ApplicationDbContext db = new ApplicationDbContext();

        //  Work with PayPal Payment
        private PayPal.Api.Payment payment;


        // Create a payment using an APIContext
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, Models.Order order) // OM: add order
        {
            // similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            // OM: find price of each item exclusively, and of each item * quantity in order to find the total price
            var carts = db.Carts.Where(x => x.CartID == User.Identity.Name).ToList();
            List<decimal> price = new List<decimal>();
            List<decimal> totalPrices = new List<decimal>();
            int count = 0;
            foreach (var item in carts)
            {
                price.Add(item.Product.Price);
                itemList.items.Add(new Item()
                {
                    name = item.Product.Name,
                    price = price[count].ToString(),
                    quantity = item.Quantity.ToString(),
                    currency = "EUR",
                    sku = item.ProductID.ToString()
                });
                totalPrices.Add(price[count] * item.Quantity);
                count++;
            }

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // OM: get total price of items in cart
            var total = totalPrices.Sum();
            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                //tax = "1",
                //shipping = "1",
                subtotal = total.ToString()
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "EUR",
                total = total.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            // OM: invoice number must be unique. Unique to the paypal sandbox account that is
            // so let's give it a huge random string and hope for the best
            var invoice = GetInvoice();

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "sales",
                invoice_number = order.ID.ToString() + invoice,
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

            // Create a payment using an APIContext
            return this.payment.Create(apiContext);
        }


        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        //
        // GET: /PayPal/PaymentWithPaypal
        [Authorize(Roles = "User")]
        public ActionResult PaymentWithPaypal() // OM: add order
        {
            // OM: get order from POST:/Checkout, if null show error view
            Models.Order order = TempData["Order"] as Models.Order;
            if (order == null)
                return View("FailureView");

            // getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    // this section will be executed first because PayerID doesn't exist
                    // it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPayPal?";

                    // guid we are generating for storing the paymentID received in session
                    // after calling the create function and it is used in the payment execution
                    var guid = Convert.ToString((new Random()).Next(100000));

                    // CreatePayment function gives us the payment approval url
                    // on which payer is redirected for paypal account payment
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, order); // OM: add order

                    // get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            // saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create

                    // Executing a payment
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                        return View("FailureView");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                PayPalLogger.Log("Error" + ex.Message);
                return View("FailureView");
            }

            // OM: Update database Orders with current after payment is complete
            SaveOrder(order);

            TempData["RedirectedFromPayment"] = true; // Flag to check that user was redirected to Checkout/Complete. Prevents ability to refresh said page and send duplicate emails

            return RedirectToAction("Complete", "Checkout", new { order.ID });
        }

        //
        // GET: /Paypal/PaymentWithCreditCart
        [Authorize(Roles = "User")]
        public ActionResult PaymentWithCreditCard()
        {
            Models.Order order = TempData["Order"] as Models.Order;
            if (order == null)
                return View("FailureView");

            CreditCardViewModel model = TempData["model"] as CreditCardViewModel;
            if (model == null)
                return View("FailureView");

            var cart = ShoppingCart.GetCart(this.HttpContext);
            var cartItems = cart.GetCartItems();

            List<Item> orderItems = new List<Item>();

            foreach (var cartItem in cartItems)
            {
                var item = new Item();

                item.name = cartItem.Product.Name;
                item.currency = "EUR";
                item.price = cartItem.Product.Price.ToString();
                item.quantity = cartItem.Quantity.ToString();

                orderItems.Add(item);
            }
            ItemList itemList = new ItemList();
            itemList.items = orderItems;

            //Address for the payment
            Address billingAddress = new Address();
            billingAddress.country_code = "GR";
            billingAddress.line1 = model.Address;
            billingAddress.postal_code = model.PostalCode.ToString();
            billingAddress.city = model.City;

            //Now Create an object of credit card and add above details to it
            CreditCard card = new CreditCard();
            card.billing_address = billingAddress;
            card.cvv2 = model.CVV.ToString();
            card.expire_month = model.ExMonth;
            card.expire_year = model.ExYear;
            card.first_name = model.Firstname;
            card.last_name = model.Lastname;
            card.number = model.CardNumber.ToString();
            card.type = model.CardType;


            var total = cart.GetTotal();
            // Specify details of your payment amount.
            Details details = new Details();

            details.subtotal = total.ToString();
        
            // Specify your total payment amount and assign the details object
            Amount amount = new Amount();
            amount.currency = "EUR";
            // Total = shipping tax + subtotal.
            amount.total = total.ToString();
            amount.details = details;

            var invoice = GetInvoice();
            // Now make a trasaction object and assign the Amount object
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "sales",
                invoice_number = order.ID.ToString() + invoice,
                amount = amount,
                item_list = itemList
            });

            // Now, we have to make a list of trasaction and add the trasactions object
            // to this list. You can create one or more object as per your requirements
           
            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above
            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = card;

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
            pymnt.transactions = transactionList;

            try
            {
                //getting context from the paypal, basically we are sending the clientID and clientSecret key in this function 
                //to the get the context from the paypal API to make the payment for which we have created the object above.
                // Basically, apiContext has a accesstoken which is sent by the paypal to authenticate the payment to facilitator account. An access token could be an alphanumeric string
                APIContext apiContext = Configuration.GetAPIContext();

                // Create is a Payment class function which actually sends the payment details to the paypal API for the payment. The function is passed with the ApiContext which we received above.
                Payment payment = pymnt.Create(apiContext);

                //if the createdPayment.State is "approved" it means the payment was successfull else not
                if (payment.state.ToLower() != "approved")
                {

                    return View("FailureView");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                PayPalLogger.Log("Error" + ex.Message);
                return View("FailureView");
            }
            order.FirstName = model.Firstname;
            order.LastName = model.Lastname;
            order.Address = model.Address;
            order.City = model.City;
            order.PostalCode = model.PostalCode;

            SaveOrder(order);

            TempData["RedirectedFromPayment"] = true; // Flag to check that user was redirected to Checkout/Complete. Prevents ability to refresh said page and send duplicate emails

            return RedirectToAction("Complete", "Checkout", new { order.ID });
        }
        // OM: Helper methods

        private void SaveOrder(Models.Order order)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            db.Orders.Add(order);
            db.SaveChanges();
            order.ID = cart.CreateOrder(order);
        }


        private string GetInvoice()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*()_+`-=[]{}<>,.";
            var stringChars = new char[20];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            var finalString = new string(stringChars);
            return (finalString);
        }
    }
}