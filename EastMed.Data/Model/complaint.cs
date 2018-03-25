namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    // fill the object from the Complaint  table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.complaint")]
    public partial class complaint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public complaint()
        {
            complaint_history = new HashSet<complaint_history>();

        }

        public int ID { get; set; }

        [StringLength(255)]
        public string COMMENT { get; set; }

        [Required]
        [StringLength(45)]
        public string STATUS { get; set; }

        public DateTime START_DATE { get; set; }

        public int PRIORITY { get; set; }

        public int? FK_USER_ID { get; set; }

        public int? FK_CATEGORY_ID { get; set; }

        public bool IsActive { get; set; }

        public int? FK_Location_ID { get; set; }

        public int? FK_ITEM_ID { get; set; }
        [Display(Name = "ITEM ID")]
        [StringLength(150)]      
        [RegularExpression("^[0-9]{0,4}$", ErrorMessage = "Item ID  must be numeric and 4 digit only")]      
        public string ITEM_ID { get; set; }

        [StringLength(255)]
        //[FileExtensions(Extensions =".png|.jpg|.jpeg|.gif",ErrorMessage =("File must be the format of png,jpg,jpeg,gif"))]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        public string ImgURL { get; set; }

        public virtual category category { get; set; }
        // many to one connection to complaint history table  using Icollection virtual object
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<complaint_history> complaint_history { get; set; }
        // One to many connection to user item and location table using virtual
        public virtual user user { get; set; }

        public virtual item item { get; set; }

        public virtual location location { get; set; }

    }
     
    
}
