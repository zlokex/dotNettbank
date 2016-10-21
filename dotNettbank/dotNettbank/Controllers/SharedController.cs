using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbank.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult Logout()
        {
            Session["LoggedIn"] = false;
            Session["UserId"] = null;
            return RedirectToAction("LoginBirth", "Home", new { area = "" });
        }
    }
}