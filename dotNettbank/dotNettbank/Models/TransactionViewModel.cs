using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{

    public class TransactionViewModel
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public double InAmount { get; set; }
        public double OutAmount { get; set; }

        public string FromName { get; set; } // Avsender navn
        public string ToName { get; set; } // Mottaker navn (Hentes gjennom Account.Owner.FirstName + LastName
        public string FromAccountNo { get; set; }
        public string ToAccountNo { get; set; }

    }
}