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
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }
            }
            return View();
        }

        public ActionResult OpenAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OpenAccount(Account innKonto)
        {/*
            if (ModelState.IsValid)
            {
                var kundeDb = new KundeLogikk();
                bool insertOK = kundeDb.settInn(innKonto);
                if (insertOK)
                {
                    return RedirectToAction("Liste");
                }
            }*/
            return View();
        }


        public ActionResult AccountStatement() // Kontoutskrift
        {
            Session["LoggedIn"] = true; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                string userBirthNo = Session["UserId"] as string;

                List<Account> accounts = bankService.getAccountsByBirthNo(userBirthNo);
                var accountViewModels = new List<AccountViewModel>();

                foreach (var a in accounts)
                {
                    AccountViewModel viewModel = new AccountViewModel()
                    {
                        Type = a.Type,
                        AccountNo = a.AccountNo,
                        Balance = a.Balance
                    };
                    accountViewModels.Add(viewModel);
                }


                /*

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
                */
                var accountStatement = new AccountStatement();
                accountStatement.Accounts = accountViewModels;

                return View(accountStatement);
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
        }

        public ActionResult PaymentInsert() // Legg til betaling
        {
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                return View();
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
        }

        public ActionResult Transfer() // Overføre (Mellom egne konti) // LAV PRIO
        {
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                return View();
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
        }

        public ActionResult DueTransactions() // Forfallsoversikt
        {
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                return View();
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
        }

        public ActionResult PaymentReceipts() // Utførte betalinger // LAV PRIO
        {
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                return View();
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
        }


        public JsonResult GetTransactions(string accountNo)
        {
            string userBirthNo = Session["UserId"] as string;

            List<Transaction> transactions = bankService.getTransactionsByAccountNo(accountNo);

            List<TransactionViewModel> tViewModels = new List<TransactionViewModel>();

            foreach (var t in transactions)
            {
                var viewModel = new TransactionViewModel()
                {
                    Date = t.Date,
                    Message = t.Message,
                    FromName = t.FromAccount.Owner.FirstName,
                    FromAccountNo = t.FromAccount.AccountNo,
                    ToName = t.ToAccount.Owner.FirstName,
                    ToAccountNo = t.ToAccount.AccountNo,
                };
                if (t.ToAccount.Owner.BirthNo == userBirthNo)
                {
                    viewModel.InAmount = t.Amount;
                } else
                {
                    viewModel.OutAmount = t.Amount;
                }
                tViewModels.Add(viewModel);
            }

            JsonResult result = Json(tViewModels, JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}