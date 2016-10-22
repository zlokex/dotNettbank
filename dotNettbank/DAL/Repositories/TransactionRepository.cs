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

        // Get all sent and received transactions for one account of one person
        public List<Transaction> getTransactionsByAccountNo(string accountNo)
        {
            // Get all transactions matching from accountNo (Avsender)
            List<Transaction> transactions = db.Transactions.Where(t => t.FromAccount.AccountNo == accountNo).ToList();
            // Add all transactions matching to accountNo (Mottaker)
            transactions.AddRange(db.Transactions.Where(t => t.ToAccount.AccountNo == accountNo).ToList());
            return transactions;
        }

        // Get all sent and received transactions for one person
        public List<Transaction> getTransactionsByBirthNo(string birthNo)
        {
            // Get all transactions matching from accountNo (Avsender)
            List<Transaction> transactions = db.Transactions.Where(t => t.FromAccount.Owner.BirthNo == birthNo).ToList();
            // Add all transactions matching to accountNo (Mottaker)
            transactions.AddRange(db.Transactions.Where(t => t.ToAccount.Owner.BirthNo == birthNo).ToList());
            return transactions;
        }


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