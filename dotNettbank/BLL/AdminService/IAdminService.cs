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
        bool addAccount(Account addAccount);
        bool updateCustomer(Customer updatedCustomer);
        Customer getCustomerByBirthNo(string birthNo);
        string deactivateAccount(string accountNo);
        string deactivateCustomer(string birthNo);
        bool createPayment(Payment p);
        bool deletePayment(int paymentID);
        bool addPostalArea(PostalArea postalArea);
        bool addCustomer(Customer customer);
        List<Account> getAllAccountsByBirthNo(string birthNo);
        List<Payment> getPaymentsByFromBirthNo(string birthNo);
        List<Transaction> getTransactionsByAccountNo(string accountNo);
        List<Transaction> getTransactionsByBirthNo(string birthNo);
        List<Transaction> getTransactionsByBirthNoArray(string[] birthNos);
        List<Transaction> getTransactionsByAccountNoArray(string[] accountNos);
        List<Payment> getPaymentsByFromAccountNoArray(string[] fromAccountNos);
        List<Payment> getPaymentsByFromBirthNoArray(string[] birthNos);
        List<Account> getAccountsByBirthNoArray(string[] birthNos);
        List<Z.EntityFramework.Plus.AuditEntry> getAllAuditEntries();
        List<Z.EntityFramework.Plus.AuditEntryProperty> getAllAuditEntryProperties();
        List<Z.EntityFramework.Plus.AuditEntryProperty> getAuditEntryPropertiesByEntryId(int auditEntryId);
    }
}
