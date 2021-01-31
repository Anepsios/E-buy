using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GroupProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "datetime2")]
        [DisplayFormat(NullDisplayText = "I am inevitable")]
        public DateTime? Created { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayFormat(NullDisplayText = "Never")]
        public DateTime? LastLog { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayFormat(NullDisplayText = "Never")]
        public DateTime? CurrentLog { get; set; }
        [DisplayName("First Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have more than 3 letters")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have more than 3 letters")]
        public string LastName { get; set; }
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Address must have more than 5 letters")]
        public string Address { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "City must have more than 3 letters")]
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Invalid Postal Code")]
        public string PostalCode { get; set; }
        public bool Subscribe { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}