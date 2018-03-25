using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EastMed.Data.Model;
using EastMedRepo.CustomFilters;
using EastMed.Core.Infrastructure;
using EastMedRepo.Models;

namespace EastMedRepo.Controllers
{
    [LoginFilter]
    [Authorize]
    public class MaintanenceComplaintController : AplicationBaseController
    {
        private EastMedDB db = new EastMedDB();
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IComplaintRepository _complaintRepository;
        public MaintanenceComplaintController(IComplaintRepository complaintrepository, IUserRepository userRepository, IRoleRepository roleRepository, ILocationRepository locationRepository, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
            _complaintRepository = complaintrepository;
        }
        // GET: MaintanenceComplaint
        [LoginFilter]
        public ActionResult Index()
        {
            int sessionRoleControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            var sessionControl = Convert.ToString(HttpContext.Session["UserID"]);
            if (sessionRoleControl != 7)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {


                try
                {
                    var SessionControl = Convert.ToInt32(HttpContext.Session["UserDatabaseID"]);
                    int UserDBID = Convert.ToInt32(SessionControl);
                    complaint modal = new complaint();
                    var dbComplaint = (from d in db.complaint
                                       where d.category.FK_USER_ID == SessionControl & d.STATUS.Trim().ToUpper() != "SOLVED" & d.STATUS.Trim().ToUpper() != "UNSOLVED"
                                       select d).OrderByDescending(x => x.START_DATE);
                    var complianttoStatusNew = (from d in db.complaint
                                                where d.category.FK_USER_ID == UserDBID
                                                select d.STATUS).ToList();


                    return View(dbComplaint.ToList());
                }
                catch (Exception ex)
                {
                    TempData["error"] = "error occured while listing complaint please try again! if problem occur again contact your administrator!";
                    return RedirectToAction("Index", "Home");
                }

            }



        }

        // GET: MaintanenceComplaint/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complaint complaint = db.complaint.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }
        #region Autocomplate textbox for room type 
        private List<Autocomplete> _GetStatus(string query)
        {
            List<Autocomplete> status = new List<Autocomplete>();
            try
            {
                var rs = db.complaint_history.Where(p => p.STATUS.Contains(query)).Select(p => new { p.STATUS }).Distinct().ToList();
                //var results = (from p in db.complaint_history
                //               where (p.STATUS).Contains(query)
                //               orderby p.STATUS
                //               select p.STATUS ).ToList().Distinct();
                //Select(w => new { name = w.STATUS, Id = w.ID }).Distinct();
                foreach (var r in rs)
                {
                    // create objects
                    Autocomplete statuss = new Autocomplete();

                    statuss.Name = r.STATUS;
                    // statuss.Id = r.Id;
                    status.Add(statuss);
                }

            }
            catch (Exception ex)
            {

            }

            return status;
        }
        public ActionResult GetStatus(string query)
        {
            return Json(_GetStatus(query), JsonRequestBehavior.AllowGet);
        }
        #endregion
        // GET: MaintanenceComplaint/Create
        public ActionResult Create(int? id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 7)
            {
                TempData["warning"] = "Un Authorized Action";
                return RedirectToAction("Index", "Home");
            }
            int sessionControl = Convert.ToInt32(HttpContext.Session["UserDatabaseID"]);
            complaint model = db.complaint.Find(id);
            ComplaintHistoryModel cmhmodel = new ComplaintHistoryModel()
            {
                CategoryID = model.FK_CATEGORY_ID,
                CompID = model.ID,
                CategoryUser = sessionControl,
            };
            var query = db.complaint_history.Distinct().Select(x => x.STATUS);
            ViewBag.StatusList = GetStatus();
            var categoryList = db.category.ToList();
            ViewBag.FK_CATEGORY_ID = categoryList;
            
            var types = new List<ComplaintHistoryStatus>();
            types.Add(new ComplaintHistoryStatus() { Id = 0, ComplaintStatus = "On Progress" });
            types.Add(new ComplaintHistoryStatus() { Id = 1, ComplaintStatus = "Solved" });
            types.Add(new ComplaintHistoryStatus() { Id = 2, ComplaintStatus = "UnSolved" });
            ViewBag.PartialTypes = types;
            return View(cmhmodel);
        }
        private List<SelectListItem> GetStatus()
        {
            List<SelectListItem> statuses = new List<SelectListItem>();
            statuses.Add(new SelectListItem { Value = "1", Text = "On Progress" });
            statuses.Add(new SelectListItem { Value = "2", Text = "Solved" });
            statuses.Add(new SelectListItem { Value = "3", Text = "Unsolved" });
            return statuses;
        }
        private IEnumerable<string> GetCompHistStatus()
        {
            return new List<string>
            {
                "On progress",
                "Solved",
                "UnSolved"
            };
        }
        private IEnumerable<SelectListItem> GetSelectStatus(IEnumerable<string> elements)
        {
            var selectlist = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectlist.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }
            return selectlist;
        }

        // POST: MaintanenceComplaint/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ComplaintHistoryModel model, int? id)
        {
            // complaint status
            string statusc = "";
            int sessionControl = Convert.ToInt32(HttpContext.Session["UserDatabaseID"]);
            complaint_history comphist = new complaint_history();
           
            int quote = (int)id;
            complaint complaint = db.complaint.Find(id);    
                 
            if (model != null)
            {
                try
                {

                
                if (Convert.ToInt32(model.Status) == 0)
                {
                    comphist.STATUS = statusc = "On Progress".Trim().ToUpper();
                }
                if (Convert.ToInt32(model.Status) == 1)
                {
                    comphist.STATUS = statusc = "SOLVED".Trim().ToUpper();
                }
                else if(Convert.ToInt32(model.Status) == 2)
                {
                    comphist.STATUS = statusc = "UNSOLVED".Trim().ToUpper();
                }

                comphist.MODIFIED_TIME = DateTime.Now;
                comphist.COMMENT = model.Comment;
                comphist.FK_CATEGORYUSER_ID = sessionControl;
                comphist.FK_COMPLAINT_ID = quote;
                comphist.FK_CATEGORY_ID = model.CategoryID;
                db.complaint_history.Add(comphist);               
                db.SaveChanges();
                    if (Convert.ToInt32(model.Status) == 0)
                    {
                        complaint.STATUS = statusc = "ON PROGRESS".Trim().ToUpper();
                    }
                    if (Convert.ToInt32(model.Status) == 1)
                    {
                        complaint.STATUS = statusc = "SOLVED".Trim().ToUpper();
                    }
                    else if(Convert.ToInt32(model.Status) == 2)
                    {
                        complaint.STATUS = statusc = "UNSOLVED".Trim().ToUpper();
                    }
                    db.complaint.Attach(complaint);
                    var entry = db.Entry(complaint);
                    entry.Property(x => x.STATUS).IsModified = true;
                    db.SaveChanges();
                    TempData["Info"] = "Complaint " + comphist.ID + " of status successfuly changed to " + comphist.STATUS;
                return RedirectToAction("Index");
                }
                catch
                {
                    TempData["error"] = "Error occured while modifying complaint";
                    return View(model);
                }
            }
            var types = new List<ComplaintHistoryStatus>();
            types.Add(new ComplaintHistoryStatus() { Id = 0, ComplaintStatus = "On Progress" });
            types.Add(new ComplaintHistoryStatus() { Id = 1, ComplaintStatus = "Solved" });
            types.Add(new ComplaintHistoryStatus() { Id = 2, ComplaintStatus = "UnSolved" });
            ViewBag.PartialTypes = types;
            ViewBag.FK_CATEGORY_ID = new SelectList(db.category, "ID", "CATEGORY_NAME", complaint.FK_CATEGORY_ID);
            return View(model);
        }

        // GET: MaintanenceComplaint/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int quoteint = (int)id;
            complaint complaint = db.complaint.Find(id);
            ViewBag.StatusList = GetStatus();
            int sessionControl = Convert.ToInt32(HttpContext.Session["UserDatabaseID"]);
            ComplaintHistoryModel model = new ComplaintHistoryModel()
            {
                CompID = quoteint,
                CategoryUser = sessionControl,
            };
            if (complaint == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_CATEGORY_ID = new SelectList(db.category, "ID", "CATEGORY_NAME", complaint.FK_CATEGORY_ID);
            ViewBag.FK_ITEM_ID = new SelectList(db.item, "ID", "ITEM_NAME", complaint.FK_ITEM_ID);
            ViewBag.FK_Location_ID = new SelectList(db.location, "ID", "ROOM_ID", complaint.FK_Location_ID);
            ViewBag.FK_USER_ID = new SelectList(db.user, "ID", "FIRST_NAME", complaint.FK_USER_ID);
            return View(model);
        }

        // POST: MaintanenceComplaint/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ComplaintHistoryModel model, int? id)
        {
           
            return View(model);
        }

        // GET: MaintanenceComplaint/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complaint complaint = db.complaint.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }

        // POST: MaintanenceComplaint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            complaint complaint = db.complaint.Find(id);
            db.complaint.Remove(complaint);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
