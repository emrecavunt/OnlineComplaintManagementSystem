using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EastMedRepo.Models
{
    public class AccountViewModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "University ID required")]
        [Display(Name = "University ID")]
        public string UNI_ID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^.*(?=.{10,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage = ("Password is not valid"))]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]             
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = ("Error Comparing Password"))]
        public string ComparePassword { get; set; }

    }
}