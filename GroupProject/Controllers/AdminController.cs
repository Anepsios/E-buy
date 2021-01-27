using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GroupProject.Models;
using GroupProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using GroupProject.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Helpers;

namespace GroupProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        readonly ApplicationDbContext context;

        public AdminController()
        {
            context = new ApplicationDbContext();
        }

        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            ViewBag.PageName = "Admin";
            return View();
        }

        // OM: TODO! Simplify LinQ query, can't be that hard
        public ActionResult UserList()
        {
            ViewBag.PageName = "Admin";
            var users = (from user in context.Users
                         select new
                         {
                             user.Roles,
                             user.CurrentLog,
                             user.Created,
                             user.Id,
                             user.UserName,
                             user.Email,
                             RoleNames = (from userRole in user.Roles
                                          join role in context.Roles on userRole.RoleId
                                          equals role.Id
                                          select role.Name).ToList()
                         }).ToList().Select(p => new UserView()
                         {
                             LastLogin = p.CurrentLog,
                             Created = p.Created,
                             UserId = p.Id,
                             Username = p.UserName,
                             Email = p.Email,
                             UserRoles = string.Join(",", p.RoleNames)
                         });
            users = users.OrderBy(x => x.UserRoles);
            return View(users);
        }

        //-----------------------------------------User Actions----------------------------------
        public ActionResult DeleteUser(string id)
        {
            ViewBag.PageName = "Admin";
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = context.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            return View(user);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(string id)
        {
            var userid = context.Users.Where(x => x.Id == id).Single();
            // OM: Delete user's cart too
            var username = context.Users.Where(y => y.Id == id).Single().UserName;
            foreach (var item in context.Carts.Where(x => x.CartID == username))
            {
                context.Carts.Remove(item);
            }
            context.Users.Remove(userid);
            context.SaveChanges();
            return RedirectToAction("UserList");
        }

        public ActionResult EditUser(string id)
        {
            ViewBag.PageName = "Admin";
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = context.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            var model = new UserView()
            {
                Email = user.Email,
                Username = user.UserName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("EditUser")]
        public ActionResult EditUserConfirmed(string id, UserView model)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!ModelState.IsValid)
                return View(model);

            // OM: Check if Username or Email already exists and throw error if yes 
            bool alreayExists = false;
            var userEmail = UserManager.FindById(id).Email;
            var userUsername = UserManager.FindById(id).UserName;
            if (UserManager.FindByEmail(model.Email) != null && userEmail != model.Email)
            {
                alreayExists = true;
                ModelState.AddModelError("Email", "Email already exists");
            }
            if (UserManager.FindByName(model.Username) != null && userUsername != model.Username)
            {
                alreayExists = true;
                ModelState.AddModelError("Username", "Username already exists");
            }
            if (alreayExists)
                return View(model);
            //

            var userToUpdate = context.Users.Find(id);
            var username = userToUpdate.UserName;

            if (TryUpdateModel(userToUpdate, "", new string[] { "Email", "Username" }))
            {
                try
                {
                    // OM: Migrate orders to new User when Username changes
                    var orders = context.Orders.Where(x => x.UserName == username);
                    foreach (var item in orders)
                    {
                        item.UserName = model.Username;
                    }

                    // OM: Migrate orders to new User when Username changes
                    var ratings = context.Ratings.Where(x => x.UserName == username);
                    foreach (var item in ratings)
                    {
                        item.UserName = model.Username;
                    }

                    // OM: Migrate cart to new User when Username changes
                    var carts = context.Carts.Where(x => x.CartID == username);
                    foreach (var item in carts)
                    {
                        item.CartID = model.Username;
                    }

                    context.SaveChanges();
                    return RedirectToAction("UserList");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(userToUpdate);
        }

        public ActionResult Create()
        {
            ViewBag.PageName = "Admin";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Created = DateTime.Now, FirstName = model.FirstName, LastName = model.LastName, EmailConfirmed = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, "Employee");
                    return RedirectToAction("UserList", "Admin");
                }
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

        //-----------------------------------------Roles Actions----------------------------------
        public ActionResult IndexRole()
        {
            ViewBag.PageName = "Admin";
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        public Boolean IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                    return true;
                else
                    return false;
            }
            return false;
        }

        public ActionResult CreateRole()
        {
            ViewBag.PageName = "Admin";
            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("IndexRole");
        }

        public ActionResult DeleteRole(string id)
        {
            ViewBag.PageName = "Admin";
            var role = context.Roles.Find(id);
            return View(role);
        }

        [HttpPost]
        [ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleConfirmed(string id)
        {
            var roled = context.Roles.Where(x => x.Id == id).Single();
            context.Roles.Remove(roled);
            context.SaveChanges();
            return RedirectToAction("IndexRole");
        }


        // --------------------- Orders Actions -----------------------------
        public async Task<ActionResult> OrderList()
        {
            var model = await context.Orders.OrderByDescending(x => x.OrderDate).ToListAsync();
            return View(model);
        }

        //------------------------------------Rating Tools---------------------------

        public async Task<ActionResult> RatingsList()
        {
            var model = await context.Ratings.OrderBy(x => x.IsApproved).ThenBy(y => y.IsEdited).ThenByDescending(z => z.ReviewCreated).ToListAsync();
            return View(model);
        }

        public async Task<ActionResult> RatingDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = await context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        [HttpPost]
        [ActionName("RatingDetail")]
        public async Task<ActionResult> RatingDetailConfirmed([Bind(Include = "RatingId, RatingText, UserName, IsApproved, ProductId, ReviewCreated, Stars")] Rating rating, int? id)
        {
            if (!ModelState.IsValid)
                return View(rating);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (rating == null)
                return HttpNotFound();
            context.Entry(rating).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return RedirectToAction("RatingsList");
        }


        //--------------------------------------- CHARTS --------------------------------------//

        public ActionResult ChartBar()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            var results = (from c in context.Users
                           join o in context.Orders on c.UserName equals o.UserName
                           group o by c.UserName into g
                           select new
                           {
                               UserName = g.Key,
                               Username = (from cust in context.Users
                                           where cust.UserName == g.Key
                                           select cust).ToList(),
                               Count = g.Select(x => x.ID).Distinct().Count()
                           }).OrderBy(y => y.Count);

            results.ToList().ForEach(rs => xValue.Add(rs.UserName));
            results.ToList().ForEach(rs => yValue.Add(rs.Count));

            new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
            .AddTitle("Orders Per Active User")
                    .AddSeries("Default", chartType: "Bar", xValue: xValue, yValues: yValue)
                    .Write("png");
            return null;
        }
    }
}
