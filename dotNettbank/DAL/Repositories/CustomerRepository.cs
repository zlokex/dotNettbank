using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class CustomerRepository
    {
        BankContext db = new BankContext();

        // GET SINGLE MODEL

        public Customer getCustomerByBirthNo(string birthNo)
        {
            // Default value for string is null (if no customer is found)
            return db.Customers.FirstOrDefault(c => c.BirthNo == birthNo);
        }


        public Customer getCustomerByLoginFields(byte[] hashedPassword, string birthNo)
        {
            return db.Customers.FirstOrDefault(
                c => c.Password == hashedPassword && c.BirthNo == birthNo);
        }

        // GET LIST OF MODELS

        public List<Customer> getAll()
        {
            return db.Customers.ToList();
        }

        // ADD / DELETE

        /// <summary>
        /// Add a customer to db from existing Customer object
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>True if succesful, false otherwise</returns>
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
                //Debug.WriteLine("DEBUG: " + e.Message);
                return false;
            }
            
        }

        /// <summary>
        /// Delete a customer from db by BirthNo
        /// </summary>
        /// <param name="birthNo"></param>
        /// <returns>True if succesful, false otherwise</returns>
        public bool deleteCustomerByBirthNo(string birthNo)
        {
            var customerToRemove = db.Customers.SingleOrDefault(c => c.BirthNo == birthNo); // Find customer to remove

            if (customerToRemove != null) // If customer with chosen birthNo exists:
            {
                db.Customers.Remove(customerToRemove);
                db.SaveChanges();
                return true;
            }
            else // If customer with selected birthNo does not exist:
            {
                return false;
            }
        }

        /// <summary>
        /// Delete a customer from db fro existing Customer object
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool deleteCustomer(Customer customer)
        {
            try
            {
                db.Customers.Attach(customer);
                db.Customers.Remove(customer);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // UPDATE

        public bool updateCustomerNoPw(Customer updatedCustomer)
        {
            try
            {
                db.Customers.Attach(updatedCustomer);

                var entry = db.Entry(updatedCustomer);
                entry.State = EntityState.Modified;

                // Verify that Password and salt has not been changed
                entry.Property(e => e.Password).IsModified = false;
                entry.Property(e => e.Salt).IsModified = false;

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool updateCustomerPassword(Customer updatedCustomer)
        {
            try
            {
                db.Customers.Attach(updatedCustomer);

                var entry = db.Entry(updatedCustomer);
                entry.State = EntityState.Modified;

                // Set only password and salt to be changed
                entry.Property(e => e.Password).IsModified = true;
                entry.Property(e => e.Salt).IsModified = true;

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /*
        public bool updateCustomerAll(Customer updatedCustomer)
        {
            try
            {
                db.Customers.Attach(updatedCustomer);

                var entry = db.Entry(updatedCustomer);
                entry.State = EntityState.Modified;

                // Verify that Password and salt has not been changed
                entry.Property(e => e.Password).IsModified = false;
                entry.Property(e => e.Salt).IsModified = false;

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        */

    }
}