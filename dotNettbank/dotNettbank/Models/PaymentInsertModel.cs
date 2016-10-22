using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbank.Models
{
    public class PaymentInsertModel
    {
        //public List<AccountViewModel> Accounts { get; set; }
        [Required(ErrorMessage = "Kontonr må oppgis")]
        [Display(Name = "Fra konto")]
        public string FromAccountNo { get; set; }

        //public int SelectedFromAccount { get; set; }
        //public AccountViewModel FromAccount { get; set; }

        [Required(ErrorMessage = "Kontonr må oppgis")]
        [Display(Name = "Kontonr")]
        public string ToAccountNo { get; set; }

        [Required(ErrorMessage = "Navn må oppgis")]
        [Display(Name = "Navn")]
        public string ToName { get; set; }

        [Required(ErrorMessage = "Vennligst skriv in KID eller melding")]
        [Display(Name = "KID eller melding")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Vennligst velg forfallsdato")]
        [Display(Name = "Forfallsdato")]
        [DisplayFormat(DataFormatString ="{dd-mm-yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Vennligst skriv inn et beløp (minimum kr 1)")]
        [Display(Name = "Kroner")]
        public int AmountKr { get; set; }

        // Ikke required
        [Display(Name = "Øre")]
        public int AmountOre { get; set; }
    }
}

