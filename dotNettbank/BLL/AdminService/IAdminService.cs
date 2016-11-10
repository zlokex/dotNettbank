using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AdminService
{
    public interface IAdminService
    {
        Admin getAdmin(string username);
        bool adminExists(string username);
        bool validateLogin(string username, string password);
        List<Payment> getAllPayments();
        List<Payment> getPaymentsByFromAccountNo(string fromAccountNo);
        bool completePayment(int paymentId);
        List<Account> getAllAccounts();
        List<Transaction> getAllTransactions();
        List<Customer> getAllCustomers();
        Account getAccountByAccountNo(string accountNo);
        bool updateAccount(Account updatedAccount);
        Customer getCustomerByBirthNo(string birthNo);
        string deactivateAccount(string accountNo);
        string deactivateCustomer(string birthNo);
        bool createPayment(Payment p);
    }
}
