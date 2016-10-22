using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            bool sess1 = (bool)Session["LoggedIn"];
            Session["LoggedIn"] = false;
            bool sess2 = (bool)Session["LoggedIn"];
            Session["UserId"] = null;
            Debug.WriteLine("---------------------LOGOUT-------------------------------: sess1:" + sess1.ToString() + " sess2: " + sess2.ToString());
            return RedirectToAction("LoginBirth", "Home", new { area = "" });
        }
    }
}