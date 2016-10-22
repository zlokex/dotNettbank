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
        BankContext db;
        public TransactionRepository(BankContext bankContext)
        {
            db = bankContext;
        }

        // GET SINGLE MODEL

        // GET LIST OF MODELS
        public List<Transaction> getAll()
        {
            return db.Transactions.ToList();
        }


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

        public bool addTransaction(Transaction transaction)
        {
            try
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DEBUG i addTransaction DAL: " + e.Message);
                return false;
            }
        }

        public bool addRangeTransactions(List<Transaction> transactions)
        {
            try
            {
                db.Transactions.AddRange(transactions);
                // Wait with saving changes until we know that payments has been removed.
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DEBUG i addRaneTransactions DAL: " + e.Message);
                return false;
            }
            
        }

        // UPDATE
    }
}