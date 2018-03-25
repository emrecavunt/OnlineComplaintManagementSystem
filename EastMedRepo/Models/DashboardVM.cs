using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EastMedRepo.Models
{
    public class DashboardVM
    {
        private EastMedDB db;
        public DashboardVM()
        {
            db = new EastMedDB();
        }
        public DashboardModelStyle GetModelDashboard()
        {
            DashboardModelStyle models = new DashboardModelStyle();
            models.NoUser = db.user.Count();
            models.SystemComplaint = db.complaint.Where(x => x.FK_CATEGORY_ID == 6).ToList().Count();
            models.TechnicalComplaint = db.complaint.Where(x => x.FK_CATEGORY_ID == 7).ToList().Count();
            models.TotalComplaint = db.complaint.Count();
            models.NoLocation = db.location.Count();
            models.NoDepartment = db.departmant.Count();
            models.TotalSolvedComplaint = db.complaint.Where(x => x.STATUS.Trim().ToUpper() == "SOLVED").Count();
            models.NewComplaint = db.complaint.Where(x => x.STATUS.Trim().ToUpper() == "NEW").Count();
            models.UnsolvedComplaint = db.complaint.Where(x => x.STATUS.Trim().ToUpper() == "UNSOLVED").Count();
            return models;

        }
        public class DashboardModelStyle
        {
            public int NewComplaint { get; set; }           
            public int SystemComplaint { get; set; }
            public int TechnicalComplaint { get; set; }
            public int UnsolvedComplaint { get; set; }
            public int TotalSolvedComplaint { get; set; }
            public int TotalComplaint { get; set; }
            public int NoLocation { get; set; }
            public int NoDepartment { get; set; }
            public int NoUser { get; set; }
            public int NoMaintanenceOfficer { get; set; }

        }

        }
}