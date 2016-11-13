using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotNettbankAdmin.Controllers;
using BLL.AdminService;
using DAL.AdminRepo;
using System.Web.Mvc;
using System.Collections.Generic;
using dotNettbank.Model;
using System.Text;
using DAL.Log;
using System.Diagnostics;
using System.Security.Cryptography;
using dotNettbankAdmin.Models;
using System.Linq;
using MvcContrib.TestHelper;

namespace dotNettbankAdmin.Tests
{
    [TestClass]
    public class AdminControllerTest
    {
        [TestMethod]
        public void AdminSide()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));
            // Act:
            var actual = (ViewResult) controller.AdminSide();

            // Assert:
            Assert.AreEqual("", actual.ViewName);
        }

        [TestMethod]
        public void AdminSide_Session_LoggedOut()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult) controller.AdminSide();

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void AdminSide_Session_LoggedIn()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = "admin";

            // Act:
            var actual = (ViewResult) controller.AdminSide();

            // Assert:
            Assert.IsNotNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("", actual.ViewName);
            Assert.AreEqual("admin", actual.ViewData["UserName"]);
            Assert.AreEqual("admin@admin.com", actual.ViewData["Email"]);
        }



        [TestMethod]
        public void Login()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            // Act:
            var result = controller.Login("admin", "admin");

            // Assert:
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Login_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);

            // Act:
            var actual = controller.Login("admin", "admin");

            // Assert:
            Assert.IsNotNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
        }

        [TestMethod]
        public void LoginEmpty()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            // Act:
            var result = controller.Login("", "");

            // Assert:
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Logout()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            // Act:
            var result = (RedirectToRouteResult) controller.Logout();

            //result.RouteValues["action"].Equals("Index");
            //result.RouteValues["controller"].Equals("Index");

            // Assert:
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Index", result.RouteValues["Controller"]);
        }

        [TestMethod]
        public void Logout_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to not null first:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = "admin";

            // Act:
            var actual = (RedirectToRouteResult)controller.Logout();

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void FindCustomers()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expectedCustomers = new List<ExtendedCustomerVM>();

            var salt = "salt";
            var passwordAndSalt = "Test123salt";
            var password = createHash(passwordAndSalt);

            var customer = new ExtendedCustomerVM()
            {
                Address = "Storgata 83",
                BirthNo = "01018912345",
                FirstName = "André",
                LastName = "Hovda",
                PhoneNo = "94486775",
                PostCode = "0182",
                PostalArea = "Oslo"
            };

            expectedCustomers.Add(customer);
            expectedCustomers.Add(customer);
            expectedCustomers.Add(customer);

            string[] birthNo = new string[] { "01018912345", "010189112233", "01018911111" };

            // Act:
            var result = (PartialViewResult)controller.FindCustomers(birthNo, birthNo);
            var modelResult = (List<ExtendedCustomerVM>)result.Model;

            //result.RouteValues["action"].Equals("Index");
            //result.RouteValues["controller"].Equals("Index");

            // Assert:
            Assert.AreEqual("_FindCustomers", result.ViewName);
            
            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedCustomers[i].Address, modelResult[i].Address);
                Assert.AreEqual(expectedCustomers[i].BirthNo, modelResult[i].BirthNo);
                Assert.AreEqual(expectedCustomers[i].FirstName, modelResult[i].FirstName);
                Assert.AreEqual(expectedCustomers[i].LastName, modelResult[i].LastName);
                Assert.AreEqual(expectedCustomers[i].PhoneNo, modelResult[i].PhoneNo);
                Assert.AreEqual(expectedCustomers[i].PostCode, modelResult[i].PostCode);
                Assert.AreEqual(expectedCustomers[i].PostalArea, modelResult[i].PostalArea);
            }
        }

        [TestMethod]
        public void FindCustomers_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.FindCustomers(null, null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }


        [TestMethod]
        public void AccountsNoCustomerSelected()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expectedAccounts = new List<Account>();
            int testObjects = 20;
            for (int i = 0; i < testObjects; i++)
            {
                var temp = new Account()
                {
                    AccountNo = "1000000000" + i,
                    Balance = 100 + (50 * i),
                    Type = "TestKonto",
                    Name = null,
                    Active = true,
                    InterestRate = 1.2,
                    OwnerBirthNo = "01018912345"
                };
                expectedAccounts.Add(temp);
            }

            string[] birthNo = new string[] { "01018912345", "010189112233", "01018911111" };
            string[] accountNo = new string[] { "10000000000", "12341212345", "98769898765" };

            // Act:
            var result = (PartialViewResult)controller.Accounts(null, accountNo);
            var modelResult = (List<Account>)result.Model;

            // Assert:
            Assert.AreEqual("_Accounts", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedAccounts[i].Active, modelResult[i].Active);
                Assert.AreEqual(expectedAccounts[i].AccountNo, modelResult[i].AccountNo);
                Assert.AreEqual(expectedAccounts[i].Type, modelResult[i].Type);
                Assert.AreEqual(expectedAccounts[i].Name, modelResult[i].Name);
                Assert.AreEqual(expectedAccounts[i].InterestRate, modelResult[i].InterestRate);
                Assert.AreEqual(expectedAccounts[i].OwnerBirthNo, modelResult[i].OwnerBirthNo);
            }
        }

        [TestMethod]
        public void Accounts_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.Accounts(null, null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void Accounts_CustomersSelected()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expectedAccounts = new List<Account>();

            string[] birthNos = new string[] { "01018912345", "010189112233", "01018911111" };
            string[] accountNos = new string[] { "10000000000", "12341212345", "98769898765" };

            foreach (var birthNo in birthNos)
            {
                var account = new Account()
                {
                    AccountNo = "12341212345",
                    Active = true,
                    Balance = 100,
                    Type = "BSU",
                    OwnerBirthNo = birthNo,
                    InterestRate = 1.2,
                    Name = null
                };
                expectedAccounts.Add(account);
            }
           
            // Act:
            var result = (PartialViewResult)controller.Accounts(birthNos, accountNos);
            var modelResult = (List<Account>)result.Model;

            // Assert:
            Assert.AreEqual("_Accounts", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedAccounts[i].Active, modelResult[i].Active);
                Assert.AreEqual(expectedAccounts[i].AccountNo, modelResult[i].AccountNo);
                Assert.AreEqual(expectedAccounts[i].Type, modelResult[i].Type);
                Assert.AreEqual(expectedAccounts[i].Name, modelResult[i].Name);
                Assert.AreEqual(expectedAccounts[i].InterestRate, modelResult[i].InterestRate);
                Assert.AreEqual(expectedAccounts[i].OwnerBirthNo, modelResult[i].OwnerBirthNo);
            }
        }


        [TestMethod]
        public void Payments_NoTags()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expectedPayments = new List<PaymentRow>();

            var date = new DateTime(2010, 1, 18);
            var payment = new PaymentRow()
            {
                Amount = 100,
                DateAdded = date,
                DueDate = date,
                FromAccountNo = "10000000000",
                ToAccountNo = "10000000009",
                Message = "Test",
                PaymentID = 1
            };
            expectedPayments.Add(payment);
            expectedPayments.Add(payment);
            expectedPayments.Add(payment);

            // Act:
            var result = (PartialViewResult)controller.RegBetaling(null, null);
            var modelResult = (List<PaymentRow>)result.Model;

            // Assert:
            Assert.AreEqual("_RegBetalingPartial", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Amount, modelResult[i].Amount);
                Assert.AreEqual(expectedPayments[i].DateAdded, modelResult[i].DateAdded);
                Assert.AreEqual(expectedPayments[i].DueDate, modelResult[i].DueDate);
                Assert.AreEqual(expectedPayments[i].FromAccountNo, modelResult[i].FromAccountNo);
                Assert.AreEqual(expectedPayments[i].ToAccountNo, modelResult[i].ToAccountNo);
                Assert.AreEqual(expectedPayments[i].Message, modelResult[i].Message);
                Assert.AreEqual(expectedPayments[i].PaymentID, modelResult[i].PaymentID);
            }
        }

        [TestMethod]
        public void Payments_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.RegBetaling(null, null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void Payments_CustomersSelected()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string[] birthNos = new string[] { "01018912345", "010189112233", "01018911111" };
            string[] accountNos = new string[] { "10000000000", "12341212345", "98769898765" };

            var expectedPayments = new List<PaymentRow>();
            var date = new DateTime(2010, 1, 18);
            foreach (var birthNo in birthNos)
            {
                var payment = new PaymentRow()
                {
                    Amount = 100,
                    DateAdded = date,
                    DueDate = date,
                    FromAccountNo = "10000000001",
                    ToAccountNo = "10000000009",
                    Message = "Test",
                    PaymentID = 1
                };
                expectedPayments.Add(payment);
            }

            // Act:
            var result = (PartialViewResult)controller.RegBetaling(birthNos, null);
            var modelResult = (List<PaymentRow>)result.Model;

            // Assert:
            Assert.AreEqual("_RegBetalingPartial", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Amount, modelResult[i].Amount);
                Assert.AreEqual(expectedPayments[i].DateAdded, modelResult[i].DateAdded);
                Assert.AreEqual(expectedPayments[i].DueDate, modelResult[i].DueDate);
                Assert.AreEqual(expectedPayments[i].FromAccountNo, modelResult[i].FromAccountNo);
                Assert.AreEqual(expectedPayments[i].ToAccountNo, modelResult[i].ToAccountNo);
                Assert.AreEqual(expectedPayments[i].Message, modelResult[i].Message);
                Assert.AreEqual(expectedPayments[i].PaymentID, modelResult[i].PaymentID);
            }
        }

        [TestMethod]
        public void Payments_AccountsSelected()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string[] birthNos = new string[] { "01018912345", "010189112233", "01018911111" };
            string[] accountNos = new string[] { "10000000000", "12341212345", "98769898765" };

            var date = new DateTime(2010, 1, 18);
            var expectedPayments = new List<PaymentRow>();
            foreach (var fromAccountNo in accountNos)
            {
                var payment = new PaymentRow()
                {
                    Amount = 100,
                    DateAdded = date,
                    DueDate = date,
                    FromAccountNo = fromAccountNo,
                    ToAccountNo = "10000000009",
                    Message = "Test",
                    PaymentID = 1
                };
                expectedPayments.Add(payment);
            }

            // Act:
            var result = (PartialViewResult)controller.RegBetaling(null, accountNos);
            var modelResult = (List<PaymentRow>)result.Model;

            // Assert:
            Assert.AreEqual("_RegBetalingPartial", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Amount, modelResult[i].Amount);
                Assert.AreEqual(expectedPayments[i].DateAdded, modelResult[i].DateAdded);
                Assert.AreEqual(expectedPayments[i].DueDate, modelResult[i].DueDate);
                Assert.AreEqual(expectedPayments[i].FromAccountNo, modelResult[i].FromAccountNo);
                Assert.AreEqual(expectedPayments[i].ToAccountNo, modelResult[i].ToAccountNo);
                Assert.AreEqual(expectedPayments[i].Message, modelResult[i].Message);
                Assert.AreEqual(expectedPayments[i].PaymentID, modelResult[i].PaymentID);
            }
        }

        [TestMethod]
        public void Payments_AccountsAndCustomersSelected()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string[] birthNos = new string[] { "01018912345", "010189112233", "01018911111" };
            string[] accountNos = new string[] { "10000000000", "12341212345", "98769898765" };

            var date = new DateTime(2010, 1, 18);
            var expectedPayments = new List<PaymentRow>();
            foreach (var fromAccountNo in accountNos)
            {
                var payment = new PaymentRow()
                {
                    Amount = 100,
                    DateAdded = date,
                    DueDate = date,
                    FromAccountNo = fromAccountNo,
                    ToAccountNo = "10000000009",
                    Message = "Test",
                    PaymentID = 1
                };
                expectedPayments.Add(payment);
            }

            // Act:
            var result = (PartialViewResult)controller.RegBetaling(birthNos, accountNos);
            var modelResult = (List<PaymentRow>)result.Model;

            // Assert:
            Assert.AreEqual("_RegBetalingPartial", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Amount, modelResult[i].Amount);
                Assert.AreEqual(expectedPayments[i].DateAdded, modelResult[i].DateAdded);
                Assert.AreEqual(expectedPayments[i].DueDate, modelResult[i].DueDate);
                Assert.AreEqual(expectedPayments[i].FromAccountNo, modelResult[i].FromAccountNo);
                Assert.AreEqual(expectedPayments[i].ToAccountNo, modelResult[i].ToAccountNo);
                Assert.AreEqual(expectedPayments[i].Message, modelResult[i].Message);
                Assert.AreEqual(expectedPayments[i].PaymentID, modelResult[i].PaymentID);
            }
        }

        [TestMethod]
        public void Transactions_NoTags()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            //string[] birthNos = new string[] { "01018912345", "010189112233", "01018911111" };
            //string[] accountNos = new string[] { "10000000000", "12341212345", "98769898765" };

            var expectedTransactions = new List<TransactionRow>();
            var date = new DateTime(2010, 1, 18);
            int testObjects = 20;

            for (int i = 0; i < testObjects; i++)
            {
                var temp = new TransactionRow()
                {
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * i),
                    Message = "Test"
                };
                expectedTransactions.Add(temp);
            }

            // Act:
            var result = (PartialViewResult)controller.Transactions(null, null);
            var modelResult = (List<TransactionRow>)result.Model;

            // Assert:
            Assert.AreEqual("_Transactions", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedTransactions[i].DatePayed, modelResult[i].DatePayed);
                Assert.AreEqual(expectedTransactions[i].Date, modelResult[i].Date);
                Assert.AreEqual(expectedTransactions[i].Amount, modelResult[i].Amount);
                Assert.AreEqual(expectedTransactions[i].Message, modelResult[i].Message);
                Assert.AreEqual(expectedTransactions[i].FromAccountNo, modelResult[i].FromAccountNo);
                Assert.AreEqual(expectedTransactions[i].ToAccountNo, modelResult[i].ToAccountNo);
            }
        }

        [TestMethod]
        public void Transactions_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.Transactions(null, null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void Transactions_CustomersSelected()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string[] birthNos = new string[] { "01018912345", "010189112233", "01018911111" };
            //string[] accountNos = new string[] { "10000000000", "12341212345", "98769898765" };

            var expectedTransactions = new List<TransactionRow>();
            var date = new DateTime(2010, 1, 18);
            int j = 0;
            foreach (var birthNo in birthNos)
            {
                var temp = new TransactionRow()
                {
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * j),
                    Message = "Test"
                };
                expectedTransactions.Add(temp);
                j++;
            }

            // Act:
            var result = (PartialViewResult)controller.Transactions(birthNos, null);
            var modelResult = (List<TransactionRow>)result.Model;

            // Assert:
            Assert.AreEqual("_Transactions", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedTransactions[i].DatePayed, modelResult[i].DatePayed);
                Assert.AreEqual(expectedTransactions[i].Date, modelResult[i].Date);
                Assert.AreEqual(expectedTransactions[i].Amount, modelResult[i].Amount);
                Assert.AreEqual(expectedTransactions[i].Message, modelResult[i].Message);
                Assert.AreEqual(expectedTransactions[i].FromAccountNo, modelResult[i].FromAccountNo);
                Assert.AreEqual(expectedTransactions[i].ToAccountNo, modelResult[i].ToAccountNo);
            }
        }

        [TestMethod]
        public void Transactions_AccountsSelected()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            //string[] birthNos = new string[] { "01018912345", "010189112233", "01018911111" };
            string[] accountNos = new string[] { "10000000000", "12341212345", "98769898765" };

            var expectedTransactions = new List<TransactionRow>();
            var date = new DateTime(2010, 1, 18);
            int j = 0;
            foreach (var accountNo in accountNos)
            {
                j++;
                var from = new TransactionRow()
                {
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * j),
                    Message = "Test",
                    FromAccountNo = accountNo,
                    ToAccountNo = "10000000000"
                };
                j++;
                var to = new TransactionRow()
                {
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * j),
                    Message = "Test",
                    FromAccountNo = "10000000000",
                    ToAccountNo = accountNo
                };
                
                expectedTransactions.Add(from);
                expectedTransactions.Add(to);
            }

            // Act:
            var result = (PartialViewResult)controller.Transactions(null, accountNos);
            var modelResult = (List<TransactionRow>)result.Model;

            // Assert:
            Assert.AreEqual("_Transactions", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedTransactions[i].DatePayed, modelResult[i].DatePayed);
                Assert.AreEqual(expectedTransactions[i].Date, modelResult[i].Date);
                Assert.AreEqual(expectedTransactions[i].Amount, modelResult[i].Amount);
                Assert.AreEqual(expectedTransactions[i].Message, modelResult[i].Message);
                Assert.AreEqual(expectedTransactions[i].FromAccountNo, modelResult[i].FromAccountNo);
                Assert.AreEqual(expectedTransactions[i].ToAccountNo, modelResult[i].ToAccountNo);
            }
        }

        [TestMethod]
        public void Transactions_AccountsAndCustomersSelected()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string[] birthNos = new string[] { "01018912345", "010189112233", "01018911111" };
            string[] accountNos = new string[] { "10000000000", "12341212345", "98769898765" };

            var expectedTransactions = new List<TransactionRow>();
            var date = new DateTime(2010, 1, 18);
            int j = 0;
            foreach (var accountNo in accountNos)
            {
                j++;
                var from = new TransactionRow()
                {
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * j),
                    Message = "Test",
                    FromAccountNo = accountNo,
                    ToAccountNo = "10000000000"
                };
                j++;
                var to = new TransactionRow()
                {
                    DatePayed = date,
                    Date = date,
                    Amount = 100 + (5 * j),
                    Message = "Test",
                    FromAccountNo = "10000000000",
                    ToAccountNo = accountNo
                };
                expectedTransactions.Add(from);
                expectedTransactions.Add(to);
            }

            // Act:
            var result = (PartialViewResult)controller.Transactions(birthNos, accountNos);
            var modelResult = (List<TransactionRow>)result.Model;

            // Assert:
            Assert.AreEqual("_Transactions", result.ViewName);

            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedTransactions[i].DatePayed, modelResult[i].DatePayed);
                Assert.AreEqual(expectedTransactions[i].Date, modelResult[i].Date);
                Assert.AreEqual(expectedTransactions[i].Amount, modelResult[i].Amount);
                Assert.AreEqual(expectedTransactions[i].Message, modelResult[i].Message);
                Assert.AreEqual(expectedTransactions[i].FromAccountNo, modelResult[i].FromAccountNo);
                Assert.AreEqual(expectedTransactions[i].ToAccountNo, modelResult[i].ToAccountNo);
            }
        }

        [TestMethod]
        public void Betal_OnePayment()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            int paymentId = 1;

            var expectedTransactions = new List<Transaction>();

            // Act:
            var result = controller.Betal(paymentId);

            // Assert:
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Betal_MultiplePayments()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            int paymentId = -1;

            var expectedTransactions = new List<Transaction>();

            // Act:
            var result = controller.Betal(paymentId);

            // Assert:
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Betal_NotExisting()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            int paymentId = 1000;

            var expectedTransactions = new List<Transaction>();

            // Act:
            var result = controller.Betal(paymentId);

            // Assert:
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Betal_NotEnoughBalance()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            int paymentId = 100;

            var expectedTransactions = new List<Transaction>();

            // Act:
            var result = controller.Betal(paymentId);

            // Assert:
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void DeletePayment_Exists()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            int paymentId = 1;

            var expectedTransactions = new List<Transaction>();

            // Act:
            var result = controller.DeletePayment(paymentId);

            // Assert:
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void DeletePayment_NotExisting()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            int paymentId = 1000;

            var expectedTransactions = new List<Transaction>();

            // Act:
            var result = controller.DeletePayment(paymentId);

            // Assert:
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void EditCustomerPartial()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string birthNo = "01018912345";

            var expectedCustomer = new CustomerVM()
            {
                BirthNo = birthNo,
                FirstName = "Ola",
                LastName = "Nordmann",
                Address = "Testveien 3a",
                PhoneNo = "11223344",
                PostalArea = "Oslo",
                PostCode = "0182"
            };

            // Act:
            var result = (PartialViewResult)controller.EditCustomerPartial(birthNo);
            var modelResult = (CustomerVM) result.Model;

            // Assert:
            Assert.AreEqual("_EditCustomersPartial", result.ViewName);

            Assert.AreEqual(expectedCustomer.BirthNo, modelResult.BirthNo);
            Assert.AreEqual(expectedCustomer.FirstName, modelResult.FirstName);
            Assert.AreEqual(expectedCustomer.LastName, modelResult.LastName);
            Assert.AreEqual(expectedCustomer.Address, modelResult.Address);
            Assert.AreEqual(expectedCustomer.PhoneNo, modelResult.PhoneNo);
        }

        [TestMethod]
        public void EditCustomerPartial_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.EditCustomerPartial(null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void AddCustomer_Success()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string birthNo = "01018933333";

            var customer = new AddCustomer()
            {
                BirthNo = birthNo,
                FirstName = "Ola",
                LastName = "Nordmann",
                Address = "Testveien 3a",
                PhoneNo = "11223344",
                PostCode = "0182"
            };

            // Act:
            var result = (JsonResult)controller.AddCustomer(customer);

            // Assert:
            Assert.AreEqual(new { success = true }.ToString(), result.Data.ToString());
        }

        [TestMethod]
        public void AddCustomer_Exists()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string birthNo = "01018912345";

            var expectedCustomer = new AddCustomer()
            {
                BirthNo = birthNo,
                FirstName = "Ola",
                LastName = "Nordmann",
                Address = "Testveien 3a",
                PhoneNo = "11223344",
                PostCode = "0182"
            };

            // Act:
            var result = (PartialViewResult) controller.AddCustomer(expectedCustomer);
            var modelResult = (AddCustomer) result.Model;

            // Assert:
            Assert.AreEqual("_AddCustomerPartial", result.ViewName);

            Assert.AreEqual(expectedCustomer.BirthNo, modelResult.BirthNo);
            Assert.AreEqual(expectedCustomer.FirstName, modelResult.FirstName);
            Assert.AreEqual(expectedCustomer.LastName, modelResult.LastName);
            Assert.AreEqual(expectedCustomer.Address, modelResult.Address);
            Assert.AreEqual(expectedCustomer.PhoneNo, modelResult.PhoneNo);
            Assert.AreEqual(expectedCustomer.PostCode, modelResult.PostCode);
        }

        [TestMethod]
        public void CreatePayment_Success()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var date = new DateTime(2010, 1, 18);
            var paymentVM = new PaymentVM()
            {
                Amount = 100,
                FromAccountNo = "10000000000",
                ToAccountNo = "10000000009",
                Message = "Test"
            };

            // Act:
            var result = (JsonResult) controller.CreatePayment(paymentVM);

            // Assert:
            Assert.AreEqual(new { success = true }.ToString(), result.Data.ToString());
        }

        [TestMethod]
        public void CreatePayment_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.CreatePayment(null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void CreatePayment_InvalidModelState()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new PaymentVM();
            controller.ViewData.ModelState.AddModelError("Amount", "Beløpet må fylles ut");

            // Act:
            var result = (PartialViewResult)controller.CreatePayment(model);

            // Assert:
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
            Assert.AreEqual("_CreatePaymentPartial", result.ViewName);
        }

        [TestMethod]
        public void CreatePayment_FromAccountNoNotExisting()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var date = new DateTime(2010, 1, 18);
            var expectedPayment = new PaymentVM()
            {
                Amount = 100,
                FromAccountNo = "90000000000",
                ToAccountNo = "10000000009",
                Message = "Test"
            };

            // Act:
            var result = (PartialViewResult) controller.CreatePayment(expectedPayment);
            var modelResult = (PaymentVM) result.Model;

            // Assert:
            Assert.AreEqual("_CreatePaymentPartial", result.ViewName);

            Assert.AreEqual(expectedPayment.Amount, modelResult.Amount);
            Assert.AreEqual(expectedPayment.FromAccountNo, modelResult.FromAccountNo);
            Assert.AreEqual(expectedPayment.ToAccountNo, modelResult.ToAccountNo);
            Assert.AreEqual(expectedPayment.Message, modelResult.Message);
        }

        [TestMethod]
        public void GetPartial()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string path = "_AddCustomerPartial";

            // Act:
            var result = (PartialViewResult) controller.GetPartial(path);

            // Assert:
            Assert.AreEqual(path, result.ViewName);
        }

        [TestMethod]
        public void GetPartial_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.GetPartial(null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void GetCreateAccountPartial()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            string accountNo = "12345678900";

            var expectedCustomers = new List<Customer>();
            var salt = "salt";
            var passwordAndSalt = "Test123salt";
            var password = createHash(passwordAndSalt);
            var customer = new Customer()
            {
                Active = true,
                Address = "Storgata 83",
                BirthNo = "01018912345",
                FirstName = "André",
                LastName = "Hovda",
                Password = password,
                Salt = salt,
                PhoneNo = "94486775",
                PostCode = "0182"
            };
            expectedCustomers.Add(customer);
            expectedCustomers.Add(customer);
            expectedCustomers.Add(customer);

            // Act:
            var result = (PartialViewResult) controller.GetCreateAccountPartial(accountNo);

            // Assert:
            Assert.AreEqual("_CreateAccountPartial", result.ViewName);
            var modelResult = (AccountVM) result.Model;
            var customersResult = modelResult.Customers;

            for (var i = 0; i < customersResult.Count; i++)
            {
                Assert.AreEqual(expectedCustomers[i].Active, customersResult[i].Active);
                Assert.AreEqual(expectedCustomers[i].Address, customersResult[i].Address);
                Assert.AreEqual(expectedCustomers[i].BirthNo, customersResult[i].BirthNo);
                Assert.AreEqual(expectedCustomers[i].FirstName, customersResult[i].FirstName);
                Assert.AreEqual(expectedCustomers[i].LastName, customersResult[i].LastName);
                Assert.IsTrue(expectedCustomers[i].Password.SequenceEqual(customersResult[i].Password));
                Assert.AreEqual(expectedCustomers[i].Salt, customersResult[i].Salt);
                Assert.AreEqual(expectedCustomers[i].PhoneNo, customersResult[i].PhoneNo);
                Assert.AreEqual(expectedCustomers[i].PostCode, customersResult[i].PostCode);
            }
        }

        [TestMethod]
        public void GetCreateAccountPartial_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.GetCreateAccountPartial(null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void UpdateCustomer()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new CustomerVM()
            {
                Address = "Storgata 83",
                BirthNo = "01018912345",
                FirstName = "André",
                LastName = "Hovda",
                PhoneNo = "94486775"
            };


            // Act:
            var result = (JsonResult) controller.UpdateCustomer(model);

            // Assert:
            Assert.AreEqual(new { success = true }.ToString(), result.Data.ToString());
        }

        [TestMethod]
        public void UpdateCustomer_NotExisting()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new CustomerVM()
            {
                Address = "Storgata 83",
                BirthNo = "01018999999",
                FirstName = "André",
                LastName = "Hovda",
                PhoneNo = "94486775"
            };


            // Act:
            var result = (JsonResult)controller.UpdateCustomer(model);

            // Assert:
            Assert.AreEqual(new { success = true }.ToString(), result.Data.ToString());
        }

        [TestMethod]
        public void UpdateCustomer_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.UpdateCustomer(null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void UpdateCustomer_InvalidModelState()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new CustomerVM();
            controller.ViewData.ModelState.AddModelError("BirthNo", "Eier kan ikke være blankt");


            // Act:
            var result = (PartialViewResult) controller.UpdateCustomer(model);

            // Assert:
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
            Assert.AreEqual("_EditCustomersPartial", result.ViewName);
        }

        [TestMethod]
        public void UpdateAccount()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new AccountVM()
            {
                OwnerBirthNo = "01018912345",
                Type = "BSU"
            };
            
            // Act:
            var result = (JsonResult)controller.UpdateAccount(model);

            // Assert:
            Assert.AreEqual(new { success = true }.ToString(), result.Data.ToString());
        }

        [TestMethod]
        public void UpdateAccount_NotExisting()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new AccountVM()
            {
                OwnerBirthNo = "01018900000",
                Type = "BSU"
            };

            // Act:
            var result = (JsonResult)controller.UpdateAccount(model);

            // Assert:
            Assert.AreEqual(new { success = true }.ToString(), result.Data.ToString());
        }

        [TestMethod]
        public void UpdateAccount_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.UpdateAccount(null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void UpdateAccount_InvalidModelState()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new AccountVM();
            controller.ViewData.ModelState.AddModelError("BirthNo", "Eier kan ikke være blankt");


            // Act:
            var result = (PartialViewResult)controller.UpdateAccount(model);

            // Assert:
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
            Assert.AreEqual("_EditAccountsPartial", result.ViewName);
        }

        [TestMethod]
        public void AddAccount()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new AccountVM()
            {
                OwnerBirthNo = "01018912345",
                Type = "BSU"
            };

            // Act:
            var result = (JsonResult)controller.AddAccount(model);

            // Assert:
            Assert.AreEqual(new { success = true }.ToString(), result.Data.ToString());
        }

        
        [TestMethod]
        public void AddAccount_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.AddAccount(null);

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }

        [TestMethod]
        public void AddAccount_InvalidModelState()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var model = new AccountVM()
            {
                OwnerBirthNo = "",
                Type = "BSU"
            };
            controller.ViewData.ModelState.AddModelError("BirthNo", "Eier kan ikke være blankt");


            // Act:
            var result = (PartialViewResult)controller.AddAccount(model);
            var modelResult = (AccountVM)result.Model;

            // Assert:
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
            Assert.AreEqual("_CreateAccountPartial", result.ViewName);
            Assert.AreEqual(model.OwnerBirthNo, modelResult.OwnerBirthNo);
            Assert.AreEqual(model.Type, modelResult.Type);
        }

        [TestMethod]
        public void DeactivateAccount_Success()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expected = "Suksess";
            var accountNo = "10000000000";

            // Act:
            var result = controller.DeactivateAccount(accountNo);

            // Assert:
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeactivateAccount_NotExisting()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expected = "Konto eksisterer ikke";
            var accountNo = "90000000000";

            // Act:
            var result = controller.DeactivateAccount(accountNo);

            // Assert:
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeactivateAccount_Null()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expected = "Klarte ikke å deaktivere konto";

            // Act:
            var result = controller.DeactivateAccount(null);

            // Assert:
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeactivateCustomer_Success()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expected = "Suksess";
            var birthNo = "01018912345";

            // Act:
            var result = controller.DeactivateCustomer(birthNo);

            // Assert:
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeactivateCustomer_NotExisting()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expected = "Kunde eksisterer ikke";
            var birthNo = "12121200000";

            // Act:
            var result = controller.DeactivateCustomer(birthNo);

            // Assert:
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeactivateCustomer_Null()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expected = "Klarte ikke å deaktivere kunde";

            // Act:
            var result = controller.DeactivateCustomer(null);

            // Assert:
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Audit()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expectedProperties = new List<AuditEntryPropertyVM>();
            var expectedEntries = new List<AuditEntryVM>();

            var date = new DateTime(2010, 1, 18);

            AuditEntryPropertyVM propertyVM = new AuditEntryPropertyVM()
            {
                Date = date,
                EntityName = "Customer",
                State = "EntityAdded",
                PropertyName = "FirstName",
                OldValue = "",
                NewValue = "Hans"
            };
            AuditEntryPropertyVM propertyVM2 = new AuditEntryPropertyVM()
            {
                Date = date,
                EntityName = "Customer",
                State = "EntityAdded",
                PropertyName = "LastName",
                OldValue = "",
                NewValue = "Hansen"
            };
            expectedProperties.Add(propertyVM);
            expectedProperties.Add(propertyVM2);

            for (int i = 0; i < 5; i++)
            {
                AuditEntryVM entryVM = new AuditEntryVM()
                {
                    AuditEntryID = i,
                    Date = date,
                    EntityName = "Customer",
                    State = "EntityAdded",
                    EntryProperties = expectedProperties
                };
                expectedEntries.Add(entryVM);
            }
            expectedEntries.Reverse();

            // Act:
            var result = (PartialViewResult)controller.Audit();

            // Assert:
            Assert.AreEqual("_Audit", result.ViewName);
            var modelResult = (List<AuditEntryVM>) result.Model;
            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedEntries[i].AuditEntryID, modelResult[i].AuditEntryID);
                Assert.AreEqual(expectedEntries[i].Date, modelResult[i].Date);
                Assert.AreEqual(expectedEntries[i].EntityName, modelResult[i].EntityName);
                Assert.AreEqual(expectedEntries[i].State, modelResult[i].State);

                var propertyModelResult = modelResult[i].EntryProperties;
                for (int j = 0; j < expectedProperties.Count; j++)
                {
                    Assert.AreEqual(expectedProperties[j].Date, propertyModelResult[j].Date);
                    Assert.AreEqual(expectedProperties[j].EntityName, propertyModelResult[j].EntityName);
                    Assert.AreEqual(expectedProperties[j].State, propertyModelResult[j].State);
                    Assert.AreEqual(expectedProperties[j].PropertyName, propertyModelResult[j].PropertyName);
                    Assert.AreEqual(expectedProperties[j].OldValue, propertyModelResult[j].OldValue);
                    Assert.AreEqual(expectedProperties[j].NewValue, propertyModelResult[j].NewValue);
                }
            }
        }

        [TestMethod]
        public void Audit_CheckSession()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Set Session to null:
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var actual = (RedirectToRouteResult)controller.Audit();

            // Assert:
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("Index", actual.RouteValues["Action"]);
            Assert.AreEqual("Index", actual.RouteValues["Controller"]);
        }


        /* NOT TEST METHODS */
        private static byte[] createHash(string innStreng)
        {
            try
            {
                byte[] innData, utData;
                var algoritme = SHA256.Create();
                innData = Encoding.UTF8.GetBytes(innStreng);
                utData = algoritme.ComputeHash(innData);
                return utData;
            }
            catch (NullReferenceException e)
            {
                string log = "Failed to create hash.\t" + e.Message + "\t" + e.StackTrace.ToString();
                Debug.Write(log);
                new LogErrors().errorLog(log);
                return new byte[0];
            }
        }
    }
}
