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
                string birthID = Session["UserId"] as string;
                List<Account> accounts = bankService.getAccountsByBirthNo(birthID);
                var model = new AccountStatement();

                var accountViewModels = new List<AccountViewModel>();
                // Populate AccountViewModel list with accounts:
                foreach (var a in accounts)
                {
                    AccountViewModel viewModel = new AccountViewModel()
                    {
                        Type = a.Type,
                        AccountNo = a.AccountNo,
                        Balance = a.Balance,
                        Name = a.Name
                    };
                    accountViewModels.Add(viewModel);
                }

                var accountStatement = new AccountStatement()
                {
                    Accounts = accountViewModels,
                };

                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }
                else
                {
                    var kundeDb = new BankService();
                    Customer enKunde = kundeDb.getCustomerByBirthNo(birthID);
                    List<Account> alleKonto = kundeDb.getAccountsByBirthNo(birthID);
                    return View(accountStatement);
                }
            }
            return RedirectToAction("LoginBirth", "Home", new { area = "" });
        }

        public ActionResult KontoOpprettet()
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

        [HttpPost]
        public ActionResult OpenAccount(regAccount regAccount)
        {
            Random random = new Random();
            int newAccNo1 = random.Next(1000, 9999);
            int newAccNo2 = random.Next(10, 99);
            int newAccNo3 = random.Next(10000, 99999);
            string user = Session["UserId"] as string;
            Customer customer = bankService.getCustomerByBirthNo(user);
            string AccountType = "Brukskonto";

            if (!ModelState.IsValid)
            {
                return View();
            }

            //Create new Account domain model:
            Account account = new Account()
            {

                AccountNo = "" + newAccNo1+"." + newAccNo2 + "." + newAccNo3,
                Name = regAccount.Name,
                Balance = 5000,
                Owner = customer,
                InterestRate = 1,
                Type = regAccount.Name
            };

            if (bankService.addAccount(account))
            {
                //if succsesfull
                ViewBag.OpenAccount = false;
                //return RedirectToAction("KontoOpprettet");
                return View();
            }
            else
            {
                ViewBag.OpenAccount = true;
                return View();
            }
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
                        Balance = a.Balance,
                        Name = a.Name
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

            //Session["LoggedIn"] = true; // TODO: USED FOR TESTING ONLY, REMEMBER TO COMMENT OUT WHEN DONE TESTING
            //Session["UserId"] = "01018912345"; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }
                // LOGIC STARTS HERE:

                string userBirthNo = Session["UserId"] as string;
                
                List<Account> fromAccounts = bankService.getAccountsByBirthNo(userBirthNo);
                List<Account> toAccounts = bankService.getAllAccounts();

                // Set initial dates for the datepicker:
                DateTime currDate = DateTime.Today;
                var model = new PaymentInsertModel()
                {
                    FromAccounts = new SelectList(fromAccounts, "AccountNo", "AccountNo"),
                    ToAccounts = new SelectList(toAccounts, "AccountNo", "AccountNo"),
                    DueDate = currDate
                };

                model.SelectedFromAccountNo = fromAccounts.FirstOrDefault().AccountNo;
                //model.SelectedFromAccountNo = "10000000001";
                model.SelectedToAccountNo = toAccounts.FirstOrDefault().AccountNo;
                //model.SelectedFromAccountNo = "10000000009";

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
            //Session["LoggedIn"] = true; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            //Session["UserId"] = "01018912345"; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
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
                string fromAccountNo = model.SelectedFromAccountNo;
                string toAccountNo = model.SelectedToAccountNo;

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
                                
                                double amount = model.AmountKr +  ((double) model.AmountOre / 100);

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
            //Session["LoggedIn"] = true; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
            //Session["UserId"] = "01018912345"; // TODO: REMEMBER TO COMMENT OUT. ONLY USED DURING TESTING PHASE
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

        [HttpGet]
        public ActionResult getPaymentInsertPartial(int paymentId)
        {
            if (Session["LoggedIn"] != null)
            {
                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }

                TempData["paymentId"] = paymentId;

                // Get payment 
                Payment p = bankService.getPaymentById(paymentId);

                int kr = (int)p.Amount;
                double oreDouble = (p.Amount - kr) * 100;
                int ore = (int)oreDouble;
                PaymentInsertModel viewModel = new PaymentInsertModel()
                {
                    AmountKr = kr,
                    AmountOre = ore,
                    DueDate = p.DueDate,
                    FromAccountNo = p.FromAccountNo,
                    Message = p.Message,
                    ToAccountNo = p.ToAccountNo
                };

                return PartialView("PaymentInsertPartial", viewModel);
            }
            else
            {
                return RedirectToAction("LoginBirth", "Home", new { area = "" });
            }
            
        }

        [HttpPost]
        public ActionResult getPaymentInsertPartial(PaymentInsertModel viewModel)
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
                    //return View();
                    return RedirectToAction("DuePayments");
                }

                double amount = viewModel.AmountKr + ((double) viewModel.AmountOre / 100);
                int paymentId = (int)TempData["paymentId"];
                Payment payment = new Payment()
                {
                    Amount = amount,
                    DateAdded = DateTime.Now,
                    DueDate = viewModel.DueDate,
                    FromAccountNo = viewModel.FromAccountNo,
                    ToAccountNo = viewModel.ToAccountNo,
                    Message = viewModel.Message,
                    PaymentID = paymentId
                };

                bool success = bankService.updatePayment(payment);

                if (success)
                {
                    //return PartialView("PaymentInsertPartial",viewModel);
                    return RedirectToAction("DuePayments");
                } else
                {
                    //return PartialView("PaymentInsertPartial", viewModel);
                    return RedirectToAction("DuePayments");
                }
            }
            //return PartialView("PaymentInsertPartial", viewModel);
            return RedirectToAction("DuePayments");
        }

        [HttpPost]
        public ActionResult GetTransactions(string accountNo, DateTime fromDate, DateTime toDate)
        {
            if (Session["LoggedIn"] != null)
            {
                string birthID = Session["UserId"] as string;

                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }
                else
                {
                    // List of transactions:
                    List<Transaction> transactions;

                    // Logged in customer birthNo:
                    string userBirthNo = Session["UserId"] as string;

                    // Check if All ccounts/Alle kontoer option was selected:
                    if (accountNo == "Alle kontoer")
                    {
                        // If all accounts are chosen

                        // Temp list of all transactions to user from db:
                        List<Transaction> transactions1 = bankService.getTransactionsByBirthNo(userBirthNo);
                        transactions = transactions1.Where(t => t.Date <= toDate && t.Date >= fromDate).ToList();

                    }
                    else
                    {
                        // If not, that means the user selected an account number, so get all transactions based on that accountNo:

                        // Temp list of all transactions to accountNo from db:
                        List<Transaction> transactions1 = bankService.getTransactionsByAccountNo(accountNo);
                        // Create a new list from our temp list where Date (Date added) is inbetween from and to date:
                        transactions = transactions1.Where(t => t.Date <= toDate && t.Date >= fromDate).ToList();
                    }


                    // View model:
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
                        }
                        else
                        {
                            viewModel.OutAmount = t.Amount;
                        }
                        tViewModels.Add(viewModel);
                    }

                    return PartialView("TransactionsPartial", tViewModels);
                }
            }
            return RedirectToAction("LoginBirth", "Home", new { area = "" });
            
        }

        [HttpPost]
        public ActionResult GetPayments(string accountNo)
        {
            if (Session["LoggedIn"] != null)
            {
                string birthID = Session["UserId"] as string;

                bool loggedIn = (bool)Session["LoggedIn"];
                if (!loggedIn)
                {
                    return RedirectToAction("LoginBirth", "Home", new { area = "" });
                }
                else
                {
                    // List of due payments 
                    List<Payment> duePayments;

                    // Check if All ccounts/Alle kontoer option was selected:
                    if (accountNo == "Alle kontoer")
                    {
                        // If all accounts are chosen
                        string userBirthNo = Session["UserId"] as string;
                        // Get all due payments to user:
                        duePayments = bankService.getDuePaymentsByBirthNo(userBirthNo);

                    }
                    else
                    {
                        // If not, that means the user selected an account number, so get all due payments based on that accountNo:
                        duePayments = bankService.getDuePaymentsByAccountNo(accountNo);
                    }

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

            }
            return RedirectToAction("LoginBirth", "Home", new { area = "" });
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
            // View model
            AccountViewModel viewModel;

            // Check if All ccounts/Alle kontoer option was selected:
            if (accountNo == "Alle kontoer")
            {
                string userBirthNo = Session["UserId"] as string;
                List<Account> accounts = bankService.getAccountsByBirthNo(userBirthNo);
                double totalBalance = 0;
                accounts.ForEach(a => totalBalance += a.Balance);

                // If all accounts are chosen
                viewModel = new AccountViewModel()
                {
                    AccountNo = "Alle kontoer",
                    Type = "Alle kontoer",
                    Balance = totalBalance
                };

            }
            else
            {
                // If not all accounts are selected, an accountNo is, get that account:
                // Get account from db matching account number:
                Account account = bankService.getByAccountNo(accountNo);

                // Create view model for this account:
                viewModel = new AccountViewModel()
                {
                    AccountNo = account.AccountNo,
                    Type = account.Type,
                    Balance = account.Balance
                };
            }
            return PartialView("AccountInfoPartial", viewModel);

        }
    }
}