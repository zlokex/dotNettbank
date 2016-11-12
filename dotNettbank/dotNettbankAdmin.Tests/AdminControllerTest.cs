using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotNettbankAdmin.Controllers;
using BLL.AdminService;
using DAL.AdminRepo;
using System.Web.Mvc;

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


    }
}
