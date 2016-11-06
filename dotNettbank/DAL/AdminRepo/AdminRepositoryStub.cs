﻿using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AdminRepo
{
    public class AdminRepositoryStub
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

        public bool completePayment(Payment payment)
        {
            if (payment != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



    }
}