using GroupProject.Data;
using GroupProject.Models;
using GroupProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //
        // GET: /ShoppingCart/
        [ChildActionOnly]
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
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the product, to display confirmation
            string productName = context.Carts
                .Single(item => item.ID == id).Product.Name;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        ////
        //// AJAX: /ShoppingCart/AddToCartJson/5
        //[HttpPost]
        //[Authorize(Roles = "User")]
        //public ActionResult AddToCartJson(int id)
        //{
        //    // Add the item to the cart
        //    var cart = ShoppingCart.GetCart(this.HttpContext);
        //    // Get product to add
        //    var product = context.Products.Single(item => item.ID == id);
        //    // Add to cart
        //    cart.AddToCart(product);
        //    // Display the confirmation message
        //    var results = new ShoppingCartRemoveViewModel
        //    {
        //        Message = Server.HtmlEncode(productName) + " has been added from your shopping cart.",
        //        AddId = product.ID
        //    };
        //    return Json(results);
        //}

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