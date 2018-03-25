using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EastMedRepo.Models
{
    // Location Item view model for the list and orient the object from the selected data
    public class LocationItemVM
    {
      
            public List<LocationItemVM> lstPartialModel { get; set; }                
        public int ID { get; set; }
        public int iID { get; set; }
        public string ITEM_NAME { get; set; }
        public int ITEMQUANTITY { get; set; }      
        public string itemQuantity { get; set; }
        public string ROOMNO { get; set; }
       
    }
}