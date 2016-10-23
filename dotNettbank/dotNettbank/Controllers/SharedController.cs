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
            Session["LoggedIn"] = false;
            Session["UserId"] = null;
            //Debug.WriteLine("---------------------LOGOUT-------------------------------);
            return RedirectToAction("LoginBirth", "Home", new { area = "" });
        }
    }
}