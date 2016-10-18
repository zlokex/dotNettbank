using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.Model
{

    // Transaction: Created when a payment is completed.
    public class Transaction
    {
        public int TransactionID { get; set; }
        public DateTime Date { get; set; } // Dato betalt
        public double Amount { get; set; } // Beløp
        public string Message { get; set; } // KID eller melding
        public string FromName { get; set; } // Avsender navn
        public string ToName { get; set; } // Mottaker navn
        public virtual Account FromAccount { get; set; } //FraKonto
        public virtual Account ToAccount { get; set; } //TilKonto

    }
}