using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class PaymentVM
    {
        [Display(Name = "Betalings ID")]
        public int PaymentID { get; set; }

        [Display(Name = "Dato lagt til")]
        //[DisplayFormat(DataFormatString = "{dd-mm-yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Forfallsdato")]
        //[DisplayFormat(DataFormatString = "{dd-mm-yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Beskrivelse")]
        public string Message { get; set; }

        [Display(Name = "Beløp")]
        public double Amount { get; set; }


        [Display(Name = "Avsender")]
        public string FromName { get; set; } // Avsender navn

        [Display(Name = "Mottaker")]
        public string ToName { get; set; } // Mottaker navn

        [Display(Name = "Avsender konto")]
        public string FromAccountNo { get; set; }

        [Display(Name = "Mottaker konto")]
        public string ToAccountNo { get; set; }
    }
}
