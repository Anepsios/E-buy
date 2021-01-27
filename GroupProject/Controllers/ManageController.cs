using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GroupProject.Models;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using GroupProject.Data;

namespace GroupProject.Controllers
{

    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        readonly ApplicationDbContext context; 


        public ManageController()
        {
            context = new ApplicationDbContext();
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        //
        // GET: /Manage/Index
        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.PageName = "Manage";
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeAccountDetailsSuccess ? "Profile updated successfully."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Created = user.Created,
                LastLogin = user.LastLog
            };
            var s = UserManager.GetRoles(userId);
            if (s[0].ToString().Equals("User"))
                ViewBag.IsUser = true;
            else
                ViewBag.IsUser = false;
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            ViewBag.PageName = "Manage";
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/Edit
        [Authorize]
        public ActionResult Edit()
        {
            ViewBag.PageName = "Manage";
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode
            };
            return View(model);
        }

        //
        // POST: /Manage/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Username, Email, FirstName, LastName, Address, City, PostalCode")] IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // OM: Check if Username or Email already exists and throw error if yes 
            bool alreadyExists = false;
            var userEmail = UserManager.FindById(User.Identity.GetUserId()).Email;
            if (UserManager.FindByEmail(model.Email) != null && userEmail != model.Email)
            {
                alreadyExists = true;
                ModelState.AddModelError("Email", "Email already exists");
            }
            if (UserManager.FindByName(model.Username) != null && User.Identity.GetUserName() != model.Username)
            {
                alreadyExists = true;
                ModelState.AddModelError("Username", "Username already exists");
            }
            if (alreadyExists)
                return View(model);
            //

            // OM: Update user with changes from model (only those that are not null)
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            user.UserName = model.Username;
            user.Email = model.Email;
            if (model.FirstName != null)
                user.FirstName = model.FirstName;
            if (model.LastName != null)
                user.LastName = model.LastName;
            if (model.Address != null)
                user.Address = model.Address;
            if (model.City != null)
                user.City = model.City;
            if (model.PostalCode != null)
                user.PostalCode = model.PostalCode;
            //

            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var username = User.Identity.GetUserName();
                // OM: Migrate orders to new Username
                var orders = context.Orders.Where(x => x.UserName == username);
                foreach (var item in orders)
                {
                    item.UserName = model.Username;
                }

                // OM: Migrate reviews to new Username 
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

                await context.SaveChangesAsync();

                // OM: Sign in user after edit to update Session. Else user will stay signed in on wrong account.
                await SignInManager.SignInAsync(user, true, true);
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangeAccountDetailsSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/Orders
        [Authorize(Roles = "User")]
        public ActionResult Orders()
        {
            ViewBag.PageName = "Manage";
            var id = User.Identity.GetUserId();
            var user = UserManager.FindById(id);

            var model = context.Orders.Where(x => x.UserName == user.UserName).Include(y => y.OrderDetails.Select(z => z.Product)).ToList();
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeAccountDetailsSuccess,
            Error
        }
    }
}