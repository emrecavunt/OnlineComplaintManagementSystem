namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    // fill the object from the privilege table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.privilege")]
    public partial class privilege
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public privilege()
        {
            user = new HashSet<user>();
        }

        public int ID { get; set; }

        [StringLength(120)]
        public string ROLE { get; set; }

        public bool IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> user { get; set; }
    }
}
