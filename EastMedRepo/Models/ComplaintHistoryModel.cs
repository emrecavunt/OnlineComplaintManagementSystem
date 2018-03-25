using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastMedRepo.Models
{
    public class ComplaintHistoryModel
    {
        private readonly List<ComplaintHistoryStatus> statues = new List<ComplaintHistoryStatus>
        {
             new ComplaintHistoryStatus { Id = 1, ComplaintStatus = "On Progress" },
             new ComplaintHistoryStatus { Id = 2, ComplaintStatus = "Solved" },
             new ComplaintHistoryStatus { Id = 3, ComplaintStatus = "UnSolved" }
        };
        public int ID { get; set; }
        public DateTime Modified_Time { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public int? CategoryID { get; set; }
        public int CategoryUser { get; set; }
        public int CompID { get; set; }
        public string CategoryName { get; set; }
        public string ItemOfComplaint { get; set; }
        public string UserName { get; set; }
        public IEnumerable<SelectListItem> StatusChoosen
        {
            get
            {
                return new SelectList(statues, "Id", "ComplaintStatus");
            }

        }
    }
}