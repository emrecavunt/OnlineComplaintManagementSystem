namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    // fill the object from the Category table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.category")]
    public partial class category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public category()
        {
            complaint = new HashSet<complaint>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Category Name Required")]
        [StringLength(155, ErrorMessage = ("Category name max. 155 character!"))]
        [Display(Name ="Category Name")]
        public string CATEGORY_NAME { get; set; }
        [Required(ErrorMessage = "Description Required")]
        [StringLength(255,ErrorMessage = (" Cannot not be more than 250 character long"))]
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Responsible User")]
        //[Required(ErrorMessage = "Responsible User Required")]
        public int? FK_USER_ID { get; set; }

        public bool IsActive { get; set; }

        public virtual user user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<complaint> complaint { get; set; }
    }
}
