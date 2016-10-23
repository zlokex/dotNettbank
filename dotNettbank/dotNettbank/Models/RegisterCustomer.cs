using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class RegisterCustomer
    {
        [Required(ErrorMessage = "Fødselsnummer må oppgis")]
        [Display(Name = "Fødselsnummer")]
        [RegularExpression(@"^((0[1-9]|[12]\d|3[01])([04][1-9]|[15][0-2])\d{7})$", ErrorMessage = "Ugyldig fødselsnummer")]
        [System.Web.Mvc.Remote("CheckExistingBirthNo", "Admin", ErrorMessage = "Du er allerede registrert")]
        public string BirthNo { get; set; } // Fødselsnummer

        [Required(ErrorMessage = "Fornavn må oppgis")]
        [Display(Name = "Fornavn")]
        [RegularExpression(@"^([äÄöÖüÜéÉëËÆØÅæøåA-Za-z]{2,25})$", ErrorMessage = "Fornavn må være mellom 2 og 25 bokstaver lang")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Etternavn må oppgis")]
        [Display(Name = "Etternavn")]
        [RegularExpression(@"^([äÄöÖüÜéÉëËÆØÅæøåA-Za-z]{2,25})$", ErrorMessage = "Etternavn må være mellom 2 og 25 bokstaver lang")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adresse må oppgis")]
        [Display(Name = "Adresse")]
        [RegularExpression(@"^([äÄöÖüÜëËÆØÅæøåA-Za-z0-9 _]{5,30})$", ErrorMessage = "Feil i adresse! Har du skrevet riktig?")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefonnummer må oppgis")]
        [Display(Name = "Telefon")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([(\d]{8})$", ErrorMessage = "Feil telefon nummer, kun 8 tall er tilatt")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Postnr må oppgis")]
        [Display(Name = "Postnr")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^([(\d]{4})$", ErrorMessage = "Feil postnummer, kun 4 tall er tilatt")]
        public string PostCode { get; set; } // Postnr

        [Required(ErrorMessage = "Poststed må oppgis")]
        [Display(Name = "Poststed")]
        [RegularExpression(@"^([äÄöÖüÜéÉëËÆØÅæøåA-Za-z]{2,15})$", ErrorMessage = "Feil poststed, har du skrevet riktig?")]
        public string PostalArea { get; set; } // Poststed

        [Required(ErrorMessage = "Passord må oppgis")]
        [StringLength(255, ErrorMessage = "Må være på mellom 5 og 255 karakterer", MinimumLength = 5)]
        [Display(Name = "Passord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gjenta passord må oppgis")]
        [StringLength(255, ErrorMessage = "Må være identisk til passordet i feltet over", MinimumLength = 5)]
        [Display(Name = "Gjenta passord")]
        [DataType(DataType.Password)]
        [Compare("Password")] // Enables compare validation of PasswordRepeat and Password (they need to be identical)
        public string PasswordRepeat { get; set; }

    }
}