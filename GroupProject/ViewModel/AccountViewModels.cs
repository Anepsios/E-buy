using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GroupProject.Models
{
 
    public class LoginViewModel 
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel 
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

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
    }
}
