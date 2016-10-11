using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbank.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginBankID()
        {
            return View();
        }

        public ActionResult Overview() // Total oversikt
        {
            return View();
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