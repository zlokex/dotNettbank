﻿using System;
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
        public string AccountNo { get; set; } //kontonr
        public double Balance { get; set; } //Saldo
        public string Type { get; set; } // Kontotype
        public string Name { get; set; } //KontoNavn sette ved opprettelse
        public double InterestRate { get; set; } // Rentesats

        private bool activeVal = true;
        [DefaultValue(true)]
        public bool Active // Konto Aktiv/Deaktivert
        {
            get
            {
                return activeVal;
            }
            set
            {
                activeVal = value;
            }
        }


        // Foreign key:
        public string OwnerBirthNo { get; set; }

        [ForeignKey("OwnerBirthNo")]
        public virtual Customer Owner { get; set; } // Eier av konto

        [InverseProperty("FromAccount")]
        public virtual IEnumerable<Transaction> SentTransactions { get; set; }
        [InverseProperty("ToAccount")]
        public virtual IEnumerable<Transaction> ReceivedTransactions { get; set; }

        [InverseProperty("FromAccount")]
        public virtual IEnumerable<Payment> SentPayments { get; set; }
        [InverseProperty("ToAccount")]
        public virtual IEnumerable<Payment> ReceivedPayments { get; set; }



    }

    /*
    public class CreditAccount : Account
    {
        //private CreditAccount() { Type = AccountType.Credit; }

        private CreditAccount() { Type = "Kreditt"; }

        public double CreditBalance { get; set; }
    }
    */

    /*
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
    */

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