using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public enum AccountType
    {
        Saving, Regular, Credit
    }

    public class Account
    {
        [Key]
        public int AccountNo { get; set; }
        [ForeignKey("Customer")]
        public string BirthNo { get; set; } // Fødselsnummer
        public double Balance { get; set; }
        public AccountType AccountType { get; set; }
    }
}