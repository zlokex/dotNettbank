using DAL.Log;
using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AdminRepo
{
    public class AdminRepositoryStub : IAdminRepository
    {
        public Admin getAdmin(string username)
        {

            var password = lagHash("admin");
            Admin admin = new Admin()
            {
                Username = username,
                Email = "admin@admin.com",
                Password = password,
                Salt = "salt"
            };
            return admin;
        }

        public bool adminExists(string username)
        {
            if (username != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    

        public List<Customer> getAllCustomers()
        {
            var customers = new List<Customer>();

            var salt = "salt";
            var passwordAndSalt = "Test123salt";
            var password = createHash(passwordAndSalt);

            var customer = new Customer()
            {
                Active = true,
                Address = "Storgata 83",
                BirthNo = "0101891245",
                FirstName = "André",
                LastName = "Hovda",
                Password = password,
                Salt = salt,
                PhoneNo = "94486775",
                PostCode = "0182"
            };

            customers.Add(customer);
            customers.Add(customer);
            customers.Add(customer);

            return customers;
        }

        public List<Payment> getAllPayments()
        {
            var currDate = DateTime.Now;
            var payments = new List<Payment>();
            var payment = new Payment()
            {
                Amount = 100,
                DateAdded = currDate,
                DueDate = currDate,
                FromAccountNo = "1000000000",
                ToAccountNo = "1000000009",
                Message = "Test",
                PaymentID = 1
            };
            payments.Add(payment);
            payments.Add(payment);
            payments.Add(payment);

            return payments;
        }

        public List<Payment> getPaymentsByFromAccountNo(string fromAccountNo)
        {
            var currDate = DateTime.Now;
            var payments = new List<Payment>();
            var payment = new Payment()
            {
                Amount = 100,
                DateAdded = currDate,
                DueDate = currDate,
                FromAccountNo = fromAccountNo,
                ToAccountNo = "1000000009",
                Message = "Test",
                PaymentID = 1
            };
            payments.Add(payment);
            payments.Add(payment);
            payments.Add(payment);

            return payments;
        }

        public bool completePayment(int paymentId)
        {
            if (paymentId != -1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<Transaction> getAllTransactions()
        {
            throw new NotImplementedException();
        }

        public List<Account> getAllAccounts()
        {
            throw new NotImplementedException();
        }

        public Account getAccountByAccountNo(string accountNo)
        {
            throw new NotImplementedException();
        }

        public bool addAccount(Account adddAccount)
        {
            throw new NotImplementedException();
        }

        public bool updateAccount(Account updatedAccount)
        {
            throw new NotImplementedException();
        }

        public bool updateCustomer(Customer updatedCustomer)
        {
            throw new NotImplementedException();
        }

        public Customer getCustomerByBirthNo(string birthNo)
        {
            throw new NotImplementedException();
        }

        public string deactivateAccount(string accountNo)
        {
            throw new NotImplementedException();
        }

        public string deactivateCustomer(string birthNo)
        {
            throw new NotImplementedException();
        }

        

        public List<Transaction> getTransactionsByBirthNoArray(string[] birthNos)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> getTransactionsByAccountNoArray(string[] accountNos)
        {
            throw new NotImplementedException();
        }

        public List<Payment> getPaymentsByFromAccountNoArray(string[] fromAccountNos)
        {
            throw new NotImplementedException();
        }

        public List<Payment> getPaymentsByFromBirthNoArray(string[] birthNos)
        {
            throw new NotImplementedException();
        }

        public List<Account> getAccountsByBirthNoArray(string[] birthNos)
        {
            var accounts = new List<Account>();

            var account = new Account()
            {
                AccountNo = "12341212345",
                Active = true,
                Balance = 100,
                Type = "BSU",
                OwnerBirthNo = "01018912345"
            };
            accounts.Add(account);
            accounts.Add(account);
            accounts.Add(account);

            return accounts;
        }

        public bool createPayment(Payment newPayment)
        {
            throw new NotImplementedException();
        }

        public List<Z.EntityFramework.Plus.AuditEntry> getAllAuditEntries()
        {
            throw new NotImplementedException();
        }

        public List<Z.EntityFramework.Plus.AuditEntryProperty> getAllAuditEntryProperties()
        {
            throw new NotImplementedException();
        }

        public List<Z.EntityFramework.Plus.AuditEntryProperty> getAuditEntryPropertiesByEntryId(int auditEntryId)
        {
            throw new NotImplementedException();
        }

        public bool addPostalArea(PostalArea postalArea)
        {
            throw new NotFiniteNumberException();
        }

        public bool addCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }




        private static byte[] lagHash(string innPassord)
        {
            byte[] innData, utData;
            var algoritme = System.Security.Cryptography.SHA256.Create();
            innData = System.Text.Encoding.ASCII.GetBytes(innPassord);
            utData = algoritme.ComputeHash(innData);
            return utData;
        }

        public static byte[] createHash(string innStreng)
        {
            try
            {
                byte[] innData, utData;
                var algoritme = SHA256.Create();
                innData = Encoding.UTF8.GetBytes(innStreng);
                utData = algoritme.ComputeHash(innData);
                return utData;
            }
            catch (NullReferenceException e)
            {
                string log = "Failed to create hash.\t" + e.Message + "\t" + e.StackTrace.ToString();
                Debug.Write(log);
                new LogErrors().errorLog(log);
                return new byte[0];
            }
        }


        /* ---------------------------*/
        /*     NOT CURRENTLY IN USE   */
        /* ---------------------------*/

        public List<Account> getAllAccountsByBirthNo(string birthNo)
        {
            throw new NotImplementedException();
        }

        public List<Payment> getPaymentsByFromBirthNo(string birthNo)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> getTransactionsByAccountNo(string accountNo)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> getTransactionsByBirthNo(string birthNo)
        {
            throw new NotImplementedException();
        }

    }
}
