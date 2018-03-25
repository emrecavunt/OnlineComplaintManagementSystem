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

namespace EastMedRepo.Controllers
{
   [LoginFilter]
    [Authorize]
    public class categoriesController : AplicationBaseController
    {
        private EastMedDB db = new EastMedDB();

        // GET: categories
        public ActionResult Index()
        {
            var category = db.category.Include(c => c.user);
            return View(category.ToList());
        }

        // GET: categories/Details/5
       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: categories/Create
        public ActionResult Create()
        {
            ViewBag.FK_USER_ID = new SelectList(db.user.Where(x => x.IsActive == true && x.FK_PRIVILEGE_ID == 7), "ID", "FIRST_NAME", "LAST_NAME");
            return View();
        }

        // POST: categories/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CATEGORY_NAME,DESCRIPTION,FK_USER_ID,IsActive")] category category)
        {
            try
            {

           
            if (ModelState.IsValid)
            {
                db.category.Add(category);
                db.SaveChanges();
                    TempData["info"] = "Adding category Succesful";
                return RedirectToAction("Index");
            }
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            ViewBag.FK_USER_ID = new SelectList(db.user.Where(x => x.IsActive == true && x.FK_PRIVILEGE_ID == 7), "ID", "FIRST_NAME", "LAST_NAME", category.FK_USER_ID);
            return View(category);
        }

        // GET: categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.FK_USER_ID = new SelectList(db.user.Where(x => x.IsActive == true && x.FK_PRIVILEGE_ID == 7 ), "ID", "FIRST_NAME", category.FK_USER_ID);
            return View(category);
        }

        // POST: categories/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CATEGORY_NAME,DESCRIPTION,FK_USER_ID,IsActive")] category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                TempData["info"] = "Category Editing Succesfully";
                return RedirectToAction("Index");
            }
            ViewBag.FK_USER_ID = new SelectList(db.user, "ID", "FIRST_NAME", category.FK_USER_ID);
            return View(category);
        }
        // If you want to delete category you have to make sure System and Technical database category table ID otherwise it will be cause a problem in dashboard controller,
        //which is Fk_CATEGORY_ID = 6 for System , FK_CATEGORY_ID = 7 for Technical
        // GET: categories/Delete/5
        public ActionResult Delete(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {            
            category category = db.category.Find(id);
                if( category.ID == 6 || category.ID == 7  )
                {
                    TempData["error"] = "Cannot delete"+ " "  + category.CATEGORY_NAME +" "+"Category";
                    return RedirectToAction("Delete", "categories", new { id = id });
                }
                if(category.complaint.Any())
                {
                    TempData["error"] = "You Cannot delete any category which is have a complaint already!. Try to Edit Category name or Responsible user";
                    return RedirectToAction("Delete", "categories", new { id = id });
                }
            db.category.Remove(category);
            db.SaveChanges();
            TempData["info"] = "Deleting Category Successful!";
            return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            TempData["info"] = "Deleting Category Successful!";
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
