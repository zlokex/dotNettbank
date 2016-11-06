using dotNettbank.DAL;
using dotNettbank.DAL.Repositories;
using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace dotNettbank.BLL
{
    public class BankService
    {
        BankContext db = new BankContext();

        AccountRepository accountRepository;
        CustomerRepository customerRepository;
        PaymentRepository paymentRepository;
        PostalAreaRepository postalAreaRepository;
        TransactionRepository transactionRepository;

        public BankService()
        {
            accountRepository = new AccountRepository(db);
            customerRepository = new CustomerRepository(db);
            paymentRepository = new PaymentRepository(db);
            postalAreaRepository = new PostalAreaRepository(db);
            transactionRepository = new TransactionRepository(db);
        }


        public Customer getCustomerByBirthNo(string birthNo)
        {
            return customerRepository.getCustomerByBirthNo(birthNo);
        }

        // Validate BankID (Dummy) Returns true, as long as birthNo matches a customer.
        public Boolean validateBankId(string birthNo, string bankID)
        {
            // Get customer belonging to birthNo:
            Customer c = getCustomerByBirthNo(birthNo);
            Debug.WriteLine("" + c);
            // If birthNo is valid:
            if (c != null)
            {
                // return true:
                return true;
            }
            else
            {
                // Return false if birthNo does not match a customer:
                return false;
            }
            
        }

        public List<Account> getListByBirthNo(string birthNo)
        {
            return accountRepository.getListByBirthNo(birthNo);
        }

        public bool checkValidLogin(string password, string birthNo)
        {
            Customer customer = customerRepository.getCustomerByBirthNo(birthNo);
            //Debug.WriteLine("customer:" + customer);
            if (customer != null)
            {
                byte[] passordForTest = createHash(password + customer.Salt);
                bool passwordCorrect = customer.Password.SequenceEqual(passordForTest);
                return passwordCorrect; // Return true if password is correct, false otherwise
            }
            else
            {
                // Return false if birthNo does not match any customers in DB:
                return false;
            }
        }

        public List<Account> getAllAccounts()
        {
            return accountRepository.getAll();
        }

        public bool registerCustomer(Customer customer)
        {
            // Add customer to DB through repository:
            return customerRepository.addCustomer(customer);
        }

        public bool addPostalArea(PostalArea postalArea)
        {
            return postalAreaRepository.addPostalArea(postalArea);
        }

        public bool addAccount(Account account)
        {
            return accountRepository.addAccount(account);
        }

        public bool addPayment(Payment payment)
        {
            return paymentRepository.addPayment(payment);
        }

        public bool addTransaction(Transaction transaction)
        {
            return transactionRepository.addTransaction(transaction);
        }

        public Account getByAccountNo(string accountNo)
        {
            return accountRepository.getByAccountNo(accountNo);
        }


        public List<Account> getAccountsByBirthNo(string birthNo)
        {
            return accountRepository.getListByBirthNo(birthNo);
        }

        public List<Transaction> getTransactionsByAccountNo(string accountNo)
        {
            return transactionRepository.getTransactionsByAccountNo(accountNo);
        }

        public List<Transaction> getTransactionsByBirthNo(string birthNo)
        {
            return transactionRepository.getTransactionsByBirthNo(birthNo);
        }

        public List<Payment> getDuePaymentsByAccountNo(string accountNo)
        {
            return paymentRepository.getDuePaymentsByAccountNo(accountNo);
        }

        public List<Payment> getDuePaymentsByBirthNo(string birthNo)
        {
            return paymentRepository.getDuePaymentsByBirthNo(birthNo);
        }

        public bool deletePayment(Payment payment)
        {
            return paymentRepository.deletePayment(payment);
        }

        public Payment getPaymentById(int id)
        {
            return paymentRepository.getById(id);
        }

        public bool updatePayment(Payment updatedPayment)
        {
            return paymentRepository.updatePayment(updatedPayment);
        }


        public void completePayments()
        {
            // Get list of paymnets that has passed due date:
            List<Payment> payments = paymentRepository.getPaymentsPassedDueDate();

            // From and To Accounts, used to update balance:
            List<Account> fromAccounts = new List<Account>();
            List<Account> toAccounts = new List<Account>();

            // Import these payments to transactions:
            List<Transaction> transactions = new List<Transaction>();
            DateTime currDate = DateTime.Now;
            foreach (var p in payments)
            {
                // Get from and to account:
                Account fromAcc = p.FromAccount;
                Account toAcc = p.ToAccount;
                // verify that fromAccount has enough balance:
                if (fromAcc.Balance < p.Amount)
                {
                    //Update balance to from and to account:
                    fromAcc.Balance -= p.Amount;
                    toAcc.Balance += p.Amount;
                    // Add from and to account to lists:
                    fromAccounts.Add(fromAcc);
                    toAccounts.Add(toAcc);

                    // Import to transactions:
                    Transaction t = new Transaction()
                    {
                        DatePayed = p.DueDate,
                        Date = currDate,
                        Amount = p.Amount,
                        Message = p.Message,
                        FromAccountNo = p.FromAccountNo,
                        ToAccountNo = p.ToAccountNo
                    };
                    transactions.Add(t);
                }
                else
                {
                    // If from acc does not have enough balance, remove the payment from our current list (So that it wont be deleted)
                    payments.Remove(p);
                }
            }

            // Attempt to complete payment:
            try
            {
                // Remove payments:
                paymentRepository.removePayments(payments);
                // Add to transactions:
                transactionRepository.addRangeTransactions(transactions);
            }
            catch (Exception e)
            {
                //
                //db.Entry(entity).Reload();
            }
        }

        public static string generateSalt()
        {
            byte[] randomArray = new byte[10];
            string randomString;

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            randomString = Convert.ToBase64String(randomArray);
            return randomString;
        }

        //TODO Lag en try catch for tilfellet hvor passord ikke er skrevet inn
        public static byte[] createHash(string innStreng)
        {
            byte[] innData, utData;
            var algoritme = SHA256.Create();
            innData = Encoding.UTF8.GetBytes(innStreng);
            utData = algoritme.ComputeHash(innData);
            return utData;
        }

        public string HashString(string innStreng)
        {
            byte[] hash = createHash(innStreng);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}