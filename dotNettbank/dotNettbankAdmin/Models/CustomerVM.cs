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
        [RegularExpression(@"^([äÄöÖüÜéÉëËÆØÅæøåA-Za-z ]{2,25})$", ErrorMessage = "Fornavn må være mellom 2 og 25 bokstaver lang")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Etternavn kan ikke være blankt")]
        [Display(Name = "Etternavn")]
        [RegularExpression(@"^([äÄöÖüÜéÉëËÆØÅæøåA-Za-z]{2,25})$", ErrorMessage = "Etternavn må være mellom 2 og 25 bokstaver lang")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adresse kan ikke være blankt")]
        [Display(Name = "Adresse")]
        [RegularExpression(@"^([äÄöÖüÜëËÆØÅæøåA-Za-z0-9 _]{5,30})$", ErrorMessage = "Feil i adresse! Har du skrevet riktig?")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefonnummer kan ikke være blankt")]
        [Display(Name = "Telefonnummer")]
        [RegularExpression(@"^([(\d]{8})$", ErrorMessage = "Feil telefon nummer, kun 8 tall er tilatt")]

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