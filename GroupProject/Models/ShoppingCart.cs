using GroupProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Models
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        public string CartID { get; set; }
        public int ProductID { get; set; }
        [Range(1, 100, ErrorMessage = "Invalid Quantity selected")]
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }
    }

    public class ShoppingCart
    {
        readonly ApplicationDbContext context = new ApplicationDbContext();

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartID";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public static void LeaveCart(HttpContextBase context)
        {
            context.Session[CartSessionKey] = null;
        }

        public void AddToCart(Product product)
        {
            // Get the matching cart and product instances
            var cartItem = context.Carts.SingleOrDefault(
                           c => c.CartID == ShoppingCartId
                           && c.ProductID == product.ID);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductID = product.ID,
                    CartID = ShoppingCartId,
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };
                context.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Quantity++;
            }
            context.SaveChanges();
        }

        public int AddToCartInt(Product product)
        {
            // Get the matching cart and product instances
            var cartItem = context.Carts.SingleOrDefault(
                           c => c.CartID == ShoppingCartId
                           && c.ProductID == product.ID);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductID = product.ID,
                    CartID = ShoppingCartId,
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };
                context.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Quantity++;
            }
            context.SaveChanges();
            return (cartItem.Quantity);
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = context.Carts.Single(
            cart => cart.CartID == ShoppingCartId
            && cart.ID == id);
            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    context.Carts.Remove(cartItem);
                }
                context.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = context.Carts.Where(cart => cart.CartID == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                context.Carts.Remove(cartItem);
            }
            context.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return context.Carts.Where(cart => cart.CartID == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in context.Carts
                          where cartItems.CartID == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            // Multiply product price by count of that product to get 
            // the current price for each of those products in the cart
            // sum all product price totals to get the cart total
            decimal? total = (from cartItems in context.Carts
                              where cartItems.CartID == ShoppingCartId
                              select (int?)cartItems.Quantity * cartItems.Product.Price).Sum();
            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetails
                {
                    ProductID = item.ProductID,
                    OrderID = order.ID,
                    Price = item.Product.Price,
                    Quantity = item.Quantity
                };

                // Set the order total of the shopping cart
                orderTotal += (item.Quantity * item.Product.Price);
                context.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.TotalPrice = orderTotal;
            // Save the order
            context.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.ID;
        }

        // We're using HttpContextBase to allow access to cookies.
        // OM: Anonymus cart is not used but implemented for possible future extentions
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        //When a user has logged in, migrate their shopping cart to
        //be associated with their username
        //public void MigrateCart(string userName)
        //{
        //    var shoppingCart = context.Carts.Where(c => c.CartID == ShoppingCartId);
        //    foreach (Cart item in shoppingCart)
        //    {
        //        item.CartID = userName;
        //    }
        //    context.SaveChanges();
        //}
    }
}