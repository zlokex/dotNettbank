using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dotNettbank.Model
{

    

    public class Account
    {
        [Key]
        public string AccountNo { get; set; }
        public double Balance { get; set; }
        public AccountType Type { get; set; } // Kontotype
        public double InterestRate { get; set; } // Rentesats
        public virtual Customer Owner { get; set; } // Eier av konto

    }

    public class CreditAccount : Account
    {
        private CreditAccount() { Type = AccountType.Credit; }

        public double CreditBalance { get; set; }
    }

    public class AccountType
    {
        private AccountType(string value) { Value = value; }

        public string Value { get; set; }

        public static AccountType Usage { get { return new AccountType("Brukskonto"); } }
        public static AccountType Saving { get { return new AccountType("Sparekonto"); } }
        public static AccountType BSU { get { return new AccountType("BSU-konto"); } }
        public static AccountType Credit { get { return new AccountType("Kredittkort"); } }

        // Override ToString to display value:
        public override string ToString()
        {
            return Value;
        }

        // Eksempel: Type = AccountType.Saving;
    }

    /*
    public enum AccountType
    {
        [Description("Brukskonto")]
        USAGE = 1,
        [Description("Brukskonto")]
        SAVING = 2,
        BSU = 3,
        CREDIT = 4
    }
    */
}