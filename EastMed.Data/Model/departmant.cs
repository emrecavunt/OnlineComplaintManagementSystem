namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Net.Http;
    // fill the object from the deparment table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.departmant")]
    public partial class departmant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public departmant()
        {
            location = new HashSet<location>();
        }

        public int ID { get; set; }
        [Display(Name ="Department Name")]
        [Required(ErrorMessage ="Department Name Required")]
        [StringLength(150,ErrorMessage ="Department Name Cannot be more than 150 character!.")]
        public string DEPT_NAME { get; set; }
        [Display(Name ="Department ID")]
        [Required(ErrorMessage ="Department Id Required")]
        public int DEPT_ID { get; set; }

        public bool IsActive { get; set; }
        // Many to one connection to location table
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<location> location { get; set; }
    }
}
