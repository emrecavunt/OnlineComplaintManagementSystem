using EastMed.Data.Model;
using EastMedRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using EastMed.Core.Infrastructure;
using EastMedRepo.CustomFilters;

namespace EastMedRepo.Controllers
{
    [LoginFilter]
    [Authorize]
    public class CategoryAdminController : AplicationBaseController
    {
        // Database initialization
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly EastMedDB db = new EastMedDB();
        public CategoryAdminController(IUserRepository userRepository, IRoleRepository roleRepository, ILocationRepository locationRepository, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
        }
        // GET: CategoryAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Categories()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 6)
            {
                return RedirectToAction("Index", "Home");
            }
            var category = db.category.Include(c => c.user);
            return View(category.ToList());
        }
        public ActionResult ComplaintList()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 6)
            {
                return RedirectToAction("Index", "Home");
            }
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
        public ActionResult UserList()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 6)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var UserList = _userRepository.GetAll();

                return View(UserList.OrderByDescending(x => x.CREATED_DATE));
            }
            catch(Exception Ex)
            {
                TempData["Info"] = "There is a problem occuried refresh the page or contact with admin";
                return View();
            }
          
        }
        // Get list of location table 
        public ActionResult LocationList()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 6)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                // include the database table as a join method
                var location = db.location.Include(l => l.departmant).Include(l => l.location_has_item);
                return View(location.OrderByDescending(x => x.CREATED_DATE));
            }
            
            catch(Exception Ex)
            {
                TempData["error"] = "Error Occured Reload the page if problem occured again please contact your administrator!";                
            }
            return View();
        }
       
    }
}