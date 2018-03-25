using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EastMed.Data.Model;
using System.Data.Entity.Infrastructure;
using PagedList;
using EastMedRepo.Models;
using System.Data.Entity.Core;
using EastMed.Core.Infrastructure;
using EastMedRepo.Class;
using EastMedRepo.CustomFilters;
using EastMedRepo.Controllers;

namespace EastMedRepo.Controllers
{
    //[Authorize(Roles ="Admin")]   
    [LoginFilter]
    [Authorize]
    public class LocationController : AplicationBaseController
    {
        #region database
        private readonly EastMedDB db = new EastMedDB();
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IItemTypeRepository _itemtypeRepository;
        public LocationController(IItemTypeRepository itemrepository ,IUserRepository userRepository, IRoleRepository roleRepository, ILocationRepository locationRepository, IDepartmentRepository departmentRepository)
        {
            _itemtypeRepository = itemrepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
        }
        #endregion


        // GET: Location // This index for Location Table List and include with sorting, searching and pagination from server side.
        // This will improve the speed of loading time and searching time on client side. 
        // 
        public ActionResult Index(string sortOrder, string SearchString, string currentFilter, int? page, int? id, int? ItemID)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if(SessionControl !=5)
            {
                return RedirectToAction("Index", "Home");
            }
            //Sending the current request form the client and add them into the viewbags and we can get the datas easily on front and to show the action easily.
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DepartmentSortParm = String.IsNullOrEmpty(sortOrder) ? "dept_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date"; // 
            ViewBag.RoomSortParm = String.IsNullOrEmpty(sortOrder) ? "room_desc" : ""; // Question mark ( ? )  work as  if else statement coding faster. 
            // if searchString from the request is not null current fillter to default 
            // then searchstring is not null or not empty search ROOM_ID and TYPE in location table.
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString;
            var locations = from s in db.location
                            select s;
            if (!String.IsNullOrEmpty(SearchString))
            {
                locations = locations.Where(s => s.ROOM_ID.Contains(SearchString)
                 || s.TYPE.Contains(SearchString) || s.departmant.DEPT_NAME.Contains(SearchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    locations = locations.OrderByDescending(s => s.ROOM_ID); // case for Rooms to sorting 
                    break;
                case "Date":
                    locations = locations.OrderBy(s => s.CREATED_DATE); // case for Created dates to sorting 
                    break;
                case "date_desc":
                    locations = locations.OrderByDescending(s => s.CREATED_DATE); // case for Created date to sorting 
                    break;
                case "room_desc":
                    locations = locations.OrderByDescending(s => s.TYPE); // case for Type to sorting 
                    break;
                case "dept_desc":
                    locations = locations.OrderByDescending(s => s.FK_DEPT_ID); // case for Type to sorting 
                    break;
                default:
                    locations = locations.OrderByDescending(s => s.CREATED_DATE); // case for Rooms to sorting 
                    break;

            }
            int pageSize = 10; //pageSize for that How many row you want to show in each page 
            int pageNumber = (page ?? 1); // To show the current page and make them as a counter.
            
            return View(locations.ToPagedList(pageNumber, pageSize));
        }

       // Autocomplete for Location edit page to fill the textbox as a list with the type from database.
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {

            EastMedDB entities = new EastMedDB();

            var locationtype = (from location in entities.location
                                where location.TYPE.StartsWith(prefix)                                
                             select new
                             {
                                 label = location.TYPE,
                                 val = location.ID
                             }).Distinct().ToList();
            return Json(locationtype);
        }
        
    

    #region Department Views  

    public ActionResult DepartmentList(int? page)
        {
            var departmants = (from d in db.departmant
                               select d).OrderByDescending(x => x.DEPT_NAME);
            int pageSized = 5;
            int pageNumberd = (page ?? 1);
            return View(departmants.ToPagedList(pageNumberd, pageSized));
        }

        public ActionResult DepartmentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departmant department = db.departmant.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }

            ViewBag.LocationCount = db.departmant.Where(x => x.ID == id).Count();


            return View(department);

        }
        #endregion

        // Autocomplete for Location Add page to fill the textbox as a list with the type from database.

        #region Autocomplate textbox for room type 
        private List<Autocomplete> _GetRoomType(string query)
        {
            List<Autocomplete> room = new List<Autocomplete>();
            try
            {
                var rs = db.location.Where(p => p.TYPE.Contains(query)).Select(p => new { p.TYPE }).Distinct().ToList();
                /**** In here we can use eatier linq or EF to bring data. You can implement it both of them for 
                 *  ****/
                //var results = (from p in db.location
                //               where (p.TYPE).Contains(query)
                //               orderby p.TYPE
                //               select p).ToList();
                foreach (var r in rs)
                {
                    // create objects
                    Autocomplete roomtype = new Autocomplete();

                    roomtype.Name = r.TYPE;
                    //roomtype.Id = r.ID;
                    room.Add(roomtype);
                }

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }

            return room;
        }
        public ActionResult GetRoomType(string query)
        {
            return Json(_GetRoomType(query), JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region Autocomplate for Item Type 
        private List<Autocomplete> _GetItemType(string query)
        {
            List<Autocomplete> item = new List<Autocomplete>();
            try
            {
                var results = (from p in db.itemtype
                               where (p.Item_Type).Contains(query)
                               orderby p.Item_Type
                               select p).Take(10).ToList();
                foreach (var r in results)
                {
                    // create objects
                    Autocomplete itemtypes = new Autocomplete();

                    itemtypes.Name = r.Item_Type;
                    itemtypes.Id = r.ID;
                    item.Add(itemtypes);
                }

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }

            return item;
        }
        public ActionResult GetItemType(string query)
        {
            return Json(_GetItemType(query), JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult AutoCompleteItem(string prefix)
        {

            EastMedDB entities = new EastMedDB();

            var itemname = (from item in db.item                                                  
                         where item.ITEM_NAME.StartsWith(prefix)                         
                         select new 
                         { 
                           label = item.ID,
                           val = item.ITEM_NAME   
                         }).ToList();            
            return Json(itemname);
        }

        // GET: Location/Details/5
        public ActionResult Details(int? id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl == 5 || SessionControl == 6)
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                location location = db.location.Find(id);
                if (location == null)
                {
                    return HttpNotFound();
                }
              
                
                return View(location);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        public ActionResult LocationsItem(int locationID)
        {           
            var query = (from l in db.location
                         join lc in db.location_has_item on l.ID equals lc.location_ID
                         join i in db.item on lc.item_ID equals i.ID
                         where l.ID == locationID
                         orderby (i.ITEM_NAME)
                         select new LocationItemVM
                         {
                             ID = l.ID,
                             iID = lc.item_ID,
                             ITEM_NAME = i.ITEM_NAME,
                             ITEMQUANTITY = lc.ItemQuantity
                         }).ToList();
            return View(query);
        }

        /// <summary>
        ///  Create location with autocomplete reference method
        /// </summary>
        /// <returns></returns>
        #region Create Location

        // GET: Location/Create
        public ActionResult Create()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            ViewBag.FK_DEPT_ID = new SelectList(db.departmant, "ID", "DEPT_NAME");
            //PopulateItemDropdownList();

            return View();
        }

        // POST: Location/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ROOM_ID,TYPE,FK_DEPT_ID,CREATED_DATE,UPDATED_DATE,IsActive")] location location)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            try
            {
                if (db.location.Any(x => x.ROOM_ID.Trim().ToUpper() == location.ROOM_ID.Trim().ToUpper()))
                {
                    TempData["info"] = location.ROOM_ID + " " + " Location already created!";
                    return RedirectToAction("Create", "Location");
                }
                if (ModelState.IsValid)
                {
                    location.ROOM_ID = location.ROOM_ID.Trim().ToUpper();
                    location.TYPE=location.TYPE.Trim().ToLower();
                    location.CREATED_DATE = DateTime.Now;
                    location.UPDATED_DATE = DateTime.Now;
                    db.location.Add(location);
                    db.SaveChanges();
                    TempData["Msg"] = location.ROOM_ID+ " "+ "Location has been saved succeessfully";

                    return RedirectToAction("Index");
                }
            }
            catch (DataException ex) // or use DataException 
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                TempData["info"] = "Error Occured : " + " " + ex.Message;
                return RedirectToAction("Create", "Location");
            }

            ViewBag.FK_DEPT_ID = new SelectList(db.departmant, "ID", "DEPT_NAME", location.FK_DEPT_ID);

            return View(location);
        }

        #endregion


        /// <summary>
        /// Edit location with autocomplete reference method from url id to fill areas from ruquested value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region Edit Location 

        // GET: Location/Edit/5
        public ActionResult Edit(int? id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            location location = db.location.Find(id);
            var SessionDB = Convert.ToInt32(Session["UserDatabaseID"]);
            user UserDb = db.user.Find(SessionDB);
            var AdminControl = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (AdminControl != 1000000000 & SessionControl == 5 & UserDb.location.FK_DEPT_ID != location.FK_DEPT_ID)
            {
                ViewBag.ErrorMessage = "You do not have permission to delete another department location which is not belong your deparment!";
                return View(location);
            }
            if (location == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_DEPT_ID = new SelectList(db.departmant, "ID", "DEPT_NAME", location.FK_DEPT_ID);
            return View(location);
        }


        // POST: Location/Edit/5
        //[Bind(Include = "ID,ROOM_ID,TYPE,FK_DEPT_ID,CREATED_DATE,UPDATED_DATE,IsActive")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(location location)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            if (location == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (db.location.Any(x => x.ROOM_ID.Trim().ToUpper() == location.ROOM_ID.Trim().ToUpper()))
            {
                TempData["info"] = location.ROOM_ID + " " + " Location already created!";
                return RedirectToAction("Create", "Location");
            }
           
          
            if (ModelState.IsValid)
            {

               
                location.UPDATED_DATE = DateTime.Now;               
                db.location.Attach(location);
                var entry = db.Entry(location);
                entry.Property(x => x.ROOM_ID).IsModified = true;
                entry.Property(x => x.UPDATED_DATE).IsModified = true;
                entry.Property(x => x.TYPE).IsModified = true;
                entry.Property(x => x.FK_DEPT_ID).IsModified = true;               
                db.SaveChanges();
                //db.Entry(location).State = EntityState.Modified;
                //db.SaveChanges();
                //_locationRepository.Update(location);
                //_locationRepository.Save();

                TempData["Msg"] = "Location has been updated succeessfully";
                return RedirectToAction("Index");
            }
            ViewBag.FK_DEPT_ID = new SelectList(db.departmant, "ID", "DEPT_NAME", location.FK_DEPT_ID);
            return View(location);
        }

        #endregion

        /// <summary>
        /// Location Delete has savechanges error method to log exception 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saveChangesError"></param>
        /// <returns></returns>
        #region Delete Location
        // GET: Location/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            location location = db.location.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationTotalComplaint = db.complaint.Where(x => x.FK_Location_ID == location.ID).Count();
            ViewBag.UserName = db.user.Where(x => x.FK_LOCATION_ID == id).Select(x => x.FIRST_NAME).FirstOrDefault();
            return View(location);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            var AdminControl = Convert.ToInt32(HttpContext.Session["UserID"]);
            var SessionDB = Convert.ToInt32(Session["UserDatabaseID"]);
            user UserDb = db.user.Find(SessionDB);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            try
            {
                location dblocation = db.location.Find(id);

                //location location = new location() { ID = id, UPDATED_DATE = DateTime.UtcNow, IsActive = false };
                if(dblocation.ID== 23)
                {
                    ViewBag.ErrorMessage = "You do not have permission to delete Super Admin location!";
                    return View(dblocation);
                }
                if (AdminControl != 1000000000 & SessionControl == 5 & UserDb.location.FK_DEPT_ID != dblocation.FK_DEPT_ID)
                {
                    ViewBag.ErrorMessage = "You do not have permission to delete another department location which is not belong your deparment!";
                    return View(dblocation);
                }
                //if(UserDb.location.FK_DEPT_ID != dblocation.FK_DEPT_ID)
                //{
                //    ViewBag.ErrorMessage = "You do not have permission to delete another department location which is not belong your deparment!";
                //    return View(dblocation);
                //}
                if(dblocation.complaint.Any())
                {
                    dblocation.IsActive = false;
                    dblocation.UPDATED_DATE = DateTime.Now;
                    db.location.Attach(dblocation);
                    var entry = db.Entry(dblocation);
                    entry.Property(x => x.UPDATED_DATE).IsModified = true;
                    entry.Property(x => x.IsActive).IsModified = true;
                    db.SaveChanges();
                    ViewBag.ErrorMessage = "Location you want to delete have a complaint already. IsActive status changed to disabled! ";
                    return View(dblocation);
                }
                else
                {
                    db.Entry(dblocation).State = EntityState.Modified;
                    db.location.Remove(dblocation);
                    db.SaveChanges();
                    TempData["Msg"] = "Location of "+ dblocation.ROOM_ID+" "+"has been deleted succeessfully";
                    return RedirectToAction("Index");
                }
               
            }
            catch (DataException)
            {
                
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            
            return RedirectToAction("Index");
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region Add New Item 
        [HttpGet]
        public ActionResult AddItem(int? id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SetItemType();
            var query = (from l in db.location
                         join lc in db.location_has_item on l.ID equals lc.location_ID
                         join i in db.item on lc.item_ID equals i.ID
                         join it in db.itemtype on i.FK_ITEMTYPE_ID equals it.ID
                         where l.ID == id
                         orderby (i.ITEM_NAME)
                         select new ItemVM
                         {
                              ID = lc.ID,                             
                             FK_ITEMTYPE_ID = it.ID,
                             LocationID = l.ID,
                             ItemID = lc.item_ID,
                             ItemName = i.ITEM_NAME,
                             ItemQuantity = lc.ItemQuantity
                         });
            if (query == null)
            {
                return HttpNotFound();
            }
            ViewBag.ITEM = new SelectList(db.item, "ID", "ITEM_NAME");
            ViewBag.ITEMTYPE = new SelectList(db.itemtype, "ID", "Item_Type");
            return View();
        }
        [HttpPost]
        public ActionResult AddItem(ItemVM model, int? id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }          
            ViewBag.ITEMTYPE = new SelectList(db.itemtype, "ID", "Item_Type");
            ViewBag.ITEM = new SelectList(db.item, "ID", "ITEM_NAME");
            int locid = id ?? 0;
            var query = (from l in db.location
                         join lc in db.location_has_item on l.ID equals lc.location_ID
                         join i in db.item on lc.item_ID equals i.ID
                         where l.ID == id
                         orderby (i.ITEM_NAME)
                         select new LocationItemVM
                         {
                             
                             ID = l.ID,
                             iID = lc.item_ID,
                             ITEM_NAME = i.ITEM_NAME,
                             ITEMQUANTITY = lc.ItemQuantity
                         }).ToList();
            //var lcd = db.location_has_item.Where(x => x.location_ID == id);           
            var locationitems = db.location.Where(x => x.ID == id);
           
            try
            {
                model.LocationID = locid;
                if (model != null)
                {
                    item items = new item();                         
                    itemtype itemtypes = new itemtype();
                    location_has_item location_has_items = new location_has_item();                                                                         
                    if (db.location_has_item.Any( x => x.location_ID == id && x.item_ID == items.ID ))
                    {
                        TempData["info"] = "This location already have that item " + locationitems.Select(x => x.ROOM_ID);
                        return RedirectToAction("Index");
                    }
                    else
                    {                        
                        location_has_items.location_ID =locid;
                        location_has_items.item_ID = Convert.ToInt32(model.ItemName);
                        location_has_items.ItemQuantity = model.ItemQuantity;                       
                        db.location_has_item.Add(location_has_items);
                        db.SaveChanges();
                        TempData["info"] = "Adding Item Succesfull ";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["info"] = ex.Message ;
                return RedirectToAction("Index", "Location");
            }
            TempData["info"] = "Adding Item Succesfull";
            ViewBag.ITEMTYPE = new SelectList(db.itemtype, "ID", "Item_Type");
            return View("Index", "Location");
        }
        #endregion
        [HttpGet]
        public ActionResult AddNewItem()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return HttpNotFound();
            }
            ViewBag.ITEMTYPE = new SelectList(db.itemtype, "ID", "Item_Type");
            return View();
        }
        [HttpPost]
        public ActionResult AddNewItem(location_has_item modal)
        {
            try
            {
                

            }
            catch (Exception ex)
            {
                TempData["error"] = "Error Occured while creating a new item.";
                ViewBag.ITEMTYPE = new SelectList(db.itemtype, "ID", "Item_Type");
                return View();
            }
            ViewBag.ITEMTYPE = new SelectList(db.itemtype, "ID", "Item_Type");
            return RedirectToAction("Index");
        }

        #region Delete Item

        
        public JsonResult DeleteItem(location_has_item modal)
        {
            location_has_item dbItem = db.location_has_item.Find(modal.ID);
            var currentSessionUser = Convert.ToString(HttpContext.Session["UserID"]);
            if (dbItem == null)
            {
                return Json(new ResultJson { Success = false, Message = "Item Doesnt Fınd !" });
            }
            try
            {
               
               
                db.location_has_item.Remove(dbItem);
                db.SaveChanges();                   
                    return Json(new ResultJson { Success = true, Message = "Item Deleted Succesfully" });
               
            }
            catch (Exception ex)
            {

                return Json(new ResultJson { Success = false, Message = "Error Occured please Try Again!" });
            }
        }
       
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        public ActionResult LocationEditItem(int? id)
        {
            location_has_item lci = db.location_has_item.Find(id);
            return View(lci);
        }
        [HttpPost]
        public ActionResult LocationEditItem(location_has_item model,int? id)
        {
            location_has_item lci = db.location_has_item.Find(id);
            lci.ItemQuantity = model.ItemQuantity;
            db.location_has_item.Attach(lci);
            var entry = db.Entry(lci);
            entry.Property(x => x.ItemQuantity).IsModified = true;
            db.SaveChanges();
            TempData["info"] = "Item added succesfuly";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult LocationsItem(LocationItemVM model, int locaid, int lhtid)
        {
            item it = db.item.Find(lhtid);
            it.ITEM_NAME = model.ITEM_NAME;


            location_has_item lht = db.location_has_item.Find(locaid);
            lht.ItemQuantity = model.ITEMQUANTITY;
            db.SaveChanges();
            return View(model);
        }
        public void SetItemType(object kategori = null)
        {
            var KategoriList = db.itemtype.Where(x => x.IsActive == true).ToList();
            ViewBag.Kategori = KategoriList;
        }
    }
}
