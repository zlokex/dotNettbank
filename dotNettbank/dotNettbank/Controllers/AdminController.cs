using dotNettbank.BLL;
using dotNettbank.Models.DomainModels;
using dotNettbank.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbank.Controllers
{

    public class AdminController : Controller
    {
        BankService bankService = new BankService();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterCustomer(RegisterCustomer regCustomer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // Add customer to DB through BLL:
            if (bankService.registerCustomer(regCustomer))
            {
                // If succesfull:
                return RedirectToAction("Index");
            }
            else
            {
                // If not successfull:
                return View();
            }
        }
    }
}