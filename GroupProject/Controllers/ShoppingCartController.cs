using GroupProject.Data;
using GroupProject.Models;
using GroupProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    [Authorize(Roles = "User")]
    public class ShoppingCartController : Controller
    {
        readonly ApplicationDbContext context = new ApplicationDbContext(); // OM: make readonly

        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            ViewBag.PageName = "Cart";
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        public ActionResult Cart()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return PartialView(viewModel);
        }

        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            TempData["AddedToCart"] = "Oops, something went wrong";
            // Retrieve product from the database
            var addedProduct = context.Products.Single(product => product.ID == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedProduct);
            TempData["AddedToCart"] = "Product added successfully";
            return RedirectToAction("Details", "Products", new { id = (int?)id });
        }
       
        //
        //AJAX:  /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the product, to display confirmation
            try
            {
                string productName = context.Carts.SingleOrDefault(item => item.ID == id).Product.Name;
                var quantity = context.Carts.SingleOrDefault(item => item.ID == id).Quantity;
                string productQuantity = (quantity - 1).ToString();
                string manufacturer = context.Carts.SingleOrDefault(item => item.ID == id).Product.Manufacturer.Name;
                // Remove from cart
                int itemCount = cart.RemoveFromCart(id);
                // Display the confirmation message
                var results = new ShoppingCartRemoveViewModel
                {
                    Message ="The product has been removed. "+"\'"+ Server.HtmlEncode(productQuantity) + "\'" + " " + Server.HtmlEncode(manufacturer) + " " + Server.HtmlEncode(productName) + " left in the cart.",
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeleteId = id
                };
                return Json(results);
            }
            catch
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        //
        //AJAX:  /ShoppingCart/AddToCartJson/5
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult AddToCartJson(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get product to add
            var product = context.Products.Single(item => item.ID == id);
            // Add to cart
            var itemCount = cart.AddToCartInt(product);
            int? cartId = context.Carts.Where(x => x.ProductID == id && x.CartID == User.Identity.Name).SingleOrDefault().ID;

            if (cartId == null)
                return View("Error");
            string productName = context.Products.Single(item => item.ID == id).Name;
            var quantity = itemCount;
            string manufacturer = context.Products.SingleOrDefault(item => item.ID == id).Manufacturer.Name;
            var results = new
            {
                Message = "Product succesfully added! There are " + "\'" + Server.HtmlEncode(quantity.ToString())  +"\'" + " " + Server.HtmlEncode(manufacturer) + " " + Server.HtmlEncode(productName) + " in cart!",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                AddId = cartId,
                Price = product.Price,
                Name = product.Name,
                Manufacturer = manufacturer
            };
            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewBag.CartCount = cart.GetCount();
            return PartialView("CartSummary");
        }

    }
}