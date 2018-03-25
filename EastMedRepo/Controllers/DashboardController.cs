using EastMedRepo.CustomFilters;
using EastMedRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastMedRepo.Controllers
{
    [Authorize]
    [LoginFilter]
    public class DashboardController : AplicationBaseController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            ViewBag.ActiveUser = HttpContext.Application["ActiveUser"];
            ViewBag.TotalUser = HttpContext.Application["TotalUser"];
            return View(new DashboardVM().GetModelDashboard());
        }

    }

}