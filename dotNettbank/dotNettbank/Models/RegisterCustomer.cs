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
        public string BirthNo { get; set; } // Fødselsnummer
        [Required(ErrorMessage = "Fornavn må oppgis")]
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Etternavn må oppgis")]
        [Display(Name = "Etternavn")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Adresse må oppgis")]
        [Display(Name = "Adresse")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Telefonnummer må oppgis")]
        [Display(Name = "Telefon")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Postnr må oppgis")]
        [Display(Name = "Postnr")]
        [DataType(DataType.PostalCode)]
        public string PostCode { get; set; } // Postnr
        [Required(ErrorMessage = "Poststed må oppgis")]
        [Display(Name = "Poststed")]
        public string PostalArea { get; set; } // Poststed
        [Required(ErrorMessage = "Passord må oppgis")]
        [Display(Name = "Passord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}