using EastMed.Core.Infrastructure;
using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EastMedRepo.Models;
using System.Data;
using System.Data.Entity;
using EastMedRepo.CustomFilters;
using EastMedRepo.Class;

namespace EastMedRepo.Controllers
{
    [LoginFilter]
    [Authorize]
    public class ComplainantUserController : AplicationBaseController
    {
        #region DB Configration
        // Database inialization
        private EastMedDB db = new EastMedDB();
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IComplaintRepository _complaintRepository;
        public ComplainantUserController(IComplaintRepository complaintrepository, IUserRepository userRepository, IRoleRepository roleRepository, ILocationRepository locationRepository, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
            _complaintRepository = complaintrepository;
        }
        #endregion
        // GET: ComplainantUser
        public ActionResult Index()
        {
            int sessionRoleControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            string sessionControl = Convert.ToString(HttpContext.Session["UserID"]);
            int SessionUserControl = Convert.ToInt32(Session["UserDatabaseID"]);
            if (sessionControl == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var ComplainantExist = _userRepository.GetMany(x => x.FK_PRIVILEGE_ID == sessionRoleControl && x.IsActive == true && x.UNI_ID == sessionControl);
                if (Convert.ToInt32(sessionRoleControl) == 8)
                {
                    //var categories = db.user.Where(x => x.FK_USER_ID == maintanenceuserisexist && x.IsActive == true).SingleOrDefault();
                    //var complaint = db.complaint.Where(x => x.FK_USER_ID == sessionControl);
                    
                    var complaint = db.complaint.Include(c => c.category).Include(c => c.item).Include(c => c.location).Include(c => c.user ).Where(c => c.FK_USER_ID == SessionUserControl);
                    var query = (from comp in db.complaint
                                 join u in db.user on comp.FK_USER_ID equals u.ID
                                 join cate in db.category on comp.FK_CATEGORY_ID equals cate.ID
                                 join loc in db.location on comp.FK_Location_ID equals loc.ID
                                 join i in db.item on comp.FK_ITEM_ID equals i.ID
                                 join it in db.itemtype on i.FK_ITEMTYPE_ID equals it.ID
                                 //where u.ID == SessionUserControl
                                 orderby comp.ID descending
                                 select new ComplaintModel
                                 {
                                     User_ID = comp.FK_USER_ID,
                                     STATUS = comp.STATUS,
                                     COMMENT = comp.COMMENT,
                                     UserName = u.FIRST_NAME + " " + u.LAST_NAME,
                                     ITEM_ID = comp.ITEM_ID,
                                     IsActive = comp.IsActive,
                                     ComplaintId = comp.ID,
                                     CategoryName = cate.CATEGORY_NAME,
                                     PRIORITY = comp.PRIORITY,
                                     STARTDATE = comp.START_DATE,
                                     itemName = i.ITEM_NAME,
                                     RoomNo = loc.ROOM_ID,
                                     ImgUrl = comp.ImgURL


                                 }).ToList();
                    return View(query);
                }
                else
                {
                    ViewBag.Message = "Unauthorized Action";                                      
                    return View();
                }
            }
          
        }

        // GET: Complaint/Create
        public ActionResult CreateComplaint()
        {
            SetCategory();
            //ViewBag.Itemlist = new SelectList(GetItemType(), "ID", "Item_Type");
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");

            return View();
        }

        // POST: Complaint/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComplaint(complaint complaint, int FK_CATEGORY_ID, HttpPostedFileBase ComplaintImg)
        {
            var SessionDB = Convert.ToInt32(Session["UserDatabaseID"]);
            // Control the which user Login
            var SessionControl = HttpContext.Session["UserID"];
            // Select the item name if there is already complaint about
            var itemname = db.item.Where(x => x.ID == complaint.FK_ITEM_ID).Select(x => x.ITEM_NAME).Single();
            // İf item Exist by name on database eg. Computer 9 
            var itemExist = _complaintRepository.GetMany(x => x.IsActive == true && x.ITEM_ID.Trim().ToUpper() == (itemname + " " + complaint.ITEM_ID).ToUpper().Trim());
            if (complaint != null)
            {
                try
                {
                    user User = _userRepository.GetById(Convert.ToInt32(SessionControl));
                    complaint.FK_CATEGORY_ID = FK_CATEGORY_ID;
                    if (ComplaintImg != null)
                    {
                        string FileName = Guid.NewGuid().ToString().Replace("-", "");
                        string Path = System.IO.Path.GetExtension(Request.Files[0].FileName);
                        string FullPath = "/External/ComplaintImg/" + FileName + Path;
                        Request.Files[0].SaveAs(Server.MapPath(FullPath));
                        complaint.ImgURL = FullPath;
                    }
                    else
                    {
                        complaint.ImgURL = null;
                    }
                    complaint.START_DATE = DateTime.Now;
                    complaint.STATUS = "New";
                    complaint.FK_USER_ID = SessionDB;
                    complaint.IsActive = true;
                    if (itemExist != null)
                    {
                        complaint.PRIORITY = +1;
                    }
                    else
                        complaint.ITEM_ID = complaint.ITEM_ID;
                    _complaintRepository.Insert(complaint);
                    _complaintRepository.Save();
                    return Json(new ResultJson { Success = true, Message = "Complaint Added Successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new ResultJson { Success = true, Message = "Error Occured While adding Complaint.Try Again!" });
                }
            }

            SetCategory();
            //ViewBag.Itemlist = new SelectList(GetItemType(), "ID", "Item_Type");
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            return View(complaint);
        }

        // GET: Complaint/Edit/5
        public ActionResult Edit(int? id)
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
            SetCategory();
            //ViewBag.Itemlist = new SelectList(GetItemType(), "ID", "Item_Type");
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            return View(complaint);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,COMMENT,STATUS,START_DATE,PRIORITY,FK_USER_ID,FK_CATEGORY_ID,IsActive,FK_Location_ID,FK_ITEM_ID,ITEM_ID")] complaint complaint)
        {
            if (ModelState.IsValid)
            {

                db.complaint.Attach(complaint);
                var entry = db.Entry(complaint);
                entry.Property(x => x.COMMENT).IsModified = true;
                entry.Property(x => x.START_DATE).IsModified = true;
                entry.Property(x => x.PRIORITY).IsModified = true;
                entry.Property(x => x.FK_USER_ID).IsModified = true;
                entry.Property(x => x.IsActive).IsModified = true;
                entry.Property(x => x.FK_Location_ID).IsModified = true;
                entry.Property(x => x.FK_ITEM_ID).IsModified = true;
                entry.Property(x => x.ITEM_ID).IsModified = true;
                db.Entry(complaint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCategory();
            //ViewBag.Itemlist = new SelectList(GetItemType(), "ID", "Item_Type");
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            return View(complaint);
        }
        // GET: Complaint/Delete/5
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

        // POST: Complaint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            complaint complaint = db.complaint.Find(id);
            db.complaint.Remove(complaint);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void SetCategory(object category = null)
        {
            var CategoryList = db.category.Where(x => x.IsActive == true).ToList();
            ViewBag.Category = CategoryList;
        }

        public List<departmant> GetDepartmentList()
        {
            List<departmant> departmants = _departmentRepository.GetMany(x => x.IsActive == true).ToList();
            return departmants;
        }

        public ActionResult GetLocationList(int DepartmentID)
        {
            List<location> locationlist = _locationRepository.GetMany(x => x.FK_DEPT_ID == DepartmentID && x.IsActive == true).ToList();
            ViewBag.LocationOptions = new SelectList(locationlist, "ID", "ROOM_ID");
            return PartialView("__LocationOptionPartial");
        }

        public ActionResult GetItemList(int locationID)
        {
            var item = (from l in db.location
                        join lc in db.location_has_item on l.ID equals lc.location_ID
                        join i in db.item on lc.item_ID equals i.ID
                        where l.ID == locationID
                        orderby (i.ITEM_NAME)
                        select new LocationItemVM
                        {
                            iID = l.ID,
                            ID = lc.item_ID,
                            ITEM_NAME = i.ITEM_NAME

                        }).ToList();

            //List<item> itemlist = db.item.Where(x => x.FK_ITEMTYPE_ID == itemtypeID && x.IsActive == true).ToList();

            ViewBag.Itemoptions = new SelectList(item, "ID", "ITEM_NAME");
            return PartialView("_ItemOptionPartial");
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