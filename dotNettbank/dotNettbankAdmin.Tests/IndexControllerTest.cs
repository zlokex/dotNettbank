using BLL.AdminService;
using DAL.AdminRepo;
using dotNettbankAdmin.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dotNettbankAdmin.Tests
{
    [TestClass]
    public class IndexControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange:
            var controller = new IndexController(new AdminService(new AdminRepositoryStub()));
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = "admin";

            // Act:
            var result = (RedirectToRouteResult) controller.Index();

            // Assert:
            Assert.IsNotNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
            Assert.AreEqual("AdminSide", result.RouteValues["Action"]);
            Assert.AreEqual("Admin", result.RouteValues["Controller"]);
        }

        [TestMethod]
        public void Index_CheckSession()
        {
            // Arrange:
            var controller = new IndexController(new AdminService(new AdminRepositoryStub()));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            controller.ControllerContext.HttpContext.Session["LoggedIn"] = null;

            // Act:
            var result = (ViewResult) controller.Index();

            // Assert:
            Assert.AreEqual("", result.ViewName);
            Assert.IsNull(controller.ControllerContext.HttpContext.Session["LoggedIn"]);
        }
    }
}
