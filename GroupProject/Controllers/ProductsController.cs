using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GroupProject.Data;
using GroupProject.Hubs;
using GroupProject.Models;
using GroupProject.ViewModel;
using PagedList;
using PagedList.Mvc;

namespace GroupProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string selectedCategory, string selectedManufacturer, int? page)
        {
            QueryParamArgs args = new QueryParamArgs();
            args.sortOrder = sortOrder == null ? "" : sortOrder;
            args.currentFilter = currentFilter == null ? "" : currentFilter;
            args.searchString = searchString == null ? "" : searchString;
            args.selectedCategory = selectedCategory == null ? "" : selectedCategory;
            args.selectedManufacturer = selectedManufacturer == "undefined" || selectedManufacturer == null ? "" : selectedManufacturer;
            args.page = page == null ? 1 : page;

            ViewBag.SortOrder = sortOrder;
            return View(args);
        }

        public ActionResult GetProductData(string sortOrder, string currentFilter, string searchString, string selectedCategory, string selectedManufacturer, int? page)
        {
            ViewBag.PageName = "Products";

            var products = db.Products.Include(p => p.Category).Include(p => p.Manufacturer); //OM: get all products
            ViewBag.IsAdminOrEmployee = User.IsInRole("Admin") || User.IsInRole("Employee"); //OM: Used to check what to hide things from employee/admin
            ViewBag.IsUser = User.IsInRole("User"); // OM: to show only to user

            products = products.OrderBy(x => x.Name); // OM: initial order, pagedlist must have been ordered at least once

            // OM: Searchbar
            if (!string.IsNullOrEmpty(searchString))
                page = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString == "" ? "" : searchString; // OM: to keep searchstring in different pages
            ViewBag.CurrentPlaceHolder = searchString == "" ? "Search for Products" : searchString;
            //ViewBag.CurrentFilter = searchString; 

            // OM: filter by category
            var categories = (from c in db.Categories select c.Name).Distinct();
            ViewBag.SelectedCategory = new SelectList(categories);
            ViewBag.CurrentCategory = selectedCategory;

            // OM: filter by manufacturer
            var manufacturers = (from c in db.Manufacturers select c.Name).Distinct();
            ViewBag.SelectedManufacturer = new SelectList(manufacturers);
            ViewBag.CurrentManufacturer = selectedManufacturer;

            // OM: actually do the filtering according to the above
            if (!string.IsNullOrEmpty(selectedManufacturer) && !string.IsNullOrEmpty(selectedCategory) && !string.IsNullOrEmpty(searchString))
                products = db.Products.Where(x => x.Category.Name == selectedCategory).Where(y => y.Manufacturer.Name == selectedManufacturer).Where(s => s.Name.Contains(searchString) || searchString.Contains(s.Name));
            else if ((!string.IsNullOrEmpty(selectedManufacturer) && !string.IsNullOrEmpty(searchString)))
                products = db.Products.Where(y => y.Manufacturer.Name == selectedManufacturer).Where(s => s.Name.Contains(searchString) || searchString.Contains(s.Name) || s.Category.Name.Contains(searchString) || searchString.Contains(s.Category.Name));
            else if ((!string.IsNullOrEmpty(selectedCategory) && !string.IsNullOrEmpty(searchString)))
                products = db.Products.Where(x => x.Category.Name == selectedCategory).Where(s => s.Name.Contains(searchString) || searchString.Contains(s.Name) || searchString.Contains(s.Manufacturer.Name) || s.Manufacturer.Name.Contains(searchString));
            else if (!string.IsNullOrEmpty(selectedManufacturer) && !string.IsNullOrEmpty(selectedCategory))
                products = db.Products.Where(x => x.Category.Name == selectedCategory).Where(y => y.Manufacturer.Name == selectedManufacturer);
            else if (!string.IsNullOrEmpty(selectedCategory))
                products = db.Products.Where(x => x.Category.Name == selectedCategory);
            else if (!string.IsNullOrEmpty(selectedManufacturer))
                products = db.Products.Where(x => x.Manufacturer.Name == selectedManufacturer);
            else if (!string.IsNullOrEmpty(searchString))
                products = db.Products.Where(x => x.Name.Contains(searchString) || x.Manufacturer.Name.Contains(searchString) || x.Category.Name.Contains(searchString)
                                                  || searchString.Contains(x.Name) || searchString.Contains(x.Manufacturer.Name) || searchString.Contains(x.Category.Name));

            // OM: sort by price
            ViewBag.sortParam = string.IsNullOrEmpty(sortOrder) ? "price_asc" : ""; // OM: default sort is price ascending
            ViewBag.CurrentSort = sortOrder; // OM: to keep sortorder in different pages
            if (string.IsNullOrEmpty(sortOrder) ? false : sortOrder.Equals("price_asc")) // OM: !sortOrder == "price_asc" but i read somewhere sometime to use string.Equals() to compare strings. Probably simplify it later when I'm no longer emotionally attached to the ternary operator
            {
                products = products.OrderByDescending(x => x.Price);
                ViewBag.Descending = true; // OM: to check in View and print it
            }
            else
            {
                products = products.OrderBy(x => x.Price);
                ViewBag.Descending = false; // OM: to check in View and print it
            }

            ViewBag.ItemCount = products.Count();

            // OM: pages
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return PartialView("_ProductData", products.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RandomItems()
        {

            return View();
        }

        //
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.PageName = "Products";

            ViewBag.AddedToCart = "";
            if (!(TempData["AddedToCart"] == null))
                ViewBag.AddedToCart = TempData["AddedToCart"].ToString();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var productRatingsCount = product.Ratings.Count();
            var productApprovedRatingsCount = product.Ratings.Where(x => x.IsApproved == true).ToList().Count;
            ViewBag.RatingExist = productRatingsCount > 0;
            ViewBag.ApprovedRatingExist = productApprovedRatingsCount > 0;

            return View(product);
        }

        //
        // GET: Products/Create
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Create()
        {
            ViewBag.PageName = "Products";

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name");
            return View();
        }

        //
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID, Name, Description, ProductImage, Price, CategoryID, ManufacturerID")] Product product)
        {
            product.ProductImage = "~/Images/" + product.ProductImage;
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                UpdateProductsHub.BroadcastData();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name", product.ManufacturerID);
            return View(product);
        }

        //
        // GET: Products/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Edit(int? id)
        {
            ViewBag.PageName = "Products";

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Product product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name", product.ManufacturerID);
            return View(product);
        }

        //
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, Name, Description, ProductImage, Price, OldPrice, CategoryID, ManufacturerID, Offer, Discount")] Product product)
        {
            if (ModelState.IsValid)
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
                ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name", product.ManufacturerID);
                if (product.Discount == null && product.Offer)
                {
                    product.Offer = false;
                    ModelState.AddModelError("Offer", "Discount cannot be empty.");
                    return View(product);
                }

                // OM: Switch prices between old and new according to discount and in case price is editied while on discount
                if (product.Offer && (product.OldPrice == 0))
                {
                    product.OldPrice = product.Price;
                    product.Price = (decimal)(product.Price - (product.Price * (decimal)product.Discount / 100m));
                }
                // OM: if offer is true, then the old price property is taken in the View
                else if (product.Offer && product.Price / (decimal)(product.Discount / 100m) != product.OldPrice)
                    product.Price = (decimal)(product.OldPrice - (product.OldPrice * (decimal)product.Discount / 100));
                else if (!product.Offer && product.OldPrice > 0)
                {
                    product.Price = product.OldPrice;
                    product.OldPrice = 0;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                // OM: update prices that are already in cart
                UpdateCartPrices(product);

                UpdateProductsHub.BroadcastData();
                return RedirectToAction("Details", new { id = product.ID });
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name", product.ManufacturerID);
            return View(product);
        }

        public void UpdateCartPrices(Product product)
        {
            var cartsToUpdate = db.Carts.Where(x => x.ProductID == product.ID).Include(x => x.Product);
            foreach (var item in cartsToUpdate)
            {
                if (item.Product.Price != product.Price)
                    item.Product.Price = product.Price;
            }
        }

        //
        // GET: Products/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Delete(int? id)
        {
            ViewBag.PageName = "Products";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            UpdateProductsHub.BroadcastData();
            return RedirectToAction("Index");
        }

        public ActionResult GetApiProducts()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
