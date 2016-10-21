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
    //CustomerController har ansvar for kunder (innlogget).
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
                ViewBag.LoggedIn = (bool)Session["LoggedIn"];
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
            AccountViewModel a = new AccountViewModel()
            {
                Type = AccountType.Usage,
                AccountNo = "12345",
                Balance = 10000
            };

            var accounts = new List<AccountViewModel>();
            accounts.Add(a);
            accounts.Add(a);

            TransactionViewModel t = new TransactionViewModel()
            {
                Date = new DateTime(2016, 1, 1),
                Message = "Beskrivelse",
                InAmount = 1000,
                //OutAmount = 0,
                FromName = "André Hovda",
                ToName = "Magnus Barnholt",
                FromAccountNo = "12345",
                ToAccountNo = "23456"
            };

            var transactions = new List<TransactionViewModel>();
            transactions.Add(t);
            transactions.Add(t);
            transactions.Add(t);
            transactions.Add(t);

            var accountStatement = new AccountStatement();
            //accountStatement.Accounts.Add(a);
            accountStatement.Accounts = accounts;
            accountStatement.Transactions = transactions;
            return View(accountStatement);
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