using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.AdminService;
using dotNettbank.Model;

namespace dotNettbankAdmin.Models
{
    public class AccountVM
    {
        //[System.Web.Mvc.Remote("IsBirthNoExisting", "Validation", ErrorMessage = "Kunde med dette personnummer eksisterer ikke i nettbanken")]
        [CustomValidation(typeof(AccountVM), "IsBirthNoExisting")]
        [Required(ErrorMessage = "Eier kan ikke være blankt")]
        [Display(Name = "Eier")]
        
        public string OwnerBirthNo { get; set; }

        public List<Customer> Customers { get; set; }

        [Required(ErrorMessage = "Kontonummer kan ikke være blankt")]
        [Display(Name = "Kontonummer")]
        public string AccountNo { get; set; } //kontonr

        [Required(ErrorMessage = "Balanse kan ikke være blankt")]
        [Display(Name = "Balanse")]
        public double Balance { get; set; } //Saldo

        [Required(ErrorMessage = "Kontonavn kan ikke være blankt")]
        [Display(Name = "Kontonavn")]
        public string Type { get; set; } // Kontotype


        public static ValidationResult IsBirthNoExisting(string OwnerBirthNo)
        {
            bool isValid;

            AdminService db = new AdminService();

            var customer = db.getCustomerByBirthNo(OwnerBirthNo);
            if (customer == null)
            {
                isValid = false;
            } else
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