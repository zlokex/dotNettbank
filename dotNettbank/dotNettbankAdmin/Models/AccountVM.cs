using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbankAdmin.Models
{
    public class AccountVM
    {
        [Required(ErrorMessage = "Eier kan ikke være blankt")]
        [Display(Name = "Eier")]
        public string OwnerBirthNo { get; set; }

        [Required(ErrorMessage = "Kontonummer kan ikke være blankt")]
        [Display(Name = "Kontonummer")]
        public string AccountNo { get; set; } //kontonr

        [Required(ErrorMessage = "Balanse kan ikke være blankt")]
        [Display(Name = "Balanse")]
        public double Balance { get; set; } //Saldo

        [Required(ErrorMessage = "Kontonavn kan ikke være blankt")]
        [Display(Name = "Kontonavn")]
        public string Type { get; set; } // Kontotype
    }
}