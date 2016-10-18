using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.Model
{
    public enum PaymentStatus
    {
        InProcess, // Under behandling
        NoCoverage, // Ingen deking
        Stopped, // Stoppet etter 5 dager uten dekning
        Rejected // Avvist av ulike årsaker

    }

    // 
    public class Payment
    {
        public int PaymentID { get; set; }
        public DateTime DateAdded { get; set; } // Dato lagt til
        public DateTime DueDate { get; set; } // Forfallsdato
        public double Amount { get; set; } // Beløp
        public string Message { get; set; } // KID eller melding
        public string FromName { get; set; } // Avsender navn
        public string ToName { get; set; } // Mottaker navn
        public virtual Account FromAccount { get; set; } //FraKonto
        public virtual Account ToAccount { get; set; } //TilKonto

        public PaymentStatus PaymentStatus { get; set; }
    }
}