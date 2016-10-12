using dotNettbank.DAL.Repositories;
using dotNettbank.Models.DomainModels;
using dotNettbank.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            Customer customer = customerRepository.getCustomerByBirthNo(login.BirthNo);
            if (customer != null)
            {
                byte[] passordForTest = lagHash(login.Password + customer.Salt);
                bool passwordCorrect = customer.Password.SequenceEqual(passordForTest);
                return passwordCorrect; // Return true if password is correct, false otherwise
            }
            else
            {
                // Return false if birthNo does not match any customers in DB:
                return false;
            }
        }

        public bool registerCustomer(RegisterCustomer regCustomer)
        {
            // Generate salt and create hashed password from salt
            string salt = lagSalt();
            var passwordAndSalt = regCustomer.Password + salt;
            byte[] passwordDB = lagHash(passwordAndSalt);

            // Create new domain Customer
            var customer = new Customer();
            // Populate domain model from view model
            customer.BirthNo = regCustomer.BirthNo;
            customer.FirstName = regCustomer.FirstName;
            customer.LastName = regCustomer.LastName;
            customer.Address = regCustomer.Address;
            customer.PhoneNo = regCustomer.PhoneNo;
            customer.PostCode = regCustomer.PostCode;
            // customer.PostalArea = postalAreaRepository.addPostalArea(regCustomer.PostCode); TODO: update repository in DAL
            customer.Password = passwordDB;
            customer.Salt = salt;
            // Add customer to DB through repository:
            return customerRepository.addCustomer(customer);
        }

        private static string lagSalt()
        {
            byte[] randomArray = new byte[10];
            string randomString;

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            randomString = Convert.ToBase64String(randomArray);
            return randomString;
        }


        private static byte[] lagHash(string innStreng)
        {
            byte[] innData, utData;
            var algoritme = SHA256.Create();
            innData = Encoding.UTF8.GetBytes(innStreng);
            utData = algoritme.ComputeHash(innData);
            return utData;
        }

        public string HashStreng(string innStreng)
        {
            byte[] hash = lagHash(innStreng);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}