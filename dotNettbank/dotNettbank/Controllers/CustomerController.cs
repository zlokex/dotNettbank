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
    public class CustomerController : Controller
    {
        BankService bankService = new BankService();

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            // vis innlogging
            if (Session["LoggedIn"] == null)
            {
                Session["LoggedIn"] = false;
                ViewBag.LoggedIn = false;
            }
            else
            {
                ViewBag.LoggedIn = (bool) Session["LoggedIn"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginCredentials)
        {
            string password = loginCredentials.Password;
            string birthNo = loginCredentials.BirthNo;        

            // sjekk om innlogging OK
            if (bankService.checkValidLogin(password, birthNo))
            {
                // Ja brukernavn og passord er OK!
                Session["LoggedIn"] = true;
                ViewBag.LoggedIn = true;
                return RedirectToAction("Overview");
            }
            else
            {
                // Nei brukernavn og passord er IKKE OK!
                Session["LoggedIn"] = false;
                ViewBag.LoggedIn = false;
                return View();
            }
        }

        public ActionResult Overview() // Total oversikt
        {
            if (Session["LoggedIn"] != null)
            {
                bool LoggedIn = (bool)Session["LoggedIn"];
                if (LoggedIn)
                {

                    return View();
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult AccountStatement() // Kontoutskrift
        {
            return View();
        }

        public ActionResult PaymentInsert() // Legg til betaling
        {
            return View();
        }

        public ActionResult Transfer() // Overføre (Mellom egne konti) // LAV PRIO
        {
            return View();
        }

        public ActionResult DueTransactions() // Forfallsoversikt
        {
            return View();
        }

        public ActionResult PaymentReceipts() // Utførte betalinger // LAV PRIO
        {
            return View();
        }

    }
}