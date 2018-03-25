namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    // fill the object from the item table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.item")]
    public partial class item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public item()
        {
            complaint = new HashSet<complaint>();
            location_has_item = new HashSet<location_has_item>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "ITEM NAME")]
        public string ITEM_NAME { get; set; }

        public int FK_ITEMTYPE_ID { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<complaint> complaint { get; set; }

        public virtual itemtype itemtype { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<location_has_item> location_has_item { get; set; }
    }
}
