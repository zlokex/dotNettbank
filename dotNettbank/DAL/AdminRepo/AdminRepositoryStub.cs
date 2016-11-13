using DAL.Log;
using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

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
                BirthNo = "01018912345",
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
            var payments = new List<Payment>();
            var date = new DateTime(2010, 1, 18);
            var payment = new Payment()
            {
                Amount = 100,
                DateAdded = date,
                DueDate = date,
                FromAccountNo = "10000000000",
                ToAccountNo = "10000000009",
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
            var date = new DateTime(2010, 1, 18);
            var paymentTooMuchAmount = new Payment()
            {
                Amount = 1000000,
                DateAdded = date,
                DueDate = date,
                FromAccountNo = "10000000000",
                ToAccountNo = "10000000009",
                Message = "Test",
                PaymentID = 100
            };
            if (paymentTooMuchAmount.PaymentID == paymentId)
            {
                return false;
            }
            else
            {
                var payments = getAllPayments();
                foreach (var payment in payments)
                {
                    if (payment.PaymentID == paymentId)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool createPayment(Payment newPayment)
        {
            // If fromaccountNo to payment exists return true, false otherwise
            var accounts = getAllAccounts();
            foreach (var account in accounts)
            {
                if (account.AccountNo == newPayment.FromAccountNo)
                {
                    return true;
                }
            }
            return false;
        }

        public bool deletePayment(int paymentID)
        {
            // If payment exists, return true, if not return false:
            var payments = getAllPayments();
            foreach (var payment in payments)
            {
                if (payment.PaymentID == paymentID)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Transaction> getAllTransactions()
        {
            var transactions = new List<Transaction>();
            var date = new DateTime(2010, 1, 18);
            int testObjects = 20;
           
            for(int i = 0; i < testObjects; i++)
            {
                var temp = new Transaction()
                {
                    TransactionID = i,
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5*i),
                    Message = "Test"
                };
                transactions.Add(temp);
            }

            return transactions;
        }

        public List<Account> getAllAccounts()
        {
            var accounts= new List<Account>();
            int testObjects = 20;
            for (int i = 0; i < testObjects; i++)
            {
                var temp = new Account()
                {
                   AccountNo = "1000000000" + i,
                   Balance = 100+(50*i),
                   Type = "TestKonto",
                   Name = null,
                   Active = true,
                   InterestRate = 1.2,
                   OwnerBirthNo = "01018912345"
                };
                accounts.Add(temp);
            }

            return accounts;
        }

        public Account getAccountByAccountNo(string accountNo)
        {
            return new Account()
            {
                AccountNo = accountNo,
                Balance = 150,
                Type = "TestKonto",
                Name = null,
                Active = true,
                InterestRate = 1.2,
                OwnerBirthNo = "01018912345"
            };
        }

        public bool addAccount(Account addAccount)
        {
            // If account exists, return true, if not return false:
            var accounts = getAllAccounts();
            foreach (var account in accounts)
            {
                if (account.AccountNo == addAccount.AccountNo)
                {
                    return true;
                }
            }
            return false;
        }

        public bool updateAccount(Account updatedAccount)
        {
            // If account exists, return true, if not return false:
            var accounts = getAllAccounts();
            foreach (var account in accounts)
            {
                if (account.AccountNo == updatedAccount.AccountNo)
                {
                    return true;
                }
            }
            return false;
        }

        public bool updateCustomer(Customer updatedCustomer)
        {
            // If customer exists, return true, if not return false:
            var customers = getAllCustomers();
            foreach (var customer in customers)
            {
                if (customer.BirthNo == updatedCustomer.BirthNo)
                {
                    return true;
                }
            }
            return false;

        }

        public Customer getCustomerByBirthNo(string birthNo)
        {
            var salt = "salt";
            var passwordAndSalt = "Test123salt";

            return new Customer()
            {
                BirthNo = birthNo,
                FirstName = "Ola",
                LastName = "Nordmann",
                Address = "Testveien 3a",
                PostCode = "0182",
                PhoneNo = "11223344",
                Password = createHash(passwordAndSalt),
                Salt = salt,
                Active = true

            };
        }

        public string deactivateAccount(string accountNo)
        {
            bool exists = false;
            var accounts = getAllAccounts();
            foreach (var account in accounts)
            {
                if (account.AccountNo == accountNo)
                {
                    exists = true;
                }
            }
            if (accountNo == null) { return "Klarte ikke å deaktivere konto"; }
            else if (exists) { return "Suksess"; }
            else { return "Konto eksisterer ikke"; }
        }

        public string deactivateCustomer(string birthNo)
        {
            bool exists = false;
            var customers = getAllCustomers();
            foreach (var customer in customers)
            {
                if (customer.BirthNo == birthNo)
                {
                    exists = true;
                }
            }
            if (birthNo == null) { return "Klarte ikke å deaktivere kunde"; }
            else if (exists) { return "Suksess"; }
            else { return "Kunde eksisterer ikke"; }
        }


        public List<Transaction> getTransactionsByBirthNoArray(string[] birthNos)
        {
            var transactions = new List<Transaction>();
            var date = new DateTime(2010, 1, 18);
            int j = 0;
            foreach (var birthNo in birthNos)
            {
                var temp = new Transaction()
                {
                    TransactionID = j,
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * j),
                    Message = "Test"
                };
                transactions.Add(temp);
                j++;
            }

            return transactions;
        }

        public List<Transaction> getTransactionsByAccountNoArray(string[] accountNos)
        {
            var transactions = new List<Transaction>();
            var date = new DateTime(2010, 1, 18);
            int i = 0;
            foreach (var accountNo in accountNos)
            {
                var from = new Transaction()
                {
                    TransactionID = i++,
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * i),
                    Message = "Test",
                    FromAccountNo = accountNo,
                    ToAccountNo = "10000000000"
                };
                var to = new Transaction()
                {
                    TransactionID = i++,
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * i),
                    Message = "Test",
                    FromAccountNo = "10000000000",
                    ToAccountNo = accountNo
                };
                transactions.Add(from);
                transactions.Add(to);
            }

            return transactions;
        }

        public List<Payment> getPaymentsByFromAccountNoArray(string[] fromAccountNos)
        {
            var date = new DateTime(2010, 1, 18);
            var payments = new List<Payment>();
            foreach (var fromAccountNo in fromAccountNos)
            {
                var payment = new Payment()
                {
                    Amount = 100,
                    DateAdded = date,
                    DueDate = date,
                    FromAccountNo = fromAccountNo,
                    ToAccountNo = "10000000009",
                    Message = "Test",
                    PaymentID = 1
                };
                payments.Add(payment);
            }

            return payments;
        }

        public List<Payment> getPaymentsByFromBirthNoArray(string[] birthNos)
        {
            var date = new DateTime(2010, 1, 18);
            var payments = new List<Payment>();
            foreach (var birthNo in birthNos)
            {
                var payment = new Payment()
                {
                    Amount = 100,
                    DateAdded = date,
                    DueDate = date,
                    FromAccountNo = "10000000001",
                    ToAccountNo =   "10000000009",
                    Message = "Test",
                    PaymentID = 1
                };
                payments.Add(payment);
            }

            return payments;
        }

        public List<Account> getAccountsByBirthNoArray(string[] birthNos)
        {
            var accounts = new List<Account>();

            foreach (var birthNo in birthNos)
            {
                var account = new Account()
                {
                    AccountNo = "12341212345",
                    Active = true,
                    Balance = 100,
                    Type = "BSU",
                    OwnerBirthNo = birthNo,
                    InterestRate = 1.2,
                    Name = null
                };
                accounts.Add(account);
            }
            return accounts;
        }

        

        public List<Z.EntityFramework.Plus.AuditEntry> getAllAuditEntries()
        {
            var entries = new List<AuditEntry>();
            var date = new DateTime(2010, 1, 18);
            for (int i = 0; i < 5; i++) {
                var entry = new AuditEntry()
                {
                    AuditEntryID = i,
                    CreatedDate = date,
                    EntityTypeName = "Customer",
                    State = 0,
                    StateName = "EntityAdded",
                };
                entries.Add(entry);
            }
            return entries;
        }

        public List<Z.EntityFramework.Plus.AuditEntryProperty> getAllAuditEntryProperties()
        {
            var entries = getAllAuditEntries();
            var properties = new List<AuditEntryProperty>();
            int i = 0;
            foreach (var entry in entries)
            {
                var property = new AuditEntryProperty()
                {
                    AuditEntryID = entry.AuditEntryID,
                    AuditEntryPropertyID = i++,
                    PropertyName = "FirstName",
                    OldValue = null,
                    NewValue = "Hans"
                };
                var property2 = new AuditEntryProperty()
                {
                    AuditEntryID = entry.AuditEntryID,
                    AuditEntryPropertyID = i++,
                    PropertyName = "LastName",
                    OldValue = null,
                    NewValue = "Hansen"
                };
                properties.Add(property);
                properties.Add(property2);
            }
            return properties;
        }

        public List<Z.EntityFramework.Plus.AuditEntryProperty> getAuditEntryPropertiesByEntryId(int auditEntryId)
        {
            var properties = getAllAuditEntryProperties();

            var propertiesToSend = new List<AuditEntryProperty>();

            foreach (var property in properties)
            {
                if (property.AuditEntryID == auditEntryId)
                {
                    propertiesToSend.Add(property);
                }
            }
            return propertiesToSend;
        }

        public bool addPostalArea(PostalArea postalArea)
        {
            if(postalArea != null)return true;
            return false;
        }

        public bool addCustomer(Customer customer)
        {
            var customers = getAllCustomers();

            foreach (var c in customers)
            {
                if (c.BirthNo == customer.BirthNo)
                {
                    return false;
                }
            }
            if (customer != null) return true;
            return false;
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
                ToAccountNo = "10000000009",
                Message = "Test",
                PaymentID = 1
            };
            payments.Add(payment);
            payments.Add(payment);
            payments.Add(payment);

            return payments;
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
