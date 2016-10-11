using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{

    // Transaction: Created when a payment is completed.
    public class Transaction
    {
        public int TransactionID { get; set; }
        public DateTime Date { get; set; } // Dato betalt
        public double Amount { get; set; } // Beløp
        public Account FromAccount { get; set; } //FraKonto
        public Account ToAccount { get; set; } //TilKonto

    }
}