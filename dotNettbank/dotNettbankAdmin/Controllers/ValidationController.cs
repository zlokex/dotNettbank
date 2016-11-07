using BLL.AdminService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace dotNettbankAdmin.Controllers
{
    public class ValidationController : Controller
    {
        private IAdminService _adminService;

        public ValidationController()
        {
            _adminService = new AdminService();
        }

        public ValidationController(IAdminService stub)
        {
            _adminService = stub;
        }

        // Returnerer feilmelding hvis det eksisterer kunde med fødselsnummer:
        public JsonResult IsBirthNoAvailable(string OwnerBirthNo)
        {

            if (_adminService.getCustomerByBirthNo(OwnerBirthNo) == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            //string errorMessage = String.Format(CultureInfo.InvariantCulture,
            //    "{0} er ikke tilgjengelig", birthNo);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // Returnerer feilmelding hvis det _ikke_ eksisterer kunde med fødselsnummer:
        public JsonResult IsBirthNoExisting(string OwnerBirthNo)
        {

            if (_adminService.getCustomerByBirthNo(OwnerBirthNo) != null)
                return Json(false, JsonRequestBehavior.AllowGet);

            //string errorMessage = String.Format(CultureInfo.InvariantCulture,
            //    "{0} eksisterer ikke i denne nettbanken", birthNo);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}