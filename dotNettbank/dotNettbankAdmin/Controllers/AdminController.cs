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
using System.Diagnostics;
using DAL.Log;
using System.Diagnostics;
using MoreLinq;
using Z.EntityFramework.Plus;
using dotNettbank.BLL;

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

        private bool checkSession()
        {
            if (Session != null)
            {
                if (Session["LoggedIn"] != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        // GET: Admin
        public ActionResult AdminSide()
        {
            if (checkSession())
            {
                if (Session != null)
                {
                    Admin u = _adminService.getAdmin("" + Session["LoggedIn"]);
                    ViewBag.UserName = u.Username;
                    ViewBag.Email = u.Email;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Index");
            }
        }


        [WebMethod]
        public bool Login(string username, string password)
        {          
                if (_adminService.validateLogin(username, password))
                {
                    if (Session != null)
                    {
                        Session["LoggedIn"] = username;
                    }
                  return true;
                }       
            return false;       
        }

        public ActionResult Logout()
        {
            if (Session != null)
            {
                Session["LoggedIn"] = null;
            }
            return RedirectToAction("Index", "Index");
        }

        //--- GetPartials() PARTIALS LINKED FROM SIDEBAR MENU: ---


        public ActionResult FindCustomers(string[] birthNo, string[] accountNo)
        {
            if(!checkSession()) return RedirectToAction("Index", "Index");

            List<Customer> customers = _adminService.getAllCustomers();

            List<ExtendedCustomerVM> customersVM = new List<ExtendedCustomerVM>();
            foreach (var findCustomers in customers)
            {
                var tempPA = _adminService.getPostalAreaByPostCode(findCustomers.PostCode);

                ExtendedCustomerVM model = new ExtendedCustomerVM()
                {
                    BirthNo = findCustomers.BirthNo,
                    FirstName = findCustomers.FirstName,
                    LastName = findCustomers.LastName,
                    Address = findCustomers.Address,
                    PhoneNo = findCustomers.PhoneNo,
                    PostCode = findCustomers.PostCode,
                    PostalArea = tempPA.Area
                };
                customersVM.Add(model);
            }

            return PartialView("_FindCustomers", customersVM);

        }
        public ActionResult Accounts(string[] birthNo, string[] accountNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");

            List<Account> accounts;

            if (birthNo == null) { 
                accounts = _adminService.getAllAccounts();
            } else
            {
                accounts = _adminService.getAccountsByBirthNoArray(birthNo);
            }

            return PartialView("_Accounts", accounts);
        }

        public ActionResult RegBetaling(string[] birthNo, string[] accountNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");
            List<Payment> payments = new List<Payment>();
            if (accountNo != null)
            {
                List<Payment>  paymentsAccount = _adminService.getPaymentsByFromAccountNoArray(accountNo);
                payments.AddRange(paymentsAccount);
            }
            else if (birthNo != null)
            {
                List<Payment> paymentsBirth = _adminService.getPaymentsByFromBirthNoArray(birthNo);
                payments.AddRange(paymentsBirth);
            }
            else 
            {
                payments = _adminService.getAllPayments();
            }

            //Create view model from domain model:
            var paymentVMs = new List<PaymentRow>();
            foreach (var payment in payments)
            {
                var fromBirthNo = _adminService.getBirthNoByAccountNo(payment.FromAccountNo);

                var paymentVM = new PaymentRow()
                {
                    PaymentID = payment.PaymentID,
                    DateAdded = payment.DateAdded,
                    DueDate = payment.DueDate,
                    Amount = payment.Amount,
                    FromAccountNo = payment.FromAccountNo,
                    ToAccountNo = payment.ToAccountNo,
                    Message = payment.Message,
                    FromBirthNo = fromBirthNo
                };
                paymentVMs.Add(paymentVM);
            }

            return PartialView("_RegBetalingPartial", paymentVMs);
        }

        public ActionResult Transactions(string[] birthNo, string[] accountNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");
            List<Transaction> transactions = new List<Transaction>();
            if (birthNo == null && accountNo == null)
            {
                transactions = _adminService.getAllTransactions();
            }
            if (accountNo != null)
            {
                List<Transaction> transactionsAccount = _adminService.getTransactionsByAccountNoArray(accountNo);
                transactions.AddRange(transactionsAccount);
            }
            else if (birthNo != null)
            {
                List<Transaction> transactionsBirth = _adminService.getTransactionsByBirthNoArray(birthNo);
                transactions.AddRange(transactionsBirth);
            }
            else
            {
                transactions = _adminService.getAllTransactions();
            }

            //Create view model from domain model:
            var transactionVMs = new List<TransactionRow>();
            foreach(var transaction in transactions)
            {
                var fromBirthNo = _adminService.getBirthNoByAccountNo(transaction.FromAccountNo);
                var toBirthNo = _adminService.getBirthNoByAccountNo(transaction.ToAccountNo);

                var transactionVM = new TransactionRow()
                {
                    Date = transaction.Date,
                    DatePayed = transaction.DatePayed,
                    Amount = transaction.Amount,
                    FromAccountNo = transaction.FromAccountNo,
                    ToAccountNo = transaction.ToAccountNo,
                    Message = transaction.Message,
                    FromBirthNo = fromBirthNo,
                    ToBirthNo = toBirthNo
                };
                transactionVMs.Add(transactionVM);
            }


            return PartialView("_Transactions", transactionVMs);
        }

        // --- GET MODAL PARTIALS ---
        [HttpPost]
        public bool Betal(int paymentID)
        {
            List<Payment> paymentList = _adminService.getAllPayments();
            if (paymentID == -1) //Utfører alle betalinger 
            {
                foreach (Payment i in paymentList)
                {
                    _adminService.completePayment(i.PaymentID);
                }
                return true;
            }
            return _adminService.completePayment(paymentID);
        }

        //Slette
        [HttpPost]
        public bool DeletePayment(int paymentID)
        {
            return _adminService.deletePayment(paymentID);
        }



        [HttpGet]
        public ActionResult EditCustomerPartial(string birthNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");
            /*Debug.Indent();
            Debug.WriteLine("Ditt personummer er: " + birthNo);*/
            var customer = _adminService.getCustomerByBirthNo(birthNo);

            PostalArea tempPA = _adminService.getPostalAreaByPostCode(customer.PostCode);

            /*PostalArea postalArea = new PostalArea()
            {
                Area = tempPA.Area
            };*/
            var model = new CustomerVM()
            {
                BirthNo = customer.BirthNo,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                PhoneNo = customer.PhoneNo,
                PostCode = customer.PostCode,
                PostalArea = tempPA.Area
            };
            return PartialView("_EditCustomersPartial", model);
        }


        [HttpPost]
        public ActionResult AddCustomer(AddCustomer regCustomer)
        {    
            string salt = AdminService.generateSalt();
            var passwordAndSalt = regCustomer.Password + salt;
            byte[] passwordDB = AdminService.createHash(passwordAndSalt);
            PostalArea postalArea = new PostalArea()
            {
                Area = regCustomer.PostalArea,
                PostCode = regCustomer.PostCode
            };
            _adminService.addPostalArea(postalArea);
            Customer customer = new Customer()
            {
                BirthNo = regCustomer.BirthNo,
                FirstName = regCustomer.FirstName,
                LastName = regCustomer.LastName,
                Address = regCustomer.Address,
                PhoneNo = regCustomer.PhoneNo,
                PostCode = regCustomer.PostCode,
                Password = passwordDB,
                Salt = salt
            };
            if (_adminService.addCustomer(customer))
            {
                // If succesfull:
                return Json(new { success = true });
            }
            else
            {
                // If not successfull:
                return PartialView("_AddCustomerPartial", regCustomer);
            }
        }
        

        [HttpPost]
        public ActionResult CreatePayment (PaymentVM form)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");

            if (ModelState.IsValid)
            {
                DateTime today = DateTime.Now;
                Payment p = new Payment()
                {
                    DateAdded = today,
                    DueDate = today,
                    Amount = form.Amount,
                    Message = form.Message,
                    FromAccountNo = form.FromAccountNo,
                    ToAccountNo = form.ToAccountNo,

                };

                if (_adminService.createPayment(p))
                    return Json(new { success = true });
            }

            return PartialView("_CreatePaymentPartial", form);
        }

      
        
        public ActionResult GetPartial(string path)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");

            return PartialView(path);
        }

        [HttpGet]
        public ActionResult GetCreateAccountPartial(string accountNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");

            var account = _adminService.getAccountByAccountNo(accountNo);
            var customers = _adminService.getAllCustomers();
            AccountVM model = new AccountVM()
            {
                Customers = customers
            };
            return PartialView("_CreateAccountPartial", model);
        }

        public ActionResult UpdateCustomer(CustomerVM model)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");

            if (ModelState.IsValid)
            {
                var newPostalArea = new PostalArea()
                {
                    Area = model.PostalArea,
                    PostCode = model.PostCode
                };
                // Attempt to create new postal area:
                _adminService.addPostalArea(newPostalArea);

                Customer customer = new Customer()
                {
                    BirthNo = model.BirthNo,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNo = model.PhoneNo,
                    PostCode = model.PostCode
                };

                _adminService.updateCustomer(customer);
                return Json(new { success = true });
            }
            // else
            return PartialView("_EditCustomersPartial", model);
        }

        public ActionResult UpdateAccount(AccountVM model)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");

            if (ModelState.IsValid)
            {
                Account account = new Account()
                {
                    OwnerBirthNo = model.OwnerBirthNo,
                    Type = model.Type
                };

                _adminService.updateAccount(account);
                return Json(new { success = true });
            }
            // else
            return PartialView("_EditAccountsPartial", model);
        }


        [HttpPost]
        public ActionResult AddAccount(AccountVM model)
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");

            Random random = new Random();
            int newAccNo1 = random.Next(1000, 9999);
            int newAccNo2 = random.Next(10, 99);
            int newAccNo3 = random.Next(10000, 99999);

            if (ModelState.IsValid)
            {
                Account account = new Account()
                {
                    AccountNo = "" + newAccNo1  + newAccNo2 + newAccNo3,
                    Balance = 5000,
                    OwnerBirthNo = model.OwnerBirthNo,
                    Type = model.Type
                };

                _adminService.addAccount(account);
                return Json(new { success = true });
            }
            // else
            return PartialView("_CreateAccountPartial", model);
        }


        public string DeactivateAccount(string accountNo)
        {
            return _adminService.deactivateAccount(accountNo);
        }

        public string DeactivateCustomer(string birthNo)
        {
            return _adminService.deactivateCustomer(birthNo);
        }
    

        public ActionResult Audit()
        {
            if (!checkSession()) return RedirectToAction("Index", "Index");
            List<AuditEntry> auditEntries = _adminService.getAllAuditEntries();

            List<AuditEntryVM> entryVMs = new List<AuditEntryVM>();

            foreach (AuditEntry entry in auditEntries)
            {
                List<AuditEntryProperty> properties =
                    _adminService.getAuditEntryPropertiesByEntryId(entry.AuditEntryID);

                List<AuditEntryPropertyVM> propertyVMs = new List<AuditEntryPropertyVM>();
                foreach (AuditEntryProperty property in properties)
                {
                    string propertyOldValue = "";
                    if (property.OldValue != null)
                    {
                        propertyOldValue = property.OldValue.ToString();
                    }

                    string propertyNewValue = "";
                    if (property.NewValue != null)
                    {
                        propertyNewValue = property.NewValue.ToString();
                    }

                    AuditEntryPropertyVM propertyVM = new AuditEntryPropertyVM()
                    {
                        Date = entry.CreatedDate,
                        EntityName = entry.EntityTypeName,
                        State = entry.StateName,
                        PropertyName = property.PropertyName,
                        OldValue = propertyOldValue,
                        NewValue = propertyNewValue
                    };
                    propertyVMs.Add(propertyVM);
                }


                AuditEntryVM entryVM = new AuditEntryVM()
                {
                    AuditEntryID = entry.AuditEntryID,
                    Date = entry.CreatedDate,
                    EntityName = entry.EntityTypeName,
                    State = entry.StateName,
                    EntryProperties = propertyVMs
                };
                entryVMs.Add(entryVM);
            }

            entryVMs.Reverse();
            return PartialView("_Audit", entryVMs);
        }





        /*
    public ActionResult RegCustomer(string[] birthNo)
    {
        List<Customer> customers = new List<Customer>();
        if (birthNo == null)
        {
            customers = _adminService.getAllCustomers();
        }
        else
        {
            if (birthNo != null)
            {

            }    
        }
        return PartialView("_AddCustomerPartial", customers);
    }
    */

    }
}