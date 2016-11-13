using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbankAdmin.Models
{
    public class TransactionRow
    {
        public string FromBirthNo { get; set; }
        public string ToBirthNo { get; set; }
        public DateTime DatePayed { get; set; } // Dato betalt
        public DateTime Date { get; set; } // Dato registrert/betaling mottat
        public double Amount { get; set; } // Beløp
        public string Message { get; set; } // KID eller melding

        public string FromAccountNo { get; set; }
        public string ToAccountNo { get; set; }
    }
}