using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{

    public class TransactionViewModel
    {
        [Display(Name = "Dato")]
        [DisplayFormat(DataFormatString = "{dd-mm-yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "Beskrivelse")]
        public string Message { get; set; }

        [Display(Name = "Inn")]
        public double InAmount { get; set; }

        [Display(Name = "Ut")]
        public double OutAmount { get; set; }


        [Display(Name = "Avsender")]
        public string FromName { get; set; } // Avsender navn

        [Display(Name = "Mottaker")]
        public string ToName { get; set; } // Mottaker navn (Hentes gjennom Account.Owner.FirstName + LastName

        [Display(Name = "Avsender konto")]
        public string FromAccountNo { get; set; }

        [Display(Name = "Mottaker konto")]
        public string ToAccountNo { get; set; }
    }
}