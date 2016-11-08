using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AdminRepo
{
    public interface IAdminRepository
    {
        Admin getAdmin(string username);
        bool adminExists(string username);
        List<Payment> getAllPayments();
        List<Payment> getPaymentsByFromAccountNo(string fromAccountNo);
        bool completePayment(int paymentId);
        List<Transaction> getAllTransactions();
        List<Account> getAllAccounts();
        List<Customer> getAllCustomers();
        Account getAccountByAccountNo(string accountNo);
        bool updateAccount(Account updatedAccount);
        Customer getCustomerByBirthNo(string birthNo);
        bool deleteAccount(Account account);
    }
}
