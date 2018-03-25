namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    // fill the object from the user table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.user")]
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            category = new HashSet<category>();
            complaint = new HashSet<complaint>();
        }

        public int ID { get; set; }
        [Required]
        [Display(Name = "University Id")]
        [StringLength(45, ErrorMessage = ("Title Cannot be more than 45 "))]
        public string UNI_ID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [StringLength(255, ErrorMessage = ("First Name Cannot be more than 255 "))]
        public string FIRST_NAME { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(255, ErrorMessage = ("Last Name be more than 255 "))]
        public string LAST_NAME { get; set; }
        [Display(Name = "Title")]
        [StringLength(45, ErrorMessage = ("Title Cannot be more than 45 "))]
        public string TITLE { get; set; }
        [Display(Name = "Phone")]
        [StringLength(45, ErrorMessage = ("Phone Cannot be more than 45 "))]
        public string PHONE { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,4}")]
        [Required (ErrorMessage = ("Email Ruquired"))]
        [Display(Name = "E-mail")]
        [StringLength(45,ErrorMessage =(" Email Cannot Be more than {1}"))]
        public string EMAIL { get; set; }
        [Display(Name = "Password ")]
        [Required(ErrorMessage =("Password Required"))]
        [StringLength(80,ErrorMessage =("Password must be 80 char Long"))]
        public string PASSWORD { get; set; }
        [Required(ErrorMessage =("Privilege Required"))]
        public int FK_PRIVILEGE_ID { get; set; }        
        public int? FK_LOCATION_ID { get; set; }

        public bool IsActive { get; set; }
        [Display(Name = "Last Login Date")]
        public DateTime? LAST_LOGINDATE { get; set; }
        [Display(Name = "Created Date")]
        public DateTime? CREATED_DATE { get; set; }
        [Display(Name = "Updated Date")]
        public DateTime? UPDATED_DATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<category> category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<complaint> complaint { get; set; }

        public virtual location location { get; set; }

        public virtual privilege privilege { get; set; }
    }
}
