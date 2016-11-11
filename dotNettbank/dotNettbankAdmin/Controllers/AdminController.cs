using BLL.AdminService;
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

        public bool checkSession()
        {
            if(Session["LoggedIn"] != null)
            {
                return true;
            }else
            {
                return false;
            }
        }

        // GET: Admin
        public ActionResult AdminSide()
        {
            if (Session["LoggedIn"] != null) //Dette skal sjekke om den innloggede sitt navn er likt session 
            {
                List<Payment> bl = _adminService.getAllPayments();
                AdminSideModel model = new AdminSideModel(bl);
                
                Admin u = _adminService.getAdmin("" + Session["LoggedIn"]);
                ViewBag.UserName = u.Username;
                ViewBag.Email = u.Email;

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

        public ActionResult FindCustomers(string[] birthNo, string[] accountNo)
        {
            if(!checkSession()) return RedirectToAction("Index", "");
            List<Customer> customers = _adminService.getAllCustomers();

            return PartialView("_FindCustomers", customers);
        }
        public ActionResult Accounts(string[] birthNo, string[] accountNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "");

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
            if (!checkSession()) return RedirectToAction("Index", "");
            List<Payment> payments = new List<Payment>();
            if (birthNo == null && accountNo == null)
            {
                payments = _adminService.getAllPayments();
            }
            else
            {
                if (birthNo != null)
                {
                    List<Payment>  paymentsBirth = _adminService.getPaymentsByFromBirthNoArray(birthNo);
                    payments.AddRange(paymentsBirth);
                }
                if (accountNo != null)
                {
                    List<Payment>  paymentsAccount = _adminService.getPaymentsByFromAccountNoArray(accountNo);
                    payments.AddRange(paymentsAccount);
                }
                //payment = payment.DistinctBy(i => i.FromAccountNo).ToList();
                payments = payments.GroupBy(x => x.PaymentID).Select(x => x.First()).ToList();
            }

            return PartialView("_RegBetalingPartial", payments);
        }

        public ActionResult Transactions(string[] birthNo, string[] accountNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "");
            List<Transaction> transactions = new List<Transaction>();
            if (birthNo == null && accountNo == null)
            {
                transactions = _adminService.getAllTransactions();
            }
            else
            {
                if (birthNo != null)
                {
                    List<Transaction> transactionsBirth = _adminService.getTransactionsByBirthNoArray(birthNo);
                    transactions.AddRange(transactionsBirth);
                }
                if (accountNo != null)
                {
                    List<Transaction> transactionsAccount = _adminService.getTransactionsByAccountNoArray(accountNo);
                    transactions.AddRange(transactionsAccount);
                }
                //payment = payment.DistinctBy(i => i.FromAccountNo).ToList();
                transactions = transactions.GroupBy(x => x.TransactionID).Select(x => x.First()).ToList();
            }
            return PartialView("_Transactions", transactions);
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


        [HttpGet]
        public ActionResult EditCustomerPartial(string birthNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "");
            /*Debug.Indent();
            Debug.WriteLine("Ditt personummer er: " + birthNo);*/
            var customer = _adminService.getCustomerByBirthNo(birthNo);
            
            CustomerVM model = new CustomerVM()
            {
                BirthNo = customer.BirthNo,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                PhoneNo = customer.PhoneNo
            };
            return PartialView("_EditCustomersPartial", model);
        }

        [HttpPost]
        public ActionResult CreatePayment (PaymentVM form)
        {
            if (!checkSession()) return RedirectToAction("Index", "");

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
            if (!checkSession()) return RedirectToAction("Index", "");

            return PartialView(path);
        }

        [HttpGet]
        public ActionResult GetEditAccountPartial(string accountNo)
        {
            if (!checkSession()) return RedirectToAction("Index", "");

            var account = _adminService.getAccountByAccountNo(accountNo);
            var customers = _adminService.getAllCustomers();
            AccountVM model = new AccountVM()
            {
                Customers = customers
            };
            return PartialView("_EditAccountsPartial", model);
        }

        public ActionResult UpdateCustomer(CustomerVM model)
        {
            if (!checkSession()) return RedirectToAction("Index", "");

            if (ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    BirthNo = model.BirthNo,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNo = model.PhoneNo
                };

                _adminService.updateCustomer(customer);
                return Json(new { success = true });
            }
            // else
            return PartialView("_EditAccountsPartial", model);
        }

        public ActionResult UpdateAccount(AccountVM model)
        {
            if (!checkSession()) return RedirectToAction("Index", "");

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
            Random random = new Random();
            int newAccNo1 = random.Next(1000, 9999);
            int newAccNo2 = random.Next(10, 99);
            int newAccNo3 = random.Next(10000, 99999);

            if (!checkSession()) return RedirectToAction("Index", "");

            if (ModelState.IsValid)
            {
                Account account = new Account()
                {
                    AccountNo = "" + newAccNo1 + "." + newAccNo2 + "." + newAccNo3,
                    Balance = 5000,
                    OwnerBirthNo = model.OwnerBirthNo,
                    Type = model.Type
                };

                _adminService.addAccount(account);
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

        public ActionResult Audit()
        {
            if (!checkSession()) return RedirectToAction("Index", "");
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
    }
}