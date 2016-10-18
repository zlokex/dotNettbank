using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class AccountRepository
    {
        BankContext db = new BankContext();

        public List<Account> getAll()
        {
            return db.Accounts.ToList();
        }

        public Account getByAccountNo(int accountNo)
        {
            return db.Accounts.FirstOrDefault(a => a.AccountNo == accountNo);
        }

        public List<Account> getByType(AccountType type)
        {
            return db.Accounts.Where(a => a.Type == type).ToList();
        }

        public List<Account> getByBirthNo(string birthNo)
        {
            return db.Accounts.Where(a => a.Owner.BirthNo == birthNo).OrderBy(a => a.Type).ToList();
        }

        public List<Account> getByTypeAndBirthNo(AccountType type, string birthNo)
        {
            return db.Accounts.Where(a => a.Type == type && a.Owner.BirthNo == birthNo).ToList();
        }
    }
}