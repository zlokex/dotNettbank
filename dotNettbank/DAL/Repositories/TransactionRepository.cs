using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class TransactionRepository
    {
        // Database Context
        BankContext db = new BankContext();

        // GET SINGLE MODEL

        // GET LIST OF MODELS

        // INSERT / DELETE

        public bool addTransaction(Transaction transactions)
        {
            try
            {
                db.Transactions.Add(transactions);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DEBUG i addTransactions DAL: " + e.Message);
                return false;
            }

        }

        // UPDATE
    }
}