﻿using System;
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
        [Display(Name = "Fra kontonummer")]
        [RegularExpression(@"^([(\d]{11})$", ErrorMessage = "Feil konto nummer, kun 11 tall er tilatt")]
        public string FromAccountNo { get; set; }

        //public int SelectedFromAccount { get; set; }
        //public AccountViewModel FromAccount { get; set; }

        [Required(ErrorMessage = "Kontonr må oppgis")]
        [Display(Name = "Til kontonummer")]
        [RegularExpression(@"^([(\d]{11})$", ErrorMessage = "Feil konto nummer, kun 11 tall er tilatt")]
        public string ToAccountNo { get; set; }

        /*[Required(ErrorMessage = "Navn må oppgis")]
        [Display(Name = "Navn")]
        public string ToName { get; set; }*/

        [Required(ErrorMessage = "Vennligst skriv in KID eller melding")]
        [Display(Name = "KID eller melding")]
        [RegularExpression(@"^([äÄöÖüÜëËÆØÅæøåA-Za-z0-9 _]{1,100})$", ErrorMessage = "Feil i melding/KID! Har du skrevet riktig?")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Vennligst velg forfallsdato")]
        [Display(Name = "Forfallsdato")]
        [DisplayFormat(DataFormatString ="{dd-mm-yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Vennligst skriv inn et beløp (minimum kr 1)")]
        [Display(Name = "Kroner")]
        [RegularExpression(@"^([(\d]{1,9})$", ErrorMessage = "Feil i beløp, minimum overføring er 1 kr og maksimum er 1 milliard kr")]
        public int AmountKr { get; set; }

        // Ikke required
        [Display(Name = "Øre")]
        public int AmountOre { get; set; }
    }
}

