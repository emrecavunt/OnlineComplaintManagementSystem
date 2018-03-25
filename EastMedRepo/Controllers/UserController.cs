using EastMed.Core.Infrastructure;
using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

using EastMedRepo.Class;
using EastMedRepo.Models;
using System.Net;
using System.Data;
using System.Data.Entity;
using EastMedRepo.CustomFilters;
using System.Web.Routing;
using System.IO;

namespace EastMedRepo.Controllers
{
    [LoginFilter]
    [Authorize]
    public class UserController : AplicationBaseController
    {
        #region Database 
        string EncryptionKey = "SHA512";
        private readonly EastMedDB db = new EastMedDB();
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository, ILocationRepository locationRepository, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
        }

        #endregion


        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            var UserList = _userRepository.GetAll();
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            return View(UserList.OrderByDescending(x => x.ID));

        }



        #region Add User 

        [HttpGet]
        public ActionResult Add()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            var Users = _userRepository.GetAll();
            SetRoleList();
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");

            return View();

        }
        [HttpPost]
        public JsonResult GetUser(int id)
        {
            user UserReturn = _userRepository.GetById(id);
            if (UserReturn == null)
                return Json(new { Status = 0, Message = "Not found" });

            return Json(new { Status = 1, Message = "Ok", Content = RenderPartialViewToString("_OrderInfo", UserReturn) });
        }
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        // UserVM model is a View Model to more controllable view pages or user requests. 
        // UserVM fill take the data and send them to database user table with Userdb variable.
        // Location Id from cascading dropdown List to catch specific Location. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(UserVM model, int? LocationID)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            SetRoleList();
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            try
            {
                // if Model does not have any validation error 
               if (model != null)
            {
                // Set role List to dropdown list.                        
                var userexist = _userRepository.UserFind(model.UNI_ID);
                if (userexist != null)
                {
                    return Json(new ResultJson { Success = false, Message = userexist.UNI_ID + " User Already Registered!" });
                }                 
                if (string.Compare(model.PASSWORD.Trim().ToUpper(),model.ComparePassword.Trim().ToUpper())!=0)
                    {
                        return Json(new ResultJson { Success = false, Message = " Password should match with compare password field!" });
                    }
                // Fill as user view model user object from the reguested model and match the value to configure them.
                user Userdb = new user();                
                    Userdb.UNI_ID = model.UNI_ID.Trim().ToUpper();
                    Userdb.FIRST_NAME = model.FIRST_NAME;
                    Userdb.LAST_NAME = model.LAST_NAME.ToUpper();
                    Userdb.EMAIL = model.EMAIL;
                    Userdb.FK_LOCATION_ID = LocationID;
                    Userdb.FK_PRIVILEGE_ID = model.FK_PRIVILEGE_ID;
                    Userdb.IsActive = model.IsActive;
                    Userdb.PHONE = model.PHONE;
                    Userdb.PASSWORD = CustomEncrypt.passwordEncrypt(model.PASSWORD.Trim(), EncryptionKey);
                    Userdb.CREATED_DATE = DateTime.Now;
                    Userdb.UPDATED_DATE = DateTime.Now;
                    Userdb.TITLE = model.TITLE;
                    db.user.Add(Userdb);
                    db.SaveChanges();
                    return Json(new ResultJson { Success = true, Message = "User Added Successfully" });
                }                
                return View(model);
            }            
                catch (Exception ex)
                {
                    return Json(new ResultJson { Success = false, Message = "Error Occured while adding User!" });
                }            
        }

        #endregion 





        #region Edit User 
        // Get the user details first to fill textbox or dropdowns in the view.
        [HttpGet]
        public ActionResult Edit(int id, int? LocationID)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
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
                LocationID = LocationID
            };
            if (UserReturn == null)
            {
                return Json(new ResultJson { Success = false, Message = "User Does not Find!" });
            }
            else
            {
                ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
                SetLocationList();
                SetRoleList();
                return View(usermodel);
            }

        }
        // user db to fill as view model from the view page and add to database user table column by columns.
        // Post the action if there is no validate anti forgery token appears.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM User, int? LocationID)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
                SetLocationList();
                SetRoleList();
                if (User != null)
                {
                    user dbUser = _userRepository.GetById(User.ID);
                    dbUser.UNI_ID = User.UNI_ID;
                    dbUser.FIRST_NAME = User.FIRST_NAME;
                    dbUser.LAST_NAME = User.LAST_NAME;
                    dbUser.TITLE = User.TITLE;
                    dbUser.PHONE = User.PHONE;
                    dbUser.EMAIL = User.EMAIL;
                    dbUser.FK_LOCATION_ID = LocationID;
                    dbUser.FK_PRIVILEGE_ID = User.FK_PRIVILEGE_ID;
                    if(string.Compare((User.PASSWORD.Trim()),User.ComparePassword.Trim()) != 0)
                        {
                        return Json(new ResultJson { Success = false, Message = "Password Does not Match!" });
                    }
                    dbUser.PASSWORD = CustomEncrypt.passwordEncrypt(User.PASSWORD.Trim(), EncryptionKey);
                    dbUser.IsActive = User.IsActive;
                    dbUser.UPDATED_DATE = DateTime.Now;
                    _userRepository.Update(dbUser);
                    _userRepository.Save();
                    return Json(new ResultJson { Success = false, Message = "Edit User Succesfull!" });
                }
                else
                {
                    return Json(new ResultJson { Success = false, Message = "User Does not find!" });
                }

            }
            catch (Exception ex)
            {
                return Json(new ResultJson { Success = false, Message = "Error Occured while Editing User!" });
            }

        }
        #endregion
        // Delete user will show up the sweet alert messegae to confirm from client side 
        // will return to json to Delete user View
        // User cannot delete if he/she already login.
        public JsonResult Delete(user User)
        {           
            user dbUser = db.user.Find(User.ID);
            var currentSessionUser = Convert.ToString(HttpContext.Session["UserID"]);
            if (dbUser == null)
            {
                return Json(new ResultJson { Success = false, Message = "User Bulunamadı !" });
            }
            try
            {
                if (currentSessionUser == dbUser.UNI_ID)
                {
                    return Json(new ResultJson { Success = false, Message = "Cannot delete current already Login User !" });
                }
                if( dbUser.FK_PRIVILEGE_ID  == 5 & currentSessionUser != "1000000000")
                {
                    return Json(new ResultJson { Success = false, Message = "You Do not have permission to delete any other admin User !" });
                }
                if ( currentSessionUser == "1000000000" & User.FK_PRIVILEGE_ID ==5)
                {
                    _userRepository.Delete(User.ID);
                    _userRepository.Save();
                    return Json(new ResultJson { Success = true, Message = "Admin User Has been Deleted" });
                }
                var CategoryExist = db.category.Where(x => x.FK_USER_ID == User.ID);
                if(CategoryExist.Any())
                {
                    return Json(new ResultJson { Success = false, Message = "You Can not delete maintanence officers who still registered on a category!" });
                }  
                if(dbUser.complaint.Any())
                {
                    dbUser.IsActive = false;
                    dbUser.UPDATED_DATE = DateTime.Now;
                    db.user.Attach(dbUser);
                    var entry = db.Entry(dbUser);
                    entry.Property(x => x.UPDATED_DATE).IsModified = true;
                    entry.Property(x => x.IsActive).IsModified = true;
                    db.SaveChanges();
                    return Json(new ResultJson { Success = true, Message = "User has already complaint. User IsActive status changed to False!" });
                }
                else
                {
                    _userRepository.Delete(User.ID);
                    _userRepository.Save();
                    return Json(new ResultJson { Success = true, Message = "User Has been Deleted" });
                }                
                
               
            }
            catch (Exception ex)
            {

                return Json(new ResultJson { Success = false, Message = "Error Occured please Try Again!" });
            }
        }




        #region Details User
        // To get the details by id from user repository.
        // id / 5 
        public ActionResult Details(int id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl == 5  || SessionControl ==6 )
            {
               
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user User = _userRepository.GetById(id);
            if (User == null)
            {
                return HttpNotFound();
            }

                return View(User);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        //Get the departmant list and set with ID and departmant name to the list where ever you want the use.
        public ActionResult _LocationDeptDropdownList()
        {
            ViewBag.DepartmentList = new SelectList(GetDepartmentList(), "ID", "DEPT_NAME");
            return View();
        }
        // list the departmant list and return as a list. if they are active.
        public List<departmant> GetDepartmentList()
        {
            List<departmant> departmants = _departmentRepository.GetMany(x => x.IsActive == true).ToList();
            return departmants;
        }
        // Return the partial view to get location list by department id.
        public ActionResult GetLocationList(int DepartmentID)
        {
            List<location> locationlist = _locationRepository.GetMany(x => x.FK_DEPT_ID == DepartmentID && x.IsActive == true).ToList();
            ViewBag.LocationOptions = new SelectList(locationlist, "ID", "ROOM_ID");
            return PartialView("__LocationOptionPartial");
        }


        // Listing Location, Department, Role as an object to fill the dropdown list.with Set Location, Set Department, Set Role List.
        #region Set Location List
        public void SetLocationList(object location = null)
        {
            var LocationList = _locationRepository.GetMany(x => x.IsActive == true).ToList();
            ViewBag.Location = LocationList;
        }
        #endregion
        #region Set Department List
        public void SetDepartmantList(object department = null)
        {

            var DeptList = _departmentRepository.GetMany(x => x.IsActive == true).ToList();
            ViewBag.Department = DeptList;
        }
        #endregion
        #region Set Role List
        public void SetRoleList(object privilege = null)
        {

            var RoleList = _roleRepository.GetMany(x => x.IsActive == true).ToList();
            ViewBag.Role = RoleList;
        }
        #endregion


    }
}