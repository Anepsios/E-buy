using GroupProject.Data;
using GroupProject.Email;
using GroupProject.Models;
using GroupProject.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    public class CheckoutController : Controller
    {

        readonly ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /Checkout
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            ViewBag.PageName = "Cart";
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewBag.NoOrders = false;
            if (cart.GetCartItems().Count == 0)
            {
                ViewBag.NoOrders = true;
                return View();
            }

            var user = context.Users.Single(x => x.UserName == User.Identity.Name);
            OrderViewModel model = new OrderViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode
            };

            return View(model);
        }

        //
        // POST: /Checkout
        [HttpPost]
        public ActionResult Index(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NoOrders = false; // OM: because GET /Checkout uses this Viewbag bool for a check, so it cant be null
                return View(model);
            }

            var order = new Order();
            TryUpdateModel(order);
            order.UserName = User.Identity.GetUserName();
            try
            {
                order.OrderDate = DateTime.Now;
                var cart = ShoppingCart.GetCart(this.HttpContext);
                order.TotalPrice = cart.GetTotal();

                // OM: Pass order info to PayPal payment and add the order only after payment has gone through
                TempData["Order"] = order;
                return RedirectToAction("PaymentWithPaypal", "PayPal");
            }
            catch
            {
                ViewBag.NoOrders = false; // OM: because GET: /Checkout uses this Viewbag bool for a check, so it cant be null
                return View(model);
            }
        }

        //
        // GET: /Checkout/CreditCard
        [Authorize(Roles = "User")]
        public ActionResult CreditCard()
        {
           
            ViewBag.PageName = "Cart";
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewBag.NoOrders = false;

            if (cart.GetCartItems().Count == 0)
            {
                ViewBag.NoOrders = true;
                return View();
            }

            ViewBag.Cards = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text="MasterCard", Value = "mastercard" },
                        new SelectListItem() {Text="Visa", Value = "visa" },
                        new SelectListItem() {Text="Discover", Value = "discover" },
                        new SelectListItem() {Text="Amex", Value = "amex" }
                    };

            return View();
        }

        //
        // POST: /Checkout/CreditCard
        [HttpPost]
        public ActionResult CreditCard(CreditCardViewModel model)
        {
            ViewBag.Cards = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text="MasterCard", Value = "mastercard" },
                        new SelectListItem() {Text="Visa", Value = "visa" },
                        new SelectListItem() {Text="Discover", Value = "discover" },
                        new SelectListItem() {Text="Amex", Value = "amex" }
                    };
            ViewBag.NoOrders = false; // OM: because GET /Checkout uses these Viewbag bool/list for a check, so they cant be null

            if (!ModelState.IsValid)
                return View(model);

            var order = new Order();
            TryUpdateModel(order);

            try
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
                order.UserName = User.Identity.GetUserName();
                order.OrderDate = DateTime.Now;
                order.TotalPrice = cart.GetTotal();

                // OM: Pass order info to PayPal payment and add the order only after payment has gone through
                TempData["Order"] = order;
                TempData["model"] = model;
                return RedirectToAction("PaymentWithCreditCard", "PayPal");
            }
            catch
            {
                return View(model);
            }
        }

        //
        //GET: /Checkout/Complete
        [Authorize(Roles = "User")]
        public ActionResult Complete(int? id)
        {
            if (TempData["RedirectedFromPayment"] == null || !(bool)TempData["RedirectedFromPayment"])
                return RedirectToAction("Index", "Home");

            if (id == null)
                return View("Error");

            ViewBag.PageName = "Cart";

            // Validate customer owns this order
            bool isValid = context.Orders.Any(
                o => o.ID == id &&
                o.UserName == User.Identity.Name);

            if (!isValid)
                return View("Error");

            var user = context.Users.Single(x => x.UserName == User.Identity.Name);
            List<Order> order = context.Orders.ToList();/*First(o => o.UserName == User.Identity.Name);*/
            OrderViewModel model = new OrderViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Email/OrderCompleted.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{LastName}", model.LastName);
            body = body.Replace("{FirstName}", model.FirstName);
            body = body.Replace("{Email}", model.Email);
            body = body.Replace("{TotalPrice}", order[order.Count - 1].TotalPrice.ToString());

            // OM: table with products bought
            string tableOfOrders = "";
            var currentOrder = context.Orders.Where(x => x.ID == id && x.UserName == User.Identity.Name).Single();
            string tableOfOrdersHead = "<tr><th>Product</th><th>Price</th><th>Quantity</th></tr>";
            string productUrl;
            foreach (var item in currentOrder.OrderDetails)
            {
                // OM: Get url of details page of product to send in email
                productUrl = this.Url.Action("Details", "Products", new { id = item.ProductID }, this.Request.Url.Scheme).ToString();
                tableOfOrders += "<tr style=\"text-align:center;\"><td><a href=\"" + productUrl + "\">" + item.Product.Manufacturer.Name + " " + item.Product.Name + "</a></td>" +
                                 "<td>" + item.Price.ToString(CultureInfo.InvariantCulture) + "</td>" +
                                 "<td>" + item.Quantity.ToString() + "</td></tr>";
            }
            tableOfOrders += "<tr><th>Total Price</th><th></th><th>" + currentOrder.TotalPrice.ToString() + "</th></tr>";
            body = body.Replace("{OrderDetails}", tableOfOrders);
            body = body.Replace("{Table}", tableOfOrdersHead);

            bool IsSendEmail = SendEmail.EmailSend(model.Email, "Order Completed", body, true);
            if (IsSendEmail)
                return View(id);
            return View("Error");
        }

        public ActionResult OrderComplete()
        {
            return View();
        }
    }
}