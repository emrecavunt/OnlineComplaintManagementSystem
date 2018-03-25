using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EastMed.Data.Model;
using PagedList;
using EastMedRepo.CustomFilters;

namespace EastMedRepo.Controllers
{
    [Authorize]
    [LoginFilter]
    public class DepartmantsController : AplicationBaseController
    {
        private EastMedDB db = new EastMedDB();

        // GET: Departmants
        // Index For Department table
        public ActionResult Index(int? page)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            var departmants = (from d in db.departmant
                               select d).OrderByDescending(x => x.DEPT_NAME);
            int pageSized = 5;
            int pageNumberd = (page ?? 1);
            return View(departmants.ToPagedList(pageNumberd, pageSized));            
        }

        // Details for Department
        // GET: Departmants/Details/5
        public ActionResult Details(int? id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departmant departmant = db.departmant.Find(id);
            if (departmant == null)
            {
                return HttpNotFound();
            }
            return View(departmant);
        }

        // GET: Departmants/Create
        public ActionResult Create()
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            var SessionSuperAdmin = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            if(SessionSuperAdmin != 1000000000)
            {
                return RedirectToAction("Index", "Location");
            }
            return View();
        }

        // POST: Departmants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DEPT_NAME,DEPT_ID,IsActive")] departmant departmantModel)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
           
            //departmant dbDept = new departmant();
            try { 
            if(db.departmant.Any(x=> x.DEPT_ID == departmantModel.DEPT_ID))
            {
                TempData["info"] = departmantModel.DEPT_ID + " " +"Department already created!";
                return RedirectToAction("Create", "Departmants");
            }
            if (ModelState.IsValid)
            {               
                db.departmant.Add(departmantModel);
                db.SaveChanges();
                TempData["info"] = "Department Created Succesfull";
                return RedirectToAction("Index","Location");
            }
            }
            catch(Exception ex)
            {
                TempData["info"] = "Problem Occured: " + " " + ex.Message;
                return RedirectToAction("Index", "Location");
            }


            return View(departmantModel);
        }
       
        // GET: Departmants/Edit/5
        public ActionResult Edit(int? id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            var SessionSuperAdmin = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (SessionSuperAdmin != 1000000000)
            {
                return RedirectToAction("Index", "Location");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departmant departmant = db.departmant.Find(id);
            if (departmant == null)
            {
                return HttpNotFound();
            }
            return View(departmant);
        }

        // POST: Departmants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken] // for avoid the anti forgery keys attacks.
        public ActionResult Edit([Bind(Include = "ID,DEPT_NAME,DEPT_ID,IsActive")] departmant departmant)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            //Check if model state which is department model from requested result and validations are true then edit else return the current page
            if (ModelState.IsValid)
            {
                db.Entry(departmant).State = EntityState.Modified;
                db.SaveChanges();
                TempData["info"] = "Department Edit Succesfully";
                return RedirectToAction("Index","Location");
            }
            return View(departmant);
        }

        // GET: Departmants/Delete/5
        public ActionResult Delete(int? id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            var SessionSuperAdmin = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (SessionSuperAdmin != 1000000000)
            {
                return RedirectToAction("Index","Location");
            }
            // Request id is null then bad request error appear 400
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departmant departmant = db.departmant.Find(id);
            if (departmant == null)
            {
                return HttpNotFound();
            }
            return View(departmant);
        }

        // POST: Departmants/Delete/5
        // Posting Deletion to server to accept without forgery.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var SessionControl = Convert.ToInt32(HttpContext.Session["RoleID"]);
            if (SessionControl != 5)
            {
                return RedirectToAction("Index", "Home");
            }
            try { 
            departmant departmant = db.departmant.Find(id);
            db.departmant.Remove(departmant);
            db.SaveChanges();
            TempData["info"] = "Department Deleted Succesfully";
                if (departmant.location.Any())
                {

                }
            return RedirectToAction("Index","Location");
            }
            catch ( Exception Ex)
            {
                TempData["info"] = "There is a problem occured" + " " + Ex.Message;
                return RedirectToAction("Delete", "Departmants",new { id = id });
            }
        }
        // Closes the all connection with database.
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
