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
        bool completePayment(Payment payment);
        List<Account> getAllAccounts();
        List<Transaction> getAllTransactions();
        List<Customer> getAllCustomers();
    }
}
