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

        public List<Account> getAllAccounts()
        {
            return null;
        }
    }
}