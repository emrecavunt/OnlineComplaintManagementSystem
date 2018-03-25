namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    // fill the object from the Complaint History table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.complaint_history")]
    public partial class complaint_history
    {
        public int ID { get; set; }
        [Display(Name = "Modified Time")]
        public DateTime MODIFIED_TIME { get; set; }
        [Display(Name = "Comment")]
        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string COMMENT { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage =("Status Required"))]
        [StringLength(45)]
        public string STATUS { get; set; }

        public int FK_COMPLAINT_ID { get; set; }

        public int? FK_CATEGORYUSER_ID { get; set; }

        public int? FK_CATEGORY_ID { get; set; }
        // One to many connection to complaint table
        public virtual complaint complaint { get; set; }
    }
}
