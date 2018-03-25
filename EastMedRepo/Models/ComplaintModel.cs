using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EastMedRepo.Models
{
    public class ComplaintModel
    {
        
        public int ComplaintId { get; set; }
        public int? User_ID { get; set; }
        [StringLength(255)]
        public string COMMENT { get; set; }

        [StringLength(45)]
        public string STATUS { get; set; }

        public DateTime? STARTDATE { get; set; }

        public int? PRIORITY { get; set; }

        public string UserName { get; set; }

        public string  CategoryName { get; set; }

        public bool IsActive { get; set; }

        public string RoomNo { get; set; }

        public  string itemName { get; set; }

        public string ITEM_ID { get; set; }

        public string ImgUrl { get; set; }

        public virtual category category { get; set; }

        public virtual ICollection<complaint_history> complaint_history { get; set; }

        public virtual user user { get; set; }

        

        public ComplaintModel()
        {
            STARTDATE = DateTime.Now;
        }
    }
}