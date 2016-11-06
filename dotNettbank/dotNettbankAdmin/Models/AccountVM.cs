using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbankAdmin.Models
{
    public class AccountVM
    {
        public string OwnerBirthNo { get; set; }
        public string AccountNo { get; set; } //kontonr
        public double Balance { get; set; } //Saldo
        public string Type { get; set; } // Kontotype
    }
}