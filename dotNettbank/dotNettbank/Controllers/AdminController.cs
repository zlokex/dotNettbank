using dotNettbank.BLL;
using dotNettbank.Model;
using dotNettbank.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // If fields does not pass validation:
            if (!ModelState.IsValid)
            {
                // reload view:
                return View();
            }

            // Generate salt and create hashed password from salt
            string salt = BankService.generateSalt();
            var passwordAndSalt = regCustomer.Password + salt;
            byte[] passwordDB = BankService.createHash(passwordAndSalt);

            // Create new PostalArea Domain model:
            PostalArea postalArea = new PostalArea()
            {
                Area = regCustomer.PostalArea,
                PostCode = regCustomer.PostCode
            };
            
            // Add postal area to PostalAreas table in DB:
            bankService.addPostalArea(postalArea);

            // Create new customer domain model:
            Customer customer = new Customer()
            {
                BirthNo = regCustomer.BirthNo,
                FirstName = regCustomer.FirstName,
                LastName = regCustomer.LastName,
                Address = regCustomer.Address,
                PhoneNo = regCustomer.PhoneNo,
                PostCode = regCustomer.PostCode,
                //PostalArea = postalArea,
                Password = passwordDB,
                Salt = salt
            };

            if (bankService.registerCustomer(customer))
            {
                // If succesfull:
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
            else
            {
                // If not successfull:
                return View();
            }
        }




        [AllowAnonymous]
        public JsonResult CheckExistingBirthNo(string BirthNo)
        {
            Debug.WriteLine("---------------------------DEBUG---------" + BirthNo);
            bool ifBirthNoExists = false;
            try
            {
                ifBirthNoExists = IsBirthNoExists(BirthNo) ? true : false;
                return Json(!ifBirthNoExists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        private bool IsBirthNoExists(string birthNo)
        {
            var customer = bankService.getCustomerByBirthNo(birthNo);
            Debug.WriteLine("---------------------------DEBUG---------" + customer);
            if (customer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}