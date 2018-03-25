using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EastMedRepo.Models
{
    public class ItemVM
    {
        public int  ID { get; set; }
        public int LocationID { get; set; }
        public int ItemID { get; set; }     
        public int FK_ITEMTYPE_ID { get; set; }
        [Display(Name = "Item Type ") ]
        public string itemtype { get; set; }
        [Display(Name = "Item Quantity")]
        public int ItemQuantity { get; set; }
        [Display(Name = "Item Name ")]
        public string ItemName { get; set; }
        
    }
}