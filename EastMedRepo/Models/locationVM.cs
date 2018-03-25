using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EastMedRepo.Models
{
    public class locationVM 
    {
        
        public int ID { get; set; }

        [Required(ErrorMessage = " Room Required ")]
        [StringLength(255)]
        public string ROOM_ID { get; set; }

        [Required(ErrorMessage = "Type Of the Room required")]
        [StringLength(255)]
        [Range(2,50,ErrorMessage ="Type name must be between 2 and 50 ")]
        public string TYPE { get; set; }
        [Display(Name ="Department")]
        public int? FK_DEPT_ID { get; set; }
        [Display(Name = "Item Name")]
        public int? FK_ITEM_ID { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public DateTime? UPDATED_DATE { get; set; }

        public bool IsActive
        {
            get
            {
                return IsActive;

            }
            set
            {
                IsActive = value;
            }
        }

        
        //public virtual ICollection<complaint> complaint { get; set; }

        public virtual departmant departmant { get; set; }

        public virtual item item { get; set; }

        public virtual ICollection<user> user { get; set; }
    }
}