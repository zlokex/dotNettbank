using dotNettbank.BLL;
using dotNettbank.Model;
using dotNettbank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbank.Controllers
{
    //AdminController har ansvar for administrative handlinger f.esk legg til eller slette kunde
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
            string password = regCustomer.Password;
            string birthNo = regCustomer.BirthNo;
            string firstName = regCustomer.FirstName;
            string lastName = regCustomer.LastName;
            string address = regCustomer.Address;
            string phoneNo = regCustomer.PhoneNo;
            if (bankService.registerCustomer(password, birthNo, firstName, lastName, address, phoneNo))
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


        public ActionResult OmOss()
        {
            return View();
        }
    }
}