using CaptchaMvc.HtmlHelpers;
using EastMed.Core.Infrastructure;
using EastMed.Data.Model;
using EastMedRepo.CustomFilters;
using EastMedRepo.Helpers;
using EastMedRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace EastMedRepo.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region User 
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public AccountController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        #endregion


        public new ActionResult Profile()
        {
            int UserID = Convert.ToInt32(Session["USERID"]);
            var user = _userRepository.GetMany(x => x.ID == UserID).SingleOrDefault();
            var username = user.FIRST_NAME.ToUpper() + " " + user.LAST_NAME.ToLower();
            ViewBag.deneme = username;

            return View();
        }

        #region Login 
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {                     
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string ReturnUrl = "")
        {
            string EncryptionKey = "SHA512";
            string message = "";
            if (ModelState.IsValid)
            {
                using (EastMedDB db = new EastMedDB())
                {
                    var userexist = db.user.Where(a => a.UNI_ID == model.UNI_ID && a.IsActive == true).FirstOrDefault();
                    if (userexist != null)
                    {
                        if (string.Compare((model.Password.Trim()), CustomDecrypt.passwordDecrypt(userexist.PASSWORD,EncryptionKey)) == 0)
                        {
                            // In here 2 method has been used to save user login atraction to specific pages
                            // Sessions and cookies give as to control menus and specification for each user.
                            // Cookies to used authorized the application and protect to anonymous enter
                            // Cookies are encrypted in client site the avoid from the cookie attacks.

                            Session["RoleID"] = userexist.FK_PRIVILEGE_ID;
                            Session["UserName"] = userexist.FIRST_NAME + " " + userexist.LAST_NAME;
                            Session["UserID"] = userexist.UNI_ID;
                            Session["UserDatabaseID"] = userexist.ID;
                            int timeout = model.RememberMe ? 525600 : 30; // 30 min to expire the cookie.
                            var ticket = new FormsAuthenticationTicket(model.UNI_ID, model.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);                           
                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                userexist.LAST_LOGINDATE = DateTime.Now;
                                db.user.Attach(userexist);
                                var entry = db.Entry(userexist);
                                entry.Property(x => x.LAST_LOGINDATE).IsModified = true;
                                db.SaveChanges();                         
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid user/pass");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid user/pass");
                        return View();
                    }

                }
            }
            ViewBag.Message = message;
            return View();
           
        }
        #endregion
        [AllowAnonymous]
        public ActionResult FortgotPassword()
        {
            return View();
        }

        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            //check user existance
            string user = model.UNI_ID;
            if (user == null)
            {
                TempData["Message"] = "User Not exist.";
            }
            else
            {
                //generate password token
                var token = WebSecurity.GeneratePasswordResetToken(Convert.ToString(user));
                //create url with above token
                var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = user, rt = token }, "http") + "'>Reset Password</a>";
                //get user emailid
                EastMedDB db = new EastMedDB();
                var emailid = (from i in db.user
                               where i.UNI_ID == user
                               select i.EMAIL).FirstOrDefault();
                //send mail
                string subject = "Password Reset Token";
                string body = "<b>Please find the Password Reset Token</b><br/>" + resetLink; //edit it
                if (this.IsCaptchaValid("Validate your captcha"))
                {
                    ViewBag.ErrMessage = "Validation Messgae";
                }
                try
                {
                    EmailHelper.SendEMail(emailid, subject, body);
                    TempData["Message"] = "Mail Sent.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error occured while sending email." + ex.Message;
                }
                //only for testing
                TempData["Message"] = resetLink;
            }

            return View();
        
        }

        #region Logout        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
        #endregion

    }
}