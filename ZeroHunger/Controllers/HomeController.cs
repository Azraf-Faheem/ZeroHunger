using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using ZeroHunger.EF;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel login) {
            
                ZeroHungerEntities db = new ZeroHungerEntities();
                var user = (from u in db.Users
                            where u.username.Equals(login.Username)
                            && u.password.Equals(login.Password)
                            select u).SingleOrDefault();
            var adUser = (from a in db.Admins
                        where a.username.Equals(login.Username)
                        && a.password.Equals(login.Password)
                        select a).SingleOrDefault();

            if (user != null)
                {
                Session["user"] = user;
                    var returnUrl = Request["ReturnUrl"];
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("ResDash", "Collection");
                }
            if (adUser != null)
                {
                    Session["adUser"] = adUser;
                    var returnUrl = Request["ReturnUrl"];
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("AdminDash", "Collection");
                }

            return View(login);
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["cart"] = null;
            Session["adUser"] = null;

            return View("Login");
        }

    }
}