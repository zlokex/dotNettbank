using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class CustomerRepository
    {
        BankContext db = new BankContext();

        public Customer getCustomerByBirthNo(string birthNo)
        {
            return db.Customers.FirstOrDefault(c => c.BirthNo == birthNo);
        }

        public Customer getCustomerByLoginFields(byte[] hashedPassword, string birthNo)
        {
            return db.Customers.FirstOrDefault(
                c => c.Password == hashedPassword && c.BirthNo == birthNo);
        }

        public bool addCustomer(Customer customer)
        {
            try
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e.Message);
                
                return false;
            }
            
        }

    }
}