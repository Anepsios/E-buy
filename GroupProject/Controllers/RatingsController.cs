using GroupProject.Data;
using GroupProject.Models;
using GroupProject.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    public class RatingsController : Controller
    {

        private readonly ApplicationDbContext db = new ApplicationDbContext();


        //
        // GET: Ratings
        public async Task<ActionResult> Index()
        {
            var model = await db.Ratings.ToListAsync();
            return View(model);
        }


        //
        // GET: Ratings/Create
        [Authorize(Roles = "User")]
        public ActionResult Create(int? id)
        {
            var product = db.Products.Single(x => x.ID == id);
            var currentUserUsername = User.Identity.GetUserName();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Where(x => x.Id == currentUserId).Single();

            // OM: check if user has already rated the product, if yes show error page
            foreach (var item in product.Ratings)
            {
                if (item.UserName != null && item.UserName.Equals(currentUserUsername))
                {
                    TempData["UserHasRated"] = true;
                    return RedirectToAction("RatingFail", "Ratings", new { id });
                }
            }

            // OM: check if user has already bought the product, if no then redirect to error page
            bool userHasBoughtProduct = db.Orders.Where(x => x.UserName == currentUserUsername).SelectMany(y => y.OrderDetails.Where(z => z.ProductID == id)).Count() > 0;
            if (!userHasBoughtProduct)
            {
                TempData["UserHasNotOrderedProduct"] = true;
                return RedirectToAction("RatingFail", "Ratings", new { id });
            }

            ViewBag.Stars = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text="1", Value = "1" },
                        new SelectListItem() {Text="2", Value = "2" },
                        new SelectListItem() {Text="3", Value = "3" },
                        new SelectListItem() {Text="4", Value = "4" },
                        new SelectListItem() {Text="5", Value = "5" }
                    };

            return View();
        }


        //
        // POST: Ratings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RatingId, RatingText, IsApproved, UserName, ProductId, Stars")] Rating rating, int? id)
        {
            if (ModelState.IsValid)
            {
                rating.UserName = User.Identity.GetUserName();
                var currentProductId = db.Products.Where(p => p.ID == id).Select(x => x.ID).Single();
                rating.ProductId = currentProductId;
                rating.ReviewCreated = DateTime.Now;
                db.Ratings.Add(rating);
                db.SaveChanges();
            }
            return RedirectToAction("RatingSuccess", "Ratings", new { id });
        }


        //
        //
        [Authorize]
        public ActionResult RatingSuccess(int? id)
        {
            return View(id);
        }


        //
        //
        // OM: when user has already made a review for a product
        [Authorize]
        public ActionResult RatingFail(int? id)
        {
            // OM: Default state is false in case someone tries to access page directly from url
            if (TempData["UserHasNotOrderedProduct"] != null)
                ViewBag.UserHasNotOrderedProduct = TempData["UserHasNotOrderedProduct"];
            else
                ViewBag.UserHasNotOrderedProduct = false;
            if (TempData["UserHasRated"] != null)
                ViewBag.UserHasRated = TempData["UserHasRated"];
            else
                ViewBag.UserHasRated = false;
            return View(id);
        }


        //
        // GET: Ratings/Edit/5
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Stars = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text="1", Value = "1" },
                        new SelectListItem() {Text="2", Value = "2" },
                        new SelectListItem() {Text="3", Value = "3" },
                        new SelectListItem() {Text="4", Value = "4" },
                        new SelectListItem() {Text="5", Value = "5" }
                    };
            
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            // OM: only allow user who made the review to edit the review
            Rating rating = await db.Ratings.FindAsync(id);
            if (User.Identity.GetUserName() != rating.UserName)
                return View("Error");
            if (rating == null)
                return HttpNotFound();
            return View(rating);
        }


        //
        // POST: Ratings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RatingId, RatingText, UserName, IsApproved, ProductId, ReviewCreated, Stars")] Rating rating, int? id)
        {
            if (ModelState.IsValid)
            {
                rating.IsApproved = false;
                rating.IsEdited = true;
                rating.ReviewCreated = DateTime.Now;
                db.Entry(rating).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("RatingSuccess");
            }
            // OM: If all else fails
            return View(rating);
        }


        //
        // GET: Ratings/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            // OM: only allow user who made the review to edit the review
            Rating rating = await db.Ratings.FindAsync(id);
            if (User.Identity.GetUserName() != rating.UserName && !User.IsInRole("Admin"))
                return View("Error");
            if (rating == null)
                return HttpNotFound();
            return View(rating);
        }


        //
        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Rating rating = await db.Ratings.FindAsync(id);
            db.Ratings.Remove(rating);
            await db.SaveChangesAsync();
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Products");
            }
            return RedirectToAction("RatingsList", "Admin");
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