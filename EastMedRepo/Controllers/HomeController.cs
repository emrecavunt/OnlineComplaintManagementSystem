using EastMed.Core.Infrastructure;
using EastMed.Data.Model;
using EastMedRepo.Class;
using EastMedRepo.CustomFilters;
using EastMedRepo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EastMedRepo.Controllers
{
    [LoginFilter]
    [Authorize]
    public class HomeController : AplicationBaseController
    {
        string EncryptionKey = "SHA512";
        private readonly EastMedDB db = new EastMedDB();
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IComplaintRepository _complaintRepository;
        // From repository to homeController method specific field to fill the repositories from database tables which we declare on Repository pattern.
        public HomeController(IComplaintRepository complaintRepository, IUserRepository userRepository, IRoleRepository roleRepository, ILocationRepository locationRepository, IDepartmentRepository departmentRepository)
        {
            _complaintRepository = complaintRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
        }
        // GET: Home
        [Authorize]
        [LoginFilter]
        public ActionResult Index()
        {
            var SessionControl = HttpContext.Session["UserDatabaseID"];
            int UserDBID = Convert.ToInt32(SessionControl);
            var findUserDepartment = db.user.Where(x => x.ID == UserDBID & x.IsActive == true).Select(x => x.location.departmant.DEPT_ID).SingleOrDefault(); 
            var TotalSolvedComplaint = db.complaint.Where(x => x.FK_USER_ID == UserDBID & x.IsActive == true & x.STATUS.Contains("SOLVED")).Count();
            ViewBag.TotalSolvedComplaint = TotalSolvedComplaint;
            var userComplaintOnProgress = db.complaint.Where(x => x.FK_USER_ID == UserDBID & x.IsActive == true & x.STATUS.Contains("ON PROGRESS")).Count();
            ViewBag.userComplaintOnProgress = userComplaintOnProgress;
            var TotalComplaint = db.complaint.Where(x => x.FK_USER_ID == UserDBID & x.IsActive == true).Count();
            ViewBag.TotalComplaint = TotalComplaint;
            var TotalSeenComplaint = db.complaint.Where(x => x.FK_USER_ID == UserDBID & x.IsActive == true & x.STATUS.ToUpper().Trim() == "SEEN").Count();
            ViewBag.TotalSeenComplaint = TotalSeenComplaint;
            // NewComplaintForCategoryAdmin,totalseenByCategoryAdmin,totalComplaintByCategoryAdmin: for the maintanence offiicer who recieve the complaint by his own category
            var NewComplaintForCategoryAdmin = db.complaint.Where(x => x.category.FK_USER_ID == UserDBID & x.STATUS.ToUpper().Trim() == "NEW").Count();
            ViewBag.NewComplaintForCategoryAdmin = NewComplaintForCategoryAdmin;
            var totalseenByCategoryAdmin = db.complaint.Where(x => x.category.FK_USER_ID == UserDBID & x.location.departmant.DEPT_ID == findUserDepartment & x.STATUS.ToUpper().Trim() == "SEEN").Count();
            ViewBag.totalseenByCategoryAdmin = totalseenByCategoryAdmin;
            var totalComplaintByCategoryAdmin = db.complaint.Where(x => x.category.FK_USER_ID == UserDBID).Count();
            ViewBag.totalComplaintByCategoryAdmin = totalComplaintByCategoryAdmin;

            var MaintanenceOfficerOnProgress = db.complaint.Where(x => x.category.FK_USER_ID == UserDBID & x.STATUS == "ON PROGRESS").Count();
            ViewBag.MaintanenceOfficerOnProgress = MaintanenceOfficerOnProgress;
            var MaintanenceOfficerUnSolved = db.complaint.Where(x => x.category.FK_USER_ID == UserDBID & x.STATUS == "UNSOLVED").Count();
            ViewBag.MaintanenceOfficerUnSolved = MaintanenceOfficerUnSolved;
            var MaintanenceOfficerSolved = db.complaint.Where(x => x.category.FK_USER_ID == UserDBID & x.STATUS == "SOLVED").Count();            
            ViewBag.MaintanenceOfficerSolved = MaintanenceOfficerSolved;



            user UserDb = _userRepository.GetById(UserDBID);
            return View(UserDb);
        }
        [LoginFilter]
        public PartialViewResult RecentComplaint()
        {
            var SessionControl = HttpContext.Session["UserDatabaseID"];
            int UserDBID = Convert.ToInt32(SessionControl);
            //List<complaint> 
            //var dbComplaint = db.complaint.Where(x => x.FK_USER_ID == (Convert.ToInt32(SessionControl)) & x.IsActive == true).ToList().Take(10);
            complaint modal = new complaint();
            var dbComplaint = (from d in db.complaint
                               where d.FK_USER_ID == UserDBID
                               select d).OrderByDescending(x => x.START_DATE);
            
            return PartialView(dbComplaint.ToList().Take(10));
        }
        [LoginFilter]
        public PartialViewResult RecentComplaintMaintanenceOfficer()
        {
            var SessionControl = HttpContext.Session["UserDatabaseID"];
            int UserDBID = Convert.ToInt32(SessionControl);           
            complaint modal = new complaint();
            var dbComplaint = (from d in db.complaint
                               where d.category.FK_USER_ID == UserDBID 
                               select d).OrderBy(x => x.START_DATE);
            return PartialView(dbComplaint.ToList().Take(10));
        }
    
        #region
        [HttpGet]
        public ActionResult Edit(int id)
        {
            user UserReturn = _userRepository.GetById(id);
            UserVM usermodel = new UserVM()
            {
                ID = UserReturn.ID,
                UNI_ID = UserReturn.UNI_ID,
                FIRST_NAME = UserReturn.FIRST_NAME,
                LAST_NAME = UserReturn.LAST_NAME,
                EMAIL = UserReturn.EMAIL,
                TITLE = UserReturn.TITLE,
                PHONE = UserReturn.PHONE,
                FK_PRIVILEGE_ID = UserReturn.FK_PRIVILEGE_ID,
                FK_LOCATION_ID = UserReturn.FK_LOCATION_ID,
                IsActive = UserReturn.IsActive,
                LAST_LOGINDATE = UserReturn.LAST_LOGINDATE,
                CREATED_DATE = UserReturn.CREATED_DATE,
                UPDATED_DATE = UserReturn.UPDATED_DATE,                
            };
            if (UserReturn == null)
            {
                return Json(new ResultJson { Success = false, Message = "User Does not Find!" });
            }
            else
            {
                ViewBag.Role = db.user.Where(x => x.ID == id).Include(x => x.privilege).SingleOrDefault().privilege.ROLE;
                
                return View(usermodel);
            }

        }
        // user db to fill as view model from the view page and add to database user table column by columns.
        // Post the action if there is no validate anti forgery token appears.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM User, int? id)
        {

            try
            {
                ViewBag.Role = db.user.Where(x => x.ID == id).Include(x => x.privilege).SingleOrDefault().privilege.ROLE;
               
                if (ModelState.IsValid)
                {
                    var dbUser = db.user.Where(x => x.ID == id).SingleOrDefault();
                    dbUser.UPDATED_DATE = DateTime.Now;
                    dbUser.PASSWORD = CustomEncrypt.passwordEncrypt(User.PASSWORD, EncryptionKey);
                    dbUser.PHONE = User.PHONE;
                    dbUser.EMAIL = User.EMAIL;
                    db.user.Attach(dbUser);
                    var entry = db.Entry(dbUser);
                    entry.Property(x => x.UPDATED_DATE).IsModified = true;
                    entry.Property(x => x.PASSWORD).IsModified = true;
                    entry.Property(x => x.PHONE).IsModified = true;
                    entry.Property(x => x.EMAIL).IsModified = true;
                    db.SaveChanges();
                    TempData["info"] = "Profile Edit Succesfully";
                  
                    return Json(new ResultJson { Success = false, Message = "Edit User Succesfull!" });
                }
                else
                {
                    //ModelState.AddModelError()
                    return Json(new ResultJson { Success = false, Message = "User Does not find!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new ResultJson { Success = false, Message = "Error Occured while Editing User!" });
            }

        }
#endregion
      
    }
}