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
            Assert.AreEqual(actual.ViewName, "");
        }

        [TestMethod]
        public void Login()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            // Act:
            var result = controller.Login("admin", "admin");

            // Assert:
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void LoginEmpty()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            // Act:
            var result = controller.Login("", "");

            // Assert:
            Assert.AreEqual(result, false);
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
            Assert.AreEqual(result.RouteValues["Action"], "Index");
            Assert.AreEqual(result.RouteValues["Controller"], "Index");
        }

        [TestMethod]
        public void RegCustomer()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            var expectedCustomers = new List<Customer>();

            var salt = "salt";
            var passwordAndSalt = "Test123salt";
            var password = createHash(passwordAndSalt);

            var customer = new Customer()
            {
                Active = true,
                Address = "Storgata 83",
                BirthNo = "0101891245",
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



            string[] birthNo = new string[] { "01018912345", "010189112233", "01018911111" };

            // Act:
            var result = (PartialViewResult)controller.RegCustomer(birthNo);
            var modelResult = (List<Customer>)result.Model;

            //result.RouteValues["action"].Equals("Index");
            //result.RouteValues["controller"].Equals("Index");

            // Assert:
            Assert.AreEqual(result.ViewName, "_AddCustomerPartial");
            
            for (var i = 0; i < modelResult.Count; i++)
            {
                Assert.AreEqual(expectedCustomers[i].Active, modelResult[i].Active);
                Assert.AreEqual(expectedCustomers[i].Address, modelResult[i].Address);
                Assert.AreEqual(expectedCustomers[i].BirthNo, modelResult[i].BirthNo);
                Assert.AreEqual(expectedCustomers[i].FirstName, modelResult[i].FirstName);
                Assert.AreEqual(expectedCustomers[i].LastName, modelResult[i].LastName);
                Assert.AreEqual(expectedCustomers[i].Password, modelResult[i].Password);
                Assert.AreEqual(expectedCustomers[i].Salt, modelResult[i].Salt);
                Assert.AreEqual(expectedCustomers[i].PhoneNo, modelResult[i].PhoneNo);
                Assert.AreEqual(expectedCustomers[i].PostCode, modelResult[i].PostCode);
            }
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
                new LogErrors().errorLog(log);
                return new byte[0];
            }
        }

    }
}
