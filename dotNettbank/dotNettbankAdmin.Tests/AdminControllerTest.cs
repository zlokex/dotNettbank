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
        public void LoginEmpty()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            // Act:
            var result = controller.Login("", "");

            // Assert:
            Assert.AreEqual(result, false);
        }
    }
}
