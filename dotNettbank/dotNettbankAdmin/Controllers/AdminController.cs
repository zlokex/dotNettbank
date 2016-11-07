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

        public ActionResult AdminPage()
        {
            return View();
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

        public ActionResult RegBetalingsPartial()
        {
            return PartialView("_RegBetalingPartial");
        }

        public ActionResult Transactions()
        {
            List<Transaction> transactions = _adminService.getAllTransactions();
            return PartialView("_Transactions", transactions);
        }
    }
}