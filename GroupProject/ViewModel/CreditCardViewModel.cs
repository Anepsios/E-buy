using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.ViewModel
{
    public class CreditCardViewModel
    {
        [Required]
        [Range(1, 9999, ErrorMessage = "Invalid Number")]
        public int CVV { get; set; }
        [Required]
        [Range(1, 12, ErrorMessage = "Invalid Number")]
        public int ExMonth { get; set; }
        [Required]
        [Range(2021, 2040, ErrorMessage = "Invalid Number")]
        public int ExYear { get; set; }
        [Required]
        [Range(1,9999999999999999 , ErrorMessage = "Invalid Card Number")]
        public long CardNumber { get; set; }
        [Required]
        public string CardType { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Invalid Postal Code")]
        public string PostalCode { get; set; }
        
       
    }
}