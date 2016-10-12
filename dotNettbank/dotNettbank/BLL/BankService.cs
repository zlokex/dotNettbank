using dotNettbank.DAL.Repositories;
using dotNettbank.Models;
using dotNettbank.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.BLL
{
    public class BankService
    {
        AccountRepository accountRepository;
        BankRepository bankRepository;
        CustomerRepository customerRepository;
        PaymentRepository paymentRepository;
        PostalAreaRepository postalAreaRepository;
        TransactionRepository transactionRepository;

        public BankService()
        {
            accountRepository = new AccountRepository();
            bankRepository = new BankRepository();
            customerRepository = new CustomerRepository();
            paymentRepository = new PaymentRepository();
            postalAreaRepository = new PostalAreaRepository();
            transactionRepository = new TransactionRepository();
        }

        public bool checkValidLogin(LoginViewModel login)
        {
            byte[] hashedPassword = lagHash(login.Password);
            Customer customer = customerRepository.getCustomerByLoginFields(
                hashedPassword, login.BirthNo);
            if (customer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static byte[] lagHash(string innPassord)
        {
            byte[] innData, utData;
            var algoritme = System.Security.Cryptography.SHA256.Create();
            innData = System.Text.Encoding.ASCII.GetBytes(innPassord);
            utData = algoritme.ComputeHash(innData);
            return utData;
        }
    }
}