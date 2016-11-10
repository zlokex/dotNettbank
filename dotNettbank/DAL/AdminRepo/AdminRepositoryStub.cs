using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AdminRepo
{
    public class AdminRepositoryStub : IAdminRepository
    {
        public Admin getAdmin(string username)
        {
            Admin admin = new Admin()
            {
                Username = username,
                Email = "admin@admin.com",
                Password = null,
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
            return null; //TODO!!!!!!!!!!
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
            throw new NotImplementedException();
        }

        public bool createPayment(Payment newPayment)
        {
            throw new NotImplementedException();
        }
    }
}
