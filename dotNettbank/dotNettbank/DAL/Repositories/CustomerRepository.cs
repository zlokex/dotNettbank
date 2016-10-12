using dotNettbank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class CustomerRepository
    {
        BankContext db = new BankContext();

        public Customer getCustomerByBirthNo(string birthNo)
        {
            return db.Customers.Single(c => c.BirthNo == birthNo);
        }

        public Customer getCustomerByLoginFields(byte[] hashedPassword, string birthNo)
        {
            return db.Customers.FirstOrDefault(
                c => c.Password == hashedPassword && c.BirthNo == birthNo);
        }

    }
}