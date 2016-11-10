using DAL.Log;
using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class AccountRepository
    {
        // Database Context
        BankContext db;
        public AccountRepository(BankContext bankContext)
        {
            db = bankContext;
        }

        // GET SINGLE MODEL

        public Account getByAccountNo(string accountNo)
        {
            return db.Accounts.FirstOrDefault(a => a.AccountNo == accountNo);
        }

        // GET LIST OF MODELS

        public List<Account> getAll()
        {
            return db.Accounts.ToList();
        }

        public List<Account> getListByType(string type)
        {
            return db.Accounts.Where(a => a.Type == type).ToList();
        }

        public List<Account> getListByBirthNo(string birthNo)
        {
            return db.Accounts.Where(a => a.Owner.BirthNo == birthNo).ToList();
        }

        public List<Account> getListByTypeAndBirthNo(string type, string birthNo)
        {
            return db.Accounts.Where(a => a.Type == type && a.Owner.BirthNo == birthNo).ToList();
        }

        // INSERT / DELETE

        // ADD / DELETE

        /// <summary>
        /// Add a customer to db from existing Customer object
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>True if succesful, false otherwise</returns>
        public bool addAccount(Account account)
        {
            try
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                string log = "Failed to add account.\t" + e.Message + "\t" + e.StackTrace.ToString();
                Debug.Write(log);
                new LogErrors().errorLog(log);
                return false;
            }

        }


        // UPDATE
    }
}