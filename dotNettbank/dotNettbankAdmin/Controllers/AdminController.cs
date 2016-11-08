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

        public ActionResult FindCustomers()
        {
            List<Customer> customers = _adminService.getAllCustomers();

            return PartialView("_FindCustomers", customers);
        }
        public ActionResult Accounts()
        {
            List<Account> accounts = _adminService.getAllAccounts();

            return PartialView("_Accounts", accounts);
        }

        public ActionResult RegBetaling()
        {
            List<Payment> payment = _adminService.getAllPayments();
            return PartialView("_RegBetalingPartial", payment);
        }

        [HttpGet]
        public ActionResult Betal(int paymentId)
        {
            List<Payment> paymentList = _adminService.getAllPayments();
            if (ModelState.IsValid)
            {
                _adminService.completePayment(paymentId);
            }
            return PartialView("_RegBetalingPartial", paymentList);
        }

        public ActionResult Transactions()
        {
            List<Transaction> transactions = _adminService.getAllTransactions();
            return PartialView("_Transactions", transactions);
        }

        [HttpGet]
        public ActionResult editCustomerPartial(string customerBirthNo)
        {
            var customer = _adminService.getCustomerByBirthNo(customerBirthNo);
            CustomerVM model = new CustomerVM()
            {
                BirthNo = customer.BirthNo,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                PhoneNo = customer.PhoneNo
            };
            return PartialView("EditCustomerPartial", model);
        }

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

        public ActionResult UpdateCustomer(CustomerVM model)
        {
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

        public bool DeleteAccount(string accountNo)
        {
            Account accountToDelete = new Account() { AccountNo = accountNo };
            return _adminService.deleteAccount(accountToDelete);
        }
    }
}