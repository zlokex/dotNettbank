using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbank.Controllers
{
    //HomeController har ansvar for hjemmesiden før innlogging.
    public class HomeController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
    }
}