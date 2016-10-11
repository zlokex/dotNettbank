using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name ="Fødeselsnummer")]
        public string BirthNo { get; set; }

        [Required]
        [Display(Name ="Passord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}