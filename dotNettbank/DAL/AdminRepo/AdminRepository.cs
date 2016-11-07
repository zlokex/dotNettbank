using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNettbank.Model;
using dotNettbank.DAL;
using System.Data.Entity;
using System.Diagnostics;

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

        //--- GET LIST ---

        public List<Account> getAllAccounts()
        {
            using (var db = new BankContext())
            {
                return db.Accounts.ToList();
            }
        }

        public List<Customer> getAllCustomers()
        {
            using (var db = new BankContext())
            {
                return db.Customers.ToList();
            }
        }

        public List<Payment> getAllPayments()
        {
            using (var db = new BankContext())
            {
                return db.Payments.ToList();
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

        public List<Transaction> getAllTransactions()
        {
            using (var db = new BankContext())
            {
                return db.Transactions.ToList();
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
                    return false;
                }
            }
        }

        //--- DELETE ---

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

        public bool completePayment(Payment payment)
        {
            using (var db = new BankContext())
            {
                // Hent fra konto og til konto for betaling (for å oppdatere balanse):
                Account fromAcc = payment.FromAccount;
                Account toAcc = payment.ToAccount;

                // Sjekk at fraKonto har høy nok saldo:
                if (fromAcc.Balance <= payment.Amount)
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
                        Debug.WriteLine("DEBUG: " + e.Message);
                        return false;
                    }
                }
                return false;
            }
        }
        
    }
}

