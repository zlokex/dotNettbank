using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dotNettbank.Models.DomainModels
{
    public class Customer
    {
        [Key]
        public string BirthNo { get; set; } // Fødselsnummer
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string PostCode { get; set; } // Postnr
        public virtual PostalArea PostalArea { get; set; } // Poststed
        public byte[] Password { get; set; }
        public string Salt { get; set; }

        public virtual IEnumerable<Account> Accounts { get; set; } // List of accounts belonging to Customer
    }
}