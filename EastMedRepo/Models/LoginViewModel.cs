using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EastMedRepo.Models
{
    public class LoginViewModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "University ID required")]
        [Display(Name = "University ID")]
        public string UNI_ID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "University ID required")]
        [Display(Name = "University ID")]
        public string UNI_ID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name required")]        
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name required")]
        [Display(Name = "Name")]
        public string Surname { get; set; }
    }
}