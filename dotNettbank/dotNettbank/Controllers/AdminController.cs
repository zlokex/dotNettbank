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
            // If fields does not pass validation:
            if (!ModelState.IsValid)
            {
                // reload view:
                return View();
            }

            /*
            // Make sure that repeated password matches password:
            if (regCustomer.Password != regCustomer.PasswordRepeat)
            {
                return View();
            }
            */

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