﻿using BLL.AdminService;
using dotNettbank.Model;
using dotNettbankAdmin.Models;
using dotNettbank.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using MoreLinq;

namespace dotNettbankAdmin.Controllers
{
    public class AdminController : Controller
    {
        private IAdminService _adminService;

        public AdminController()
        {
            _adminService = new AdminService();
        }

        public AdminController(IAdminService stub)
        {
            _adminService = stub;
        }

        // GET: Admin
        public ActionResult AdminSide()
        {
            if (Session["LoggedIn"] != null) //Dette skal sjekke om den innloggede sitt navn er likt session 
            {
                List<Payment> bl = _adminService.getAllPayments();
                AdminSideModel model = new AdminSideModel(bl);

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "");
            }
        }


        [WebMethod]
        public bool Login(string username, string password)
        {
            if (_adminService.validateLogin(username, password))
            {
                Session["LoggedIn"] = username;
                return true;
            }

            return false;
        }

        public ActionResult Logout()
        {
            Session["LoggedIn"] = null;
            return RedirectToAction("Index", "Index");
        }

        //--- GetPartials() PARTIALS LINKED FROM SIDEBAR MENU: ---

        public ActionResult FindCustomers(string birthNo, string accountNo)
        {
            List<Customer> customers = _adminService.getAllCustomers();

            return PartialView("_FindCustomers", customers);
        }
        public ActionResult Accounts(string birthNo, string accountNo)
        {
            List<Account> accounts;

            if (birthNo == null) { 
                accounts = _adminService.getAllAccounts();
            } else
            {
                accounts = _adminService.getAllAccountsByBirthNo(birthNo);
            }

            return PartialView("_Accounts", accounts);
        }

        public ActionResult RegBetaling(string birthNo, string accountNo)
        {
            List<Payment> payments = new List<Payment>();
            if (birthNo == null && accountNo == null)
            {
                payments = _adminService.getAllPayments();
            }
            else
            {
                if (birthNo != null)
                {
                    List<Payment>  paymentsBirth = _adminService.getPaymentsByFromBirthNo(birthNo);
                    payments.AddRange(paymentsBirth);
                }
                if (accountNo != null)
                {
                    List<Payment>  paymentsAccount = _adminService.getPaymentsByFromAccountNo(accountNo);
                    payments.AddRange(paymentsAccount);
                }
                //payment = payment.DistinctBy(i => i.FromAccountNo).ToList();
                payments = payments.GroupBy(x => x.PaymentID).Select(x => x.First()).ToList();
            }

            return PartialView("_RegBetalingPartial", payments);
        }

        public ActionResult Transactions(string birthNo, string accountNo)
        {
            List<Transaction> transactions = new List<Transaction>();
            if (birthNo == null && accountNo == null)
            {
                transactions = _adminService.getAllTransactions();
            }
            else
            {
                if (birthNo != null)
                {
                    List<Transaction> transactionsBirth = _adminService.getTransactionsByBirthNo(birthNo);
                    transactions.AddRange(transactionsBirth);
                }
                if (accountNo != null)
                {
                    List<Transaction> transactionsAccount = _adminService.getTransactionsByAccountNo(accountNo);
                    transactions.AddRange(transactionsAccount);
                }
                //payment = payment.DistinctBy(i => i.FromAccountNo).ToList();
                transactions = transactions.GroupBy(x => x.TransactionID).Select(x => x.First()).ToList();
            }
            return PartialView("_Transactions", transactions);
        }

        // --- GET MODAL PARTIALS ---

        [HttpGet]
        public ActionResult GetEditAccountPartial(string accountNo)
        {
            var account = _adminService.getAccountByAccountNo(accountNo);
            AccountVM model = new AccountVM()
            {
                AccountNo = account.AccountNo,
                Balance = account.Balance,
                OwnerBirthNo = account.OwnerBirthNo,
                Type = account.Type
            };
            return PartialView("_EditAccountsPartial", model);
        }

        //--- UPDATE DB METODS ---

        [HttpPost]
        public bool Betal(int paymentID)
        {
            List<Payment> paymentList = _adminService.getAllPayments();
            return _adminService.completePayment(paymentID);
        }

        public ActionResult UpdateAccount(AccountVM model)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account()
                {
                    AccountNo = model.AccountNo,
                    Balance = model.Balance,
                    OwnerBirthNo = model.OwnerBirthNo,
                    Type = model.Type
                };

                _adminService.updateAccount(account);
                return Json(new { success = true });
            }
            // else
            return PartialView("_EditAccountsPartial", model);
        }

        public string DeactivateAccount(string accountNo)
        {
            
            return _adminService.deactivateAccount(accountNo);
        }

        public string DeactivateCustomer(string birthNo)
        {

            return _adminService.deactivateCustomer(birthNo);
        }
    }
}