using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dotNettbank.Model
{
    /*
    public enum PaymentStatus
    {
        InProcess, // Under behandling
        NoCoverage, // Ingen deking
        Stopped, // Stoppet etter 5 dager uten dekning
        Rejected // Avvist av ulike årsaker

    }
    */

    // 
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public DateTime DateAdded { get; set; } // Dato lagt til
        public DateTime DueDate { get; set; } // Forfallsdato
        public double Amount { get; set; } // Beløp
        public string Message { get; set; } // KID eller melding


        // Foreign keys:
        public string FromAccountNo { get; set; }
        public string ToAccountNo { get; set; }

        [ForeignKey("FromAccountNo")]
        public virtual Account FromAccount { get; set; } //FraKonto
        [ForeignKey("ToAccountNo")]
        public virtual Account ToAccount { get; set; } //TilKonto


        //public PaymentStatus PaymentStatus { get; set; }
    }
}