using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotNettbankAdmin.Controllers;
using BLL.AdminService;
using DAL.AdminRepo;

namespace dotNettbankAdmin.Tests
{
    [TestClass]
    public class AdminControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange:
            var controller = new AdminController(new AdminService(new AdminRepositoryStub()));

            //var expectedResult;
        }
    }
}
