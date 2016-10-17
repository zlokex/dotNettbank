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
        public virtual Account FromAccount { get; set; } //FraKonto
        public virtual Account ToAccount { get; set; } //TilKonto

    }
}