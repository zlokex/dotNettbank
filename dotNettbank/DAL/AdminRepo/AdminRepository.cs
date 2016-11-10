using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNettbank.Model;
using dotNettbank.DAL;
using System.Data.Entity;
using System.Diagnostics;
using DAL.Log;

namespace DAL.AdminRepo
{
    public class AdminRepository : IAdminRepository
    {
        //--- GET ONE ---
        
        public Admin getAdmin(string username)
        {
            using (var db = new BankContext())
            {

                var admin = db.Admins.Where(a => a.Username == username).First();
                return admin;
            }
        }

     
        public Account getAccountByAccountNo(string accountNo)
        {
            using (var db = new BankContext())
            {
                return db.Accounts.FirstOrDefault(a => a.AccountNo == accountNo);
            }
        }

        public Customer getCustomerByBirthNo(string birthNo)
        {
            using (var db = new BankContext())
            {
                // Default value is null (if no customer is found)
                return db.Customers.FirstOrDefault(c => c.BirthNo == birthNo);
            }
        }

        //--- GET LIST ---

        public List<Customer> getAllCustomers()
        {
            using (var db = new BankContext())
            {
                return db.Customers.Where(x => x.Active == true).ToList(); // Filter customer by only showing active customers
            }
        }

        public List<Account> getAllAccounts()
        {
            using (var db = new BankContext())
            {
                return db.Accounts.Where(x => x.Owner.Active == true && x.Active == true).ToList(); // Filter accounts by only showing active accs from active customers
            }
        }

        public List<Account> getAllAccountsByBirthNo(string birthNo)
        {
            using (var db = new BankContext())
            {
                return db.Accounts.Where(x => x.Owner.BirthNo == birthNo && x.Owner.Active == true && x.Active == true).ToList(); // Filter accounts by only showing active accs from active customers
            }
        }

        public List<Payment> getAllPayments()
        {
            using (var db = new BankContext())
            {
                return db.Payments.Where(x => x.FromAccount.Active == true).ToList(); // Filter payments by only showing payments sent from active accounts
            }
        }

        public List<Payment> getPaymentsByFromAccountNo(string fromAccountNo)
        {
            using (var db = new BankContext())
            {
                List<Payment> payments = db.Payments.Where(b => b.FromAccountNo == fromAccountNo).ToList();
                return payments;
            }
        }

        public List<Payment> getPaymentsByFromBirthNo(string birthNo)
        {
            using (var db = new BankContext())
            {
                List<Payment> payments = db.Payments.Where(t => t.FromAccount.OwnerBirthNo == birthNo && t.FromAccount.Active == true).ToList(); // Filter to only show payments from active accounts
                return payments;
            }
        }

        public List<Transaction> getAllTransactions()
        {
            using (var db = new BankContext())
            {
                return db.Transactions.ToList();
            }
        }

        // Get all sent and received transactions for one account of one person
        public List<Transaction> getTransactionsByAccountNo(string accountNo)
        {
            using (var db = new BankContext())
            {
                // Get all transactions matching from accountNo (Avsender)
                List<Transaction> transactions = db.Transactions.Where(t => t.FromAccount.AccountNo == accountNo).ToList();
                // Add all transactions matching to accountNo (Mottaker)
                transactions.AddRange(db.Transactions.Where(t => t.ToAccount.AccountNo == accountNo).ToList());
                return transactions;
            }
        }


        // Get all sent and received transactions for one person
        public List<Transaction> getTransactionsByBirthNo(string birthNo)
        {
            using (var db = new BankContext())
            {
                // Get all transactions matching from accountNo (Avsender)
                List<Transaction> transactions = db.Transactions.Where(t => t.FromAccount.Owner.BirthNo == birthNo).ToList();
                // Add all transactions matching to accountNo (Mottaker)
                transactions.AddRange(db.Transactions.Where(t => t.ToAccount.Owner.BirthNo == birthNo).ToList());
                return transactions;
            }
        }

        //--- GET LIST FROM ARRAYS ---

        public List<Account> getAccountsByBirthNoArray(string[] birthNos)
        {
            using (var db = new BankContext())
            {
                List<Account> accounts = new List<Account>();
                foreach (string birthNo in birthNos)
                {
                    List<Account> accountsTemp = db.Accounts.Where(x => x.Owner.BirthNo == birthNo && x.Owner.Active == true && x.Active == true).ToList(); // Filter accounts by only showing active accs from active customers
                    accounts.AddRange(accountsTemp);
                }
                return accounts;
            }
        }

        public List<Payment> getPaymentsByFromAccountNoArray(string[] fromAccountNos)
        {
            using (var db = new BankContext())
            {
                List<Payment> payments = new List<Payment>();
                foreach (string fromAccountNo in fromAccountNos)
                {
                    List<Payment> paymentsTemp = db.Payments.Where(b => b.FromAccountNo == fromAccountNo).ToList();
                    payments.AddRange(paymentsTemp);
                }
                return payments;
            }
        }

        public List<Payment> getPaymentsByFromBirthNoArray(string[] birthNos)
        {
            using (var db = new BankContext())
            {
                List<Payment> payments = new List<Payment>();
                foreach (string birthNo in birthNos)
                {
                    List<Payment> paymentsTemp = db.Payments.Where(t => t.FromAccount.OwnerBirthNo == birthNo && t.FromAccount.Active == true).ToList(); // Filter to only show payments from active accounts
                    payments.AddRange(paymentsTemp);
                }
                return payments;
            }
        }

        public List<Transaction> getTransactionsByBirthNoArray(string[] birthNos)
        {
            using (var db = new BankContext())
            {
                List<Transaction> transactions = new List<Transaction>();
                foreach (string birthNo in birthNos)
                {
                    List<Transaction> transactionsTemp = db.Transactions.Where(t => t.FromAccount.Owner.BirthNo == birthNo).ToList();
                    transactions.AddRange(transactionsTemp);
                }
                return transactions;
            }
        }

        public List<Transaction> getTransactionsByAccountNoArray(string[] accountNos)
        {
            using (var db = new BankContext())
            {
                List<Transaction> transactions = new List<Transaction>();
                foreach (string accountNo in accountNos)
                {
                    List<Transaction> transactionsTemp = db.Transactions.Where(t => t.FromAccount.AccountNo == accountNo).ToList();
                    transactions.AddRange(transactionsTemp);
                }
                return transactions;
            }
        }

        //--- UPDATE ---

        public bool updateAccount(Account updatedAccount)
        {
            using (var db = new BankContext())
            {
                try
                {
                    db.Accounts.Attach(updatedAccount);

                    var entry = db.Entry(updatedAccount);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    string log = "Failed to update account.\t" + e.Message + "\t" + e.StackTrace.ToString();
                    Debug.Write(log);
                    new LogErrors().errorLog(log);
                    return false;
                }
            }
        }

        public bool updateCustomer(Customer updatedCustomer)
        {
            using (var db = new BankContext())
            {
                try
                {
                    db.Customers.Attach(updatedCustomer);

                    var entry = db.Entry(updatedCustomer);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    string log = "Failed to update customer.\t" + e.Message + "\t" + e.StackTrace.ToString();
                    Debug.Write(log);
                    new LogErrors().errorLog(log);
                    return false;
                }
            }
        }

        //--- DELETE ---

        public string deactivateAccount(string accountNo)
        {
            using (var db = new BankContext())
            {
                Account account = getAccountByAccountNo(accountNo);
                if (account == null) { return "Konto eksisterer ikke"; }
                if (account.Balance > 0)
                {
                    return "Kan ikke deaktivere konto med saldo over 0";
                }
                try
                {
                    // Set active for account to false:
                    account.Active = false;

                    // Update record:
                    db.Accounts.Attach(account);
                    db.Entry(account).Property(x => x.Active).IsModified = true; // Update only Active property
                    
                    // Save changes:
                    db.SaveChanges();
                    return "Suksess";
                }
                catch (Exception e)
                {
                    string log = "Failed to deactivate account.\t" + e.Message + "\t" + e.StackTrace.ToString();
                    Debug.Write(log);
                    new LogErrors().errorLog(log);
                    return "Klarte ikke å deaktivere konto";
                }

            }
        }

        public string deactivateCustomer(string birthNo)
        {
            using (var db = new BankContext())
            {
                Customer customer = getCustomerByBirthNo(birthNo);
                if (customer == null) { return "Kunde eksisterer ikke"; }
                // Get accounts to customer:
                List<Account> accounts = db.Accounts.Where(x => x.Owner.BirthNo == birthNo).ToList();
                foreach (Account account in accounts)
                {
                    // Check if any of the accounts has any balance:
                    if (account.Balance > 0)
                    {
                        return "Kan ikke deaktivere kunde. Kunden har kontoer med saldo over 0.";
                    }
                }
                

                try
                {
                    // Set active for account to false:
                    customer.Active = false;

                    // Update record:
                    db.Customers.Attach(customer);
                    db.Entry(customer).Property(x => x.Active).IsModified = true; // Update only Active property

                    // Save changes:
                    db.SaveChanges();
                    return "Suksess";
                }
                catch (Exception e)
                {
                    string log = "Failed to deactivate customer.\t" + e.Message + "\t" + e.StackTrace.ToString();
                    Debug.Write(log);
                    new LogErrors().errorLog(log);
                    return "Klarte ikke å deaktivere kunde";
                }

            }
        }

        //--- MISC ---

        public bool adminExists(string username)
        {
            using (var db = new BankContext())
            {
                Admin foundAdmin = db.Admins.FirstOrDefault
                (a => a.Username == username);
                if (foundAdmin == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool completePayment(int paymentId)
        {

            using (var db = new BankContext())
            {

                Payment payment = db.Payments.FirstOrDefault(p => p.PaymentID == paymentId);
                // Hent fra konto og til konto for betaling (for å oppdatere balanse):
                Account fromAcc = payment.FromAccount;
                Account toAcc = payment.ToAccount;

                // Sjekk at fraKonto har høy nok saldo:
                if (fromAcc.Balance >= payment.Amount)
                {
                    //Update balance to from and to account:
                    fromAcc.Balance -= payment.Amount;
                    toAcc.Balance += payment.Amount;

                    // Create a new transaction:
                    DateTime currDate = DateTime.Now;
                    Transaction transaction = new Transaction()
                    {
                        DatePayed = payment.DueDate,
                        Date = currDate,
                        Amount = payment.Amount,
                        Message = payment.Message,
                        FromAccountNo = payment.FromAccountNo,
                        ToAccountNo = payment.ToAccountNo
                    };

                    // Attempt to complete payment:
                    try
                    {

                        // Add transaction:
                        db.Transactions.Add(transaction);

                        // Remove payment:
                        db.Payments.Attach(payment);
                        db.Payments.Remove(payment);

                        // Update balances of from account:
                        db.Accounts.Attach(fromAcc);
                        var entryFromAcc = db.Entry(fromAcc);
                        entryFromAcc.State = EntityState.Modified;

                        // Set only balance to be changed
                        entryFromAcc.Property(e => e.Balance).IsModified = true;

                        // Update balances of to account:
                        db.Accounts.Attach(toAcc);
                        var entryToAcc = db.Entry(toAcc);
                        entryToAcc.State = EntityState.Modified;

                        // Set only balance to be changed
                        entryToAcc.Property(e => e.Balance).IsModified = true;

                        // Save changes:
                        db.SaveChanges();
                        
                        // Succesful, return true:
                        return true;

                    }
                    catch (Exception e)
                    {
                        string log = "Failed to complete payment.\t" + e.Message + "\t" + e.StackTrace.ToString();
                        Debug.Write(log);
                        new LogErrors().errorLog(log);
                    }
                }
                return false;
            }
        }

        public bool createPayment(Payment newPayment)
        {
            using (var db = new BankContext())
            {
                try
                {
                    db.Payments.Add(newPayment);
                    db.SaveChanges();
                    return true;
                }
                catch(Exception e)
                {
                    Debug.WriteLine("DEBUG: " + e.Message);
                    return false;

                }
            }
        }
    }
}

