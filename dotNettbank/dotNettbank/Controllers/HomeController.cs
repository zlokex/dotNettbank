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
    //HomeController har ansvar for hjemmesiden før innlogging.
    public class HomeController : Controller
    {

        BankService bankService = new BankService();

        // GET: Customer
        public ActionResult Index()
        {
            //return View();
            return RedirectToAction("LoginBirth");
        }

        public ActionResult LoginBirth()
        {
            // vis innlogging
            if (Session["LoggedIn"] == null)
            {
                Session["LoggedIn"] = false;
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginBirth(LoginViewModel model)
        {
            //string password = model.Password;
            string birthNo = model.BirthNo;

            TempData["birthNo"] = birthNo;

            // sjekk om personnummer eksisterer blant bruker i databasen
            if (bankService.getCustomerByBirthNo(birthNo) != null)
            {
                // Ja personnummer eksisterer
                // Gå videre til BankId input:
                return RedirectToAction("LoginBankId");
            }
            else
            {
                // Nei personnummer eksisterer ikke. Last siden på nytt.
                Session["LoggedIn"] = false;
                Session["UserId"] = null;
                return View();
            }
        }

        public ActionResult LoginBankId()
        {
            if (Session["LoggedIn"] == null)
            {
                Session["LoggedIn"] = false;
            }
            return View();
        }
        

        [HttpPost]
        public ActionResult LoginBankId(LoginViewModel model)
        {
            /*
            // If birthNo is empty, redirect to LoginBirth:
            if (model.BirthNo == null)
            {
                return RedirectToAction("LoginBirth");
            }
            
            else if (model.BankID == null)
            {
                model.BankID = " "; // Change BankID from null to " " to avoid being stuck in loop.
                return View("LoginBankId", model); // Reload LoginBankId view
            }
            */

            string birthNo = (string)TempData["birthNo"];
            TempData["birthNo"] = birthNo;
            string bankID = model.BankID;
            // If birthNo is empty, redirect to LoginBirth:
            if (birthNo == null)
            {
                return RedirectToAction("LoginBirth");
            }
            //Debug.WriteLine("birthNo: " + birthNo + ", bankId: " + bankID);

            // Valider bankId ved hjelp av fødselsnummer og engangskode:
            if (bankService.validateBankId(birthNo, bankID) == true)
            {
                // Ja validering vellykket
                // Gå videre til Passord input:
                return RedirectToAction("LoginPassword");
            }
            else
            {
                // Nei validering ikke vellyket:
                Session["LoggedIn"] = false;
                return View();
            }
        }


        public ActionResult LoginPassword()
        {
            if (Session["LoggedIn"] == null)
            {
                Session["LoggedIn"] = false;
            }
            return View();
        }
        

        [HttpPost]
        public ActionResult LoginPassword(LoginViewModel model)
        {
            
            
            /*
            // If BankId is empty, remove password value and go to bankId view:
            else if (model.BankID == null)
            {
                model.Password = null; // Clear password value:
                return View("LoginBankId", model); // Return to LoginBankId view
            }
            else if (model.Password == null)
            {
                model.Password = " "; // Change Password from null to " " to avoid being stuck in loop.
                return View("LoginPassword", model); // Reload LoginPassword view
            }
            */

            string password = model.Password;
            string birthNo = (string)TempData["birthNo"];
            TempData["birthNo"] = birthNo;
            // If birthNo is empty, redirect to LoginBirth:
            if (birthNo == null)
            {
                return RedirectToAction("LoginBirth");
            }
            //Debug.WriteLine("birthNo: " + birthNo + ", passord: " + password);
            if (bankService.checkValidLogin(password, birthNo))
            {
                // Ja brukernavn og passord er OK!
                Session["LoggedIn"] = true;
                Session["UserId"] = birthNo;
                TempData["birthNo"] = null;
                return RedirectToAction("Overview", "Customer", new { area = "" });
            }
            else
            {
                // Nei brukernavn og passord er IKKE OK!
                Session["LoggedIn"] = false;
                Session["UserId"] = null;
                return View();
            }
        }



    }
}