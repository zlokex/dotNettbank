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

        public List<Account> getAllAccounts()
        {
            return _repository.getAllAccounts();
        }

        public List<Customer> getAllCustomers()
        {
            return _repository.getAllCustomers();
        }

        public List<Payment> getAllPayments()
        {
            return _repository.getAllPayments();
        }

        public List<Payment> getPaymentsByFromAccountNo(string fromAccountNo)
        {
            return _repository.getPaymentsByFromAccountNo(fromAccountNo);
        }

      

        public bool completePayment(Payment payment)
        {
            return _repository.completePayment(payment);
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
    }
}

