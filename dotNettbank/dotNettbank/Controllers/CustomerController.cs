﻿using dotNettbank.BLL;
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
            //Session["LoggedIn"] = true; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            //Session["UserId"] = "01018912345"; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                string userBirthNo = Session["UserId"] as string;

                // Get accounts to user:
                List<Account> accounts = bankService.getAccountsByBirthNo(userBirthNo);
                var model = new AccountStatement();

                var accountViewModels = new List<AccountViewModel>();
                // Populate AccountViewModel list with accounts:
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

                // Set initial dates for the datepickers:
                DateTime currDatePlusOne = DateTime.Today.AddDays(1); // Current day plus one
                DateTime oneMonthAgo = DateTime.Today.AddMonths(-1); // Date one month ago at 0:00am

                var accountStatement = new AccountStatement()
                {
                    Accounts = accountViewModels,
                    fromDate = oneMonthAgo,
                    toDate = currDatePlusOne
                };

                return View(accountStatement);
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
        }


        public ActionResult PaymentInsert() // Legg til betaling
        {

            Session["LoggedIn"] = true; // TODO: USED FOR TESTING ONLY, REMEMBER TO COMMENT OUT WHEN DONE TESTING
            Session["UserId"] = "01018912345"; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }
                // LOGIC STARTS HERE:

                string userBirthNo = Session["UserId"] as string;
                /*
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
                */
                var model = new PaymentInsertModel();
                // Add account view model list to view:
                //model.Accounts = new SelectList(accountViewModels, "SelectedAccountId", "FromAccountNo", 1);

                return View(model);
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
        }
        
        [HttpPost]
        public ActionResult PaymentInsert(PaymentInsertModel model) // Legg til betaling
        {
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                if (!ModelState.IsValid)
                {
                    return View();
                }
                // LOGIC STARTS HERE:
                //string userBirthNo = Session["UserId"] as string;
                //List<Account> accounts = bankService.getAccountsByBirthNo(userBirthNo);
                //AccountViewModel fromAccountV = model.Accounts.ElementAt(model.SelectedFromAccount);

                // User birth no:
                string userBirthNo = Session["UserId"] as string;

                // User input of from/to accountNo:
                string fromAccountNo = model.FromAccountNo;
                string toAccountNo = model.ToAccountNo;

                // Check that from and to account are not the same:
                if (fromAccountNo != toAccountNo) {
                    Account fromAccount = bankService.getByAccountNo(fromAccountNo);
                    Account toAccount = bankService.getByAccountNo(fromAccountNo);

                    // Check that from account exists:
                    if (fromAccount != null)
                    {
                        // Check that the account belongs to logged in user:
                        if (fromAccount.Owner.BirthNo == userBirthNo)
                        {
                            // Check that to account exists:
                            if (toAccount != null)
                            {
                                
                                double amount = model.AmountKr + (model.AmountOre / 100);

                                // Check that amount being payed is below or equal to balance:
                                if (amount <= fromAccount.Balance)
                                {
                                    // ALL IS GOOD: Attempt to go through with payment:
                                    var payment = new Payment()
                                    {
                                        DateAdded = DateTime.Now,
                                        DueDate = model.DueDate,
                                        Amount = amount,
                                        Message = model.Message,
                                        FromAccountNo = fromAccountNo,
                                        ToAccountNo = toAccountNo
                                    };

                                    if (bankService.addPayment(payment))
                                    {
                                        // Success     
                                        return RedirectToAction("DuePayments", "Customer", new { area = "" });
                                    }
                                    else
                                    {
                                        // Add to db failed
                                        return View();
                                    }
                                }
                            }
                        }
                    }
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

        public ActionResult DuePayments() // Forfallsoversikt
        {
            Session["LoggedIn"] = true; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            Session["UserId"] = "01018912345"; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                string userBirthNo = Session["UserId"] as string;

                // Get accounts to user:
                List<Account> accounts = bankService.getAccountsByBirthNo(userBirthNo);
                var model = new AccountStatement();

                var accountViewModels = new List<AccountViewModel>();
                // Populate AccountViewModel list with accounts:
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

                // Set initial dates for the datepickers:
                DateTime currDatePlusOne = DateTime.Today.AddDays(1); // Current day plus one
                DateTime oneMonthAgo = DateTime.Today.AddMonths(-1); // Date one month ago at 0:00am

                var duePayments = new DuePayments()
                {
                    Accounts = accountViewModels,
                    fromDate = oneMonthAgo,
                    toDate = currDatePlusOne
                };

                return View(duePayments);
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
        }

        public ActionResult PaymentReceipts() // Utførte betalinger 
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

        [HttpPost]
        public ActionResult GetTransactions(string accountNo, DateTime fromDate, DateTime toDate)
        {
            string userBirthNo = Session["UserId"] as string;

            // Temp list of transactions from db:
            List<Transaction> transactions1 = bankService.getTransactionsByAccountNo(accountNo);
            // Create a new list from our temp list where Date (Date added) is inbetween from and to date:
            List<Transaction> transactions = transactions1.Where(t => t.Date <= toDate && t.Date >= fromDate).ToList();

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
                // Check if our customer is either receiver or sender of amount in  transaction, and update 
                // either In or Out amount in ViewModel accordingly:
                if (t.ToAccount.Owner.BirthNo == userBirthNo)
                {
                    viewModel.InAmount = t.Amount;
                } else
                {
                    viewModel.OutAmount = t.Amount;
                }
                tViewModels.Add(viewModel);
            }

            return PartialView("TransactionsPartial", tViewModels);

            //JsonResult result = Json(tViewModels, JsonRequestBehavior.AllowGet);
            //return result;
        }

        [HttpPost]
        public ActionResult GetPayments(string accountNo)
        {
            string userBirthNo = Session["UserId"] as string;

            // List of due payments (payments made by this user (fromaccountno)
            List<Payment> duePayments = bankService.getDuePaymentsByAccountNo(accountNo);

            // List of Payment view model:
            List<PaymentVM> viewModels = new List<PaymentVM>();

            foreach (var t in duePayments)
            {
                var viewModel = new PaymentVM()
                {
                    PaymentID = t.PaymentID,
                    DateAdded = t.DateAdded,
                    DueDate = t.DueDate,
                    Amount = t.Amount,
                    Message = t.Message,
                    FromName = t.FromAccount.Owner.FirstName,
                    FromAccountNo = t.FromAccount.AccountNo,
                    ToName = t.ToAccount.Owner.FirstName,
                    ToAccountNo = t.ToAccount.AccountNo,
                };
                viewModels.Add(viewModel);
            }

            return PartialView("PaymentsPartial", viewModels);

        }

        
        [HttpPost]
        public bool DeleteDuePayment(int paymentID)
        {
            // Get Payment that is to be deleted from the paymentID:
            Payment paymentToBeDeleted = bankService.getPaymentById(paymentID);

            // Attempt to delete payment from db:
            bool success = bankService.deletePayment(paymentToBeDeleted);

            // Return success status:
            return success;

        }

        [HttpPost]
        public ActionResult GetAccountInfo(string accountNo)
        {
            // Get account from db matching account number:
            Account account = bankService.getByAccountNo(accountNo);

            // Create view model for this account:
            AccountViewModel viewModel = new AccountViewModel()
            {
                AccountNo = account.AccountNo,
                Type = account.Type,
                Balance = account.Balance
            };

            return PartialView("AccountInfoPartial", viewModel);

        }
    }
}