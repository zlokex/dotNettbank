﻿using BLL.AdminService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbankAdmin.Controllers
{
    public class IndexController : Controller
    {
        private IAdminService _adminService;

        public IndexController()
        {
            _adminService = new AdminService();
        }

        public IndexController(IAdminService stub)
        {
            _adminService = stub;
        }

        // GET: Index
        public ActionResult Index()
        {
            if (Session["LoggedIn"] != null)
            {
                return RedirectToAction("AdminSide", "Admin");
            }
            else
            {
                return View();
            }
        }
    }
}