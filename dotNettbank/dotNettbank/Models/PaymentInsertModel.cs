using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class PaymentInsertModel
    {
        public string konto { get; set; }
        [Required(ErrorMessage = "Kontonr må oppgis")]
        [Display(Name = "Kontonr:")]
        public string tilKontonr { get; set; }

        public int saldo { get; set; }

        [Required(ErrorMessage = "Vennligst skriv in KID eller melding")]
        [Display(Name = "")]
        public string meldingEllerKID { get; set; }

        [Required(ErrorMessage = "Vennligst velg forfallsdato")]
        [Display(Name = "")]
        [DataType(DataType.Date)]
        public DateTime forfallsdato { get; set; }

        [Required(ErrorMessage = "Vennligst skriv riktig beløp")]
        [Display(Name = "")]
        public string beløp { get; set; }
    }
}