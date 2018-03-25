using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EastMed.Data.Model;
using EastMed.Core.Infrastructure;
using EastMedRepo.Models;
using EastMedRepo.Class;
using System.IO;

namespace EastMedRepo.Controllers
{
    public class ComplaintController : AplicationBaseController
    {
        private EastMedDB db = new EastMedDB();

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IComplaintRepository _complaintRepository;

        // private IEnumerable<CategoryViewModel> _categoryViewModels = new List<CategoryViewModel>();
        public ComplaintController(IComplaintRepository repository, IUserRepository userRepository, IRoleRepository roleRepository, ILocationRepository locationRepository, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
            _complaintRepository = repository;
        }



        // GET: Complaint
        //[Authorize(Roles ="Category Admin")]
        public ActionResult Index()
        {
            // query for complaint list with join to table category ,location , item , itemtype , complaint and user
            var query = (from comp in db.complaint
                         join u in db.user on comp.FK_USER_ID equals u.ID
                         join cate in db.category on comp.FK_CATEGORY_ID equals cate.ID
                         join loc in db.location on comp.FK_Location_ID equals loc.ID
                         join i in db.item on comp.FK_ITEM_ID equals i.ID
                         join it in db.itemtype on i.FK_ITEMTYPE_ID equals it.ID

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


            var complaint = db.complaint.Include(c => c.category).Include(c => c.user);

            return View(query);
        }
        
        public ActionResult ComplaintHistory(int? ComplaintId)
        {

            //order by ch.ID;
            var complainthistories = (from ch in db.complaint_history
                                      join cate in db.category on ch.FK_CATEGORY_ID equals cate.ID
                                      join u in db.user on cate.FK_USER_ID equals u.ID
                                      join comp in db.complaint on ch.FK_COMPLAINT_ID equals comp.ID
                                      where comp.ID == ComplaintId
                                      orderby (ch.MODIFIED_TIME)
                                      select new ComplaintHistoryModel
                                      {
                                          ID = ch.ID,
                                          CategoryName = cate.CATEGORY_NAME,
                                          Comment = ch.COMMENT,
                                          ItemOfComplaint = comp.ITEM_ID,
                                          Modified_Time = ch.MODIFIED_TIME,
                                          Status = ch.STATUS,
                                          UserName = u.FIRST_NAME + " " + u.LAST_NAME,

                                      }).ToList();


            return View(complainthistories);
        }

        // GET: Complaint/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complaint complaint = db.complaint.Find(id);
            var queryComplaint = (from comp in db.complaint
                                  join u in db.user on comp.FK_USER_ID equals u.ID
                                  join cate in db.category on comp.FK_CATEGORY_ID equals cate.ID
                                  join loc in db.location on comp.FK_Location_ID equals loc.ID
                                  join i in db.item on comp.FK_ITEM_ID equals i.ID
                                  join it in db.itemtype on i.ID equals it.ID
                                  orderby (cate.CATEGORY_NAME)
                                  where comp.ID == id
                                  select new ComplaintModel
                                  {
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


                                  });
            if (queryComplaint == null)
            {
                return HttpNotFound();
            }
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (complaint.STATUS.Trim().ToUpper() == "NEW" & SessionControl == 7 )
            {            
            complaint.STATUS = "SEEN".Trim().ToUpper();
            db.complaint.Attach(complaint);
            var entry = db.Entry(complaint);
            entry.Property(x => x.STATUS).IsModified = true;
            db.SaveChanges();
            }
            return View(complaint); //Instead of using single return value it is faster and reliable to use it "singleordefault". 
        }

        // GET: Complaint/Create
        public ActionResult Create()
        {
            SetCategory();
            ViewBag.Itemlist = new SelectList(GetItemType(), "ID", "Item_Type");
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");

            return View();
        }

        // POST: Complaint/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(complaint complaint, int FK_CATEGORY_ID, HttpPostedFileBase ComplaintImg)
        {
            // Control the which user Login
            var SessionDB = Convert.ToInt32(Session["UserDatabaseID"]);
            var SessionControl = Convert.ToString(HttpContext.Session["UserID"]);
            // Select the item name if there is already complaint about
            //var itemname = db.item.Where(x => x.ID == complaint.FK_ITEM_ID).Select(x => x.ITEM_NAME).Single();
            // İf item Exist by name on database eg. Computer 9 
            var itemExist = _complaintRepository.GetMany(x => x.IsActive == true && x.ITEM_ID.Trim().ToUpper() == (complaint.ITEM_ID).ToUpper().Trim());
            if (complaint != null)
            {
                try
                {
                    user User = db.user.Where(x => x.UNI_ID == SessionControl).SingleOrDefault();
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
                        complaint.ITEM_ID =  complaint.ITEM_ID;
                    if(complaint.ITEM_ID== null & complaint.ImgURL == null)
                    {
                        return Json(new ResultJson { Success = false, Message = "Item Id and Image fields empty!.At least one of the field necessary!.Please fill out!" });
                    }
                    _complaintRepository.Insert(complaint);
                    _complaintRepository.Save();
                    return Json(new ResultJson { Success = true, Message = "Complaint Added Successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new ResultJson { Success = false, Message = "Error Occured While adding Complaint.Try Again!" });
                }
            }

            SetCategory();
            ViewBag.Itemlist = new SelectList(GetItemType(), "ID", "Item_Type");
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            return View(complaint);
        }
        //only person can edit a complaint who add a complaint!
        // GET: Complaint/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complaint complaint = db.complaint.Find(id);
            var SessionControl = Convert.ToInt32(HttpContext.Session["UserDatabaseID"]);
            if (SessionControl != complaint.FK_USER_ID)
            {
                return RedirectToAction("Index", "Home");
            }
            if (complaint == null)
            {
                return HttpNotFound();
            }
            if (complaint.STATUS.ToUpper().Trim() != "NEW" )
            {
                TempData["warn"] = "This complaint already seen by a maintanence officer,You can not edit or Delete!";
                return View(complaint);
            }
            SetCategory();
            ViewBag.Itemlist = new SelectList(GetItemType(), "ID", "Item_Type");
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            return View(complaint);
        }

        // POST: Complaint/Edit/5
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,COMMENT,STATUS,START_DATE,PRIORITY,FK_USER_ID,FK_CATEGORY_ID,IsActive,FK_Location_ID,FK_ITEM_ID,ITEM_ID")] complaint complaint, HttpPostedFileBase ComplaintImg)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["UserDatabaseID"]);


            if (SessionControl != complaint.FK_USER_ID)
            {
                return RedirectToAction("Index", "Home");
            }
            if(ComplaintImg !=null)
            {
                string FileName = complaint.ImgURL;
                string FilePath = Server.MapPath(FileName);
                FileInfo file = new FileInfo(FilePath);
                if (file.Exists)
                {
                    file.Delete();
                }

                string file_name = Guid.NewGuid().ToString().Replace("-", "");
                string path = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string fullpath = "/External/Haber/" + file_name + path;
                Request.Files[0].SaveAs(Server.MapPath(fullpath));
                complaint.ImgURL = fullpath;
            }
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
                     entry.Property(x => x.ImgURL).IsModified = true;
                db.Entry(complaint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCategory();
            ViewBag.Itemlist = new SelectList(GetItemType(), "ID", "Item_Type");
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            return View(complaint);
        }
        //onyl the person can delete a complaint who added a complaint.
        // GET: Complaint/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            complaint complaint = db.complaint.Find(id);
            var SessionControl = Convert.ToInt32(HttpContext.Session["UserDatabaseID"]);
            if (SessionControl != complaint.FK_USER_ID)
            {
                return RedirectToAction("Index", "Home");
            }
            if (complaint == null)
            {
                return HttpNotFound();
            }
            if(complaint.STATUS.ToUpper().Trim()!="NEW")
            {
                TempData["warn"] = "This complaint already seen or modified by a maintanence officer,You can not edit or Delete!";
                return View(complaint);
            }
            return View(complaint);
        }

        // POST: Complaint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            complaint complaint = db.complaint.Find(id);
            string file_name = complaint.ImgURL;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists) // Check file is exist as physcaly
            {
                file.Delete();
            }
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

        public List<itemtype> GetItemType()
        {
            List<itemtype> itemtypes = db.itemtype.Where(x => x.IsActive == true).ToList();
            return itemtypes;
        }

        public List<LocationItemVM> items(int locationID)
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
            return item;
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
