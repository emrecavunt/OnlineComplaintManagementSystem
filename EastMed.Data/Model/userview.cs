namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("eastmed.userview")]
    public partial class userview
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UNI_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(255)]
        public string FIRST_NAME { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(255)]
        public string LAST_NAME { get; set; }

        [StringLength(45)]
        public string TITLE { get; set; }

        [StringLength(45)]
        public string PHONE { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(45)]
        public string EMAIL { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(80)]
        public string PASSWORD { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FK_PRIVILEGE_ID { get; set; }

        public int? FK_LOCATION_ID { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool IsActive { get; set; }

        public DateTime? LAST_LOGINDATE { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public DateTime? UPDATED_DATE { get; set; }
    }
}
