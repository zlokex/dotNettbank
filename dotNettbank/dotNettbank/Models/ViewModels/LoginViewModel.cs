using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Fødselsnummer må oppgis")]
        [Display(Name ="Fødeselsnummer")]
        public string BirthNo { get; set; }

        [Required(ErrorMessage = "Passord må oppgis")]
        [Display(Name ="Passord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}