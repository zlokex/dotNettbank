using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbank.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginBankID()
        {
            return View();
        }

        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult AccountStatement()
        {
            return View();
        }

        public ActionResult PaymentInsert()
        {
            return View();
        }
    }
}