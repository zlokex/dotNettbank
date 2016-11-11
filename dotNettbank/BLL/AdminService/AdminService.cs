using DAL.AdminRepo;
using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AdminService
{
    public class AdminService : IAdminService
    {
        private IAdminRepository _repository;

        // --- Constructors ---

        public AdminService()
        {
            _repository = new AdminRepository();
        }

        public AdminService(IAdminRepository stub)
        {
            _repository = stub;
        }


        // -- Methods ---
        public Admin getAdmin(string username)
        {
            return _repository.getAdmin(username); 
        }

        public bool adminExists(string username)
        {
            return _repository.adminExists(username);
        }

        public bool validateLogin(string username, string password)
        {
            if (!adminExists(username)) return false;
            var temp = getAdmin(username).Password;

            if (temp != null)
            {
                byte[] passHashed = lagHash(password);
                if (temp.SequenceEqual(passHashed))
                    return true;
            }
            return false;
        }

        public List<Customer> getAllCustomers()
        {
            return _repository.getAllCustomers();
        }

        public List<Account> getAllAccounts()
        {
            return _repository.getAllAccounts();
        }

        public List<Payment> getAllPayments()
        {
            return _repository.getAllPayments();
        }

        public List<Payment> getPaymentsByFromAccountNo(string fromAccountNo)
        {
            return _repository.getPaymentsByFromAccountNo(fromAccountNo);
        }

      

        public bool completePayment(int paymentId)
        {
            return _repository.completePayment(paymentId);
        }

        private static byte[] lagHash(string innPassord)
        {
            byte[] innData, utData;
            var algoritme = System.Security.Cryptography.SHA256.Create();
            innData = System.Text.Encoding.ASCII.GetBytes(innPassord);
            utData = algoritme.ComputeHash(innData);
            return utData;
        }

        public List<Transaction> getAllTransactions()
        {
            return _repository.getAllTransactions();
        }

        public Account getAccountByAccountNo(string accountNo)
        {
            return _repository.getAccountByAccountNo(accountNo);
        }

        public bool updateAccount(Account updatedAccount)
        {
            return _repository.updateAccount(updatedAccount);
        }

        public bool addAccount(Account addAccount)
        {
            return _repository.addAccount(addAccount);
        }

        public bool updateCustomer(Customer updatedCustomer)
        {
            return _repository.updateCustomer(updatedCustomer);
        }

        public Customer getCustomerByBirthNo(string birthNo)
        {
            return _repository.getCustomerByBirthNo(birthNo);
        }

        public string deactivateAccount(string accountNo)
        {
            
            return _repository.deactivateAccount(accountNo);
        }

        public string deactivateCustomer(string birthNo)
        {
            return _repository.deactivateCustomer(birthNo);
        }

        public List<Account> getAllAccountsByBirthNo(string birthNo)
        {
            return _repository.getAllAccountsByBirthNo(birthNo);
        }

        public List<Payment> getPaymentsByFromBirthNo(string birthNo)
        {
            return _repository.getPaymentsByFromBirthNo(birthNo);
        }

        public List<Transaction> getTransactionsByAccountNo(string accountNo)
        {
            return _repository.getTransactionsByAccountNo(accountNo);
        }

        public List<Transaction> getTransactionsByBirthNo(string birthNo)
        {
            return _repository.getTransactionsByBirthNo(birthNo);
        }

        public List<Transaction> getTransactionsByBirthNoArray(string[] birthNos)
        {
            return _repository.getTransactionsByBirthNoArray(birthNos);
        }

        public List<Transaction> getTransactionsByAccountNoArray(string[] accountNos)
        {
            return _repository.getTransactionsByAccountNoArray(accountNos);
        }

        public List<Payment> getPaymentsByFromAccountNoArray(string[] fromAccountNos)
        {
            return _repository.getPaymentsByFromAccountNoArray(fromAccountNos);
        }

        public List<Payment> getPaymentsByFromBirthNoArray(string[] birthNos)
        {
            return _repository.getPaymentsByFromBirthNoArray(birthNos);
        }

        public List<Account> getAccountsByBirthNoArray(string[] birthNos)
        {
            return _repository.getAccountsByBirthNoArray(birthNos);
        }

        public bool createPayment(Payment newPayment)
        {
            return _repository.createPayment(newPayment);
        }

        public List<Z.EntityFramework.Plus.AuditEntry> getAllAuditEntries()
        {
            return _repository.getAllAuditEntries();
        }

        public List<Z.EntityFramework.Plus.AuditEntryProperty> getAllAuditEntryProperties()
        {
            return _repository.getAllAuditEntryProperties();
        }

        public List<Z.EntityFramework.Plus.AuditEntryProperty> getAuditEntryPropertiesByEntryId(int auditEntryId)
        {
            return _repository.getAuditEntryPropertiesByEntryId(auditEntryId);
        }
    }
}

