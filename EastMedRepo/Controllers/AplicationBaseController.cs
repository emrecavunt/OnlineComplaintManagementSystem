using EastMed.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastMedRepo.Controllers
{
    public class AplicationBaseController : Controller
    {
        // GET: AplicationBase
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                var context = new EastMedDB();
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.user.SingleOrDefault(u => u.UNI_ID.ToString() == username);
                    string fullName = string.Concat(new string[] { user.FIRST_NAME }); // for full name add this line instead of. string fullName = string.Concat(new string[] { user.FIRST_NAME +" "+ user.LAST_NAME})
                    ViewData.Add("FullName", fullName);
                }
            }
            base.OnActionExecuted(filterContext);
        }

        public AplicationBaseController()
        {

        }
    }
}