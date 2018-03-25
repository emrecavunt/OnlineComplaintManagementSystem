namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("eastmed.location")]
    public partial class location
    {
        // fill the object from the location table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public location()
        {
            complaint = new HashSet<complaint>();
            location_has_item = new HashSet<location_has_item>();
            user = new HashSet<user>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage =("Room Id Required"))]
        [Display(Name = ("Room ID"))]
        [StringLength(50,ErrorMessage =("Room Cannot longer than 50 Character"))]
        public string ROOM_ID { get; set; }

        [Required(ErrorMessage =("Type Required"))]
        [StringLength(25,ErrorMessage =("Type cannot longer than 25 Character."))]
        public string TYPE { get; set; }
        [Display(Name = ( " Department "))]
        public int? FK_DEPT_ID { get; set; }
        
        [Display(Name = ("Create Date"))]
        public DateTime? CREATED_DATE { get; set; }
        [Display(Name = ("Update Date"))]
        public DateTime? UPDATED_DATE { get; set; }
        [Display(Name = ("Is Active"))]
        public bool IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<complaint> complaint { get; set; }

        public virtual departmant departmant { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<location_has_item> location_has_item { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> user { get; set; }
    }
}
