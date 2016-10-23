using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dotNettbank.Model
{

    // Transaction: Created when a payment is completed.
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public DateTime DatePayed { get; set; } // Dato betalt
        public DateTime Date { get; set; } // Dato registrert/betaling mottat
        public double Amount { get; set; } // Beløp
        public string Message { get; set; } // KID eller melding

        // Foreign keys:
        public string FromAccountNo { get; set; }
        public string ToAccountNo { get; set; }

        [ForeignKey("FromAccountNo")]
        public virtual Account FromAccount { get; set; } //FraKonto
        [ForeignKey("ToAccountNo")]
        public virtual Account ToAccount { get; set; } //TilKonto

    }
}