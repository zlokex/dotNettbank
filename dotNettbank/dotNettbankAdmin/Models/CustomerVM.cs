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
    public class CustomerVM
    {
        [CustomValidation(typeof(CustomerVM), "IsBirthNoExisting")]
        [Required(ErrorMessage = "Eier kan ikke være blankt")]
        [Display(Name ="Eier")]
        public string BirthNo { get; set; }

        [Required(ErrorMessage = "Fornavn kan ikke være blankt")]
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Etternavn kan ikke være blankt")]
        [Display(Name = "Etternavn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adresse kan ikke være blankt")]
        [Display(Name = "Adresse")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefonnummer kan ikke være blankt")]
        [Display(Name = "Telefonnummer")]
        public string PhoneNo { get; set; }

        public static ValidationResult IsBirthNoExisting(string OwnerBirthNo)
        {
            bool isValid;

            AdminService db = new AdminService();

            var customer = db.getCustomerByBirthNo(OwnerBirthNo);
            if (customer == null)
            {
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Ingen registrerte kunder med '" + OwnerBirthNo + "' som fødselsnummer");
            }

        }
    }
}