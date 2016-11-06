using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class PaymentRepository
    {
        // Database Context
        BankContext db;
        public PaymentRepository(BankContext bankContext)
        {
            db = bankContext;
        }

        

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

        public List<Payment> getDuePaymentsByAccountNo(string accountNo)
        {
            // Get all transactions matching from accountNo (Avsender)
            List<Payment> payments = db.Payments.Where(t => t.FromAccount.AccountNo == accountNo).ToList();

            return payments;
        }

        public List<Payment> getDuePaymentsByBirthNo(string birthNo)
        {
            // Get all transactions matching from accountNo (Avsender)
            List<Payment> payments = db.Payments.Where(t => t.FromAccount.OwnerBirthNo == birthNo).ToList();

            return payments;
        }

        public List<Payment> getPaymentsPassedDueDate()
        {
            // Get current time:
            DateTime currTime = DateTime.Now;
            // Return all payments that has passed its due date:
            return db.Payments.Where(p => p.DueDate < currTime).ToList();
        }

        public bool removePayments(List<Payment> paymentsToRemove)
        {
            try
            {
                db.Payments.RemoveRange(paymentsToRemove);
                // db.SaveChanges(); Wait with saving until we have added them to Transactions.
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
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
        public bool addPayment(Payment payment)
        {
            try
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DEBUG i addPayemnt DAL: " + e.Message);
                return false;
            }

        }

        public bool deletePayment(Payment payment)
        {
            try
            {
                db.Payments.Attach(payment);
                db.Payments.Remove(payment);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        // UPDATE

        public bool updatePayment(Payment updatedPayment)
        {
            try
            {
                db.Payments.Attach(updatedPayment);

                var entry = db.Entry(updatedPayment);
                entry.State = EntityState.Modified;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}