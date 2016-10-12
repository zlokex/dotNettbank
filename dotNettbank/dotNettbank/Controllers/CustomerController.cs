using dotNettbank.BLL;
using dotNettbank.ViewModels;
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
                ViewBag.Innlogget = false;
            }
            else
            {
                ViewBag.Innlogget = (bool) Session["LoggedIn"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginCredentials)
        {
            // sjekk om innlogging OK
            if (bankService.checkValidLogin(loginCredentials))
            {
                // Ja brukernavn og passord er OK!
                Session["LoggedIn"] = true;
                ViewBag.Innlogget = true;
                return View();
            }
            else
            {
                // Nei brukernavn og passord er IKKE OK!
                Session["LoggedIn"] = false;
                ViewBag.Innlogget = false;
                return View();
            }
        }

        public ActionResult LoginBankID()
        {
            return View();
        }

        public ActionResult Overview() // Total oversikt
        {
            if (Session["LoggedIn"] != null)
            {
                bool loggetInn = (bool)Session["LoggedIn"];
                if (loggetInn)
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

        public ActionResult Transfer() // Overføre (Mellom egne konti)
        {
            return View();
        }

        public ActionResult DueTransactions() // Forfallsoversikt
        {
            return View();
        }

        public ActionResult PaymentReceipts() // Utførte betalinger
        {
            return View();
        }

    }
}