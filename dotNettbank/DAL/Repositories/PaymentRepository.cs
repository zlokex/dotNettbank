using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class PaymentRepository
    {
        // Database Context
        BankContext db = new BankContext();

        // GET SINGLE MODEL

        public Payment getById(int id)
        {
            return db.Payments.FirstOrDefault(p => p.PaymentID == id);
        }

        // GET LIST OF MODELS

        public List<Payment> getAll()
        {
            return db.Payments.ToList();
        }

        public List<Payment> getListByName()
        {
            return null;
        }

        public List<Payment> getListByFromName()
        {
            return null;
        }

        public List<Payment> getListByToName()
        {
            return null;
        }

        public List<Payment> getListByAccount()
        {
            return null;
        }

        public List<Payment> getListByFromAccount()
        {
            return null;
        }

        public List<Payment> getListByToAccount()
        {
            return null;
        }

        public List<Payment> getListByMessage()
        {
            return null;
        }


        public List<Payment> getListByDateRange(DateTime fromDate, DateTime toDate)
        {
            return null;
        }

        // INSERT / DELETE

        // UPDATE
    }
}