using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class AccountRepository
    {
        // Database Context
        BankContext db = new BankContext();

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

        public List<Account> getListByType(AccountType type)
        {
            return db.Accounts.Where(a => a.Type == type).ToList();
        }

        public List<Account> getListByBirthNo(string birthNo)
        {
            return db.Accounts.Where(a => a.Owner.BirthNo == birthNo).OrderBy(a => a.Type).ToList();
        }

        public List<Account> getListByTypeAndBirthNo(AccountType type, string birthNo)
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
                //Debug.WriteLine("DEBUG: " + e.Message);
                return false;
            }

        }


        // UPDATE
    }
}