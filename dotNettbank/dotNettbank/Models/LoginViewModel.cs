﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Fødselsnummer må oppgis")]
        [Display(Name ="Fødselsnummer")]
        [RegularExpression(@"^((0[1-9]|[12]\d|3[01])([04][1-9]|[15][0-2])\d{7})$", ErrorMessage = "Ugyldig fødselsnummer")]
        public string BirthNo { get; set; }

        [Required(ErrorMessage = "Passord må oppgis")]
        [Display(Name ="Passord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Engangskode må oppgis")]
        [Display(Name = "Engangskode (BankID)")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^([(\d]{6})$", ErrorMessage = "Feil engangskode, prøv igjen (seks siffer)")]
        public string BankID { get; set; }
    }
}