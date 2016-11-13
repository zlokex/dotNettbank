using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbankAdmin.Models
{
    public class PaymentRow
    {
        public int PaymentID { get; set; }
        public string FromBirthNo { get; set; }
        public DateTime DateAdded { get; set; } // Dato lagt til
        public DateTime DueDate { get; set; } // Forfallsdato
        public double Amount { get; set; } // Beløp
        public string Message { get; set; } // KID eller melding

        public string FromAccountNo { get; set; }
        public string ToAccountNo { get; set; }

    }
}