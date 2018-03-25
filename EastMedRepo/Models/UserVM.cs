using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EastMedRepo.Models
{
    public class UserVM
    {
       // below the variables you can see the validation error with using Data.Annotion. 
       // Powerfull and easy to configure. 
        public int ID { get; set; }
        [RegularExpression(@"^[a-z]{0,2}[0-9]{7}$",ErrorMessage =("University ID Must be the format e.g: OG20080234"))]
        [Required(ErrorMessage = "University  ID required")]      
        [Display(Name = ("University ID"))]
        [StringLength(45,ErrorMessage =("University ID most be max lenght 45 , Minumum Length 9"),MinimumLength =9)]
        public string UNI_ID { get; set; }
        [Required(ErrorMessage = "FIRST NAME  required")]
        [StringLength(50, ErrorMessage = ("Last Name maximum {1} character long"))]
        [Display(Name = ("First Name"))]
        public string FIRST_NAME { get; set; }
        [Required(ErrorMessage = "LAST NAME  required")]
        [StringLength(250,ErrorMessage =("Last Name maximum {1} character long"))]
        [Display(Name = ("Last Name"))]
        public string LAST_NAME { get; set; }
        [Required(ErrorMessage = "TITLE required")]
        public string TITLE { get; set; }
         [Display(Name = "Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression("^[0-9]{8}$")]
        [StringLength(18)]
        public string PHONE { get; set; }          
        [RegularExpression(@"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$")]
        [EmailAddress]
        public string EMAIL { get; set; }
        [Required(ErrorMessage = "PASSWORD required")]
        public string PASSWORD { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int FK_PRIVILEGE_ID { get; set; }      
        [Display(Name = "Location")]
        public int? FK_LOCATION_ID { get; set; }        
        public int DepartmentID { get; set; }       
        public int? LocationID { get; set; }
        public Nullable<int> FK_Department_ID { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("PASSWORD",ErrorMessage =("Error Comparing Password"))]
        public string ComparePassword { get; set; }
        public string DEPT_NAME { get; set; }
       // Otomaticaly attach the variable isActive to true when editing table of user.
        public bool ACTIVE = true;
        public bool IsActive
        {
            get
            {
                return ACTIVE;

            }
            set
            {
                ACTIVE = value;
            }
        }      
        public Nullable<System.DateTime> LAST_LOGINDATE { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }       
        public Nullable<System.DateTime> UPDATED_DATE { get; set; }
        // List as IEnumrable the collection of data.
        public  IEnumerable<SelectListItem> category { get; set; }        
        public IEnumerable<SelectListItem> complaint { get; set; }     
        public IEnumerable<SelectListItem> complaint_history { get; set; }
        public virtual location location { get; set; }
        public IEnumerable<SelectListItem>  privilege { get; set; }
       
    }
}