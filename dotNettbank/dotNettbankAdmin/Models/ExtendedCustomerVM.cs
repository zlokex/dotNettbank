using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.AdminService;

namespace dotNettbankAdmin.Models
{
    public class ExtendedCustomerVM
    {

        [Display(Name = "Eier")]
        public string BirthNo { get; set; }

        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }

        [Display(Name = "Etternavn")]
        public string LastName { get; set; }

        [Display(Name = "Adresse")]
        public string Address { get; set; }

        [Display(Name = "Telefonnummer")]
        public string PhoneNo { get; set; }

        [Display(Name = "Postnummer")]
        public string PostCode { get; set; }

        [Display(Name = "Poststed")]
        public string PostalArea { get; set; }

        
    }
}