namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    // fill the object from the location_has_item table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.location_has_item")]
    public partial class location_has_item
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
       
        public int location_ID { get; set; }

       
        public int item_ID { get; set; }

        public int ItemQuantity { get; set; }

        public virtual item item { get; set; }

        public virtual location location { get; set; }
    }
}
