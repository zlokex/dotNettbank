using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class Customer
    {
        [Key]
        public int BirthNo { get; set; } // Fødselsnummer
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        [ForeignKey("Postage")] // Poststed
        public string Postcode { get; set; } // Postnr
        public string Password { get; set; }
        public bool LoggedIn { get; set; }
    }
}