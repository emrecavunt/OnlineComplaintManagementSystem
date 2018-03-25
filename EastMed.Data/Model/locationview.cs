namespace EastMed.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    //// fill the object from the locationview table in the database by using dataannotions to control them CRUD operations easily. :CRUD ( Create Read Update Delete) 
    [Table("eastmed.locationview")]
    public partial class locationview
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ROOM_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FK_ITEMTYPE_ID { get; set; }

        [StringLength(255)]
        public string Item_Type { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(255)]
        public string ITEM_NAME { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemQuantity { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LocationID { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemID { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime LocationCreatedDate { get; set; }

        public DateTime? LocationUpdateDate { get; set; }
    }
}
