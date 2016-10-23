using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dotNettbank.Model
{
    public class Customer
    {
        [Key]
        public string BirthNo { get; set; } // Fødselsnummer
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public byte[] Password { get; set; }
        public string Salt { get; set; }

        //Foreign key:
        public string PostCode { get; set; } // Postnr
        [ForeignKey("PostCode")]
        public virtual PostalArea PostalArea { get; set; } // Poststed

        [InverseProperty("Owner")]
        public virtual IEnumerable<Account> Accounts { get; set; } // List of accounts belonging to Customer
    }
}