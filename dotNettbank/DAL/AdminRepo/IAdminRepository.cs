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
        bool addAccount(Account addAccount);
        bool updateCustomer(Customer updatedCustomer);
        Customer getCustomerByBirthNo(string birthNo);
        string deactivateAccount(string accountNo);
        string deactivateCustomer(string birthNo);
        List<Account> getAllAccountsByBirthNo(string birthNo);
        List<Payment> getPaymentsByFromBirthNo(string birthNo);
        List<Transaction> getTransactionsByAccountNo(string accountNo);
        List<Transaction> getTransactionsByBirthNo(string birthNo);
        List<Transaction> getTransactionsByBirthNoArray(string[] birthNos);
        List<Transaction> getTransactionsByAccountNoArray(string[] accountNos);
        List<Payment> getPaymentsByFromAccountNoArray(string[] fromAccountNos);
        List<Payment> getPaymentsByFromBirthNoArray(string[] birthNos);
        List<Account> getAccountsByBirthNoArray(string[] birthNos);
        bool createPayment(Payment newPayment);
        bool deletePayment(int paymentID);
        bool addPostalArea(PostalArea postalArea);
        bool addCustomer(Customer customer);
        List<Z.EntityFramework.Plus.AuditEntry> getAllAuditEntries();
        List<Z.EntityFramework.Plus.AuditEntryProperty> getAllAuditEntryProperties();
        List<Z.EntityFramework.Plus.AuditEntryProperty> getAuditEntryPropertiesByEntryId(int auditEntryId);
    }
}
