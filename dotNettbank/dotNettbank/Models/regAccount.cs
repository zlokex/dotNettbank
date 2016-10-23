using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace dotNettbank.Models
{
    public class regAccount
    {
        [Required(ErrorMessage = "Kontonavn må oppgis")]
        [Display(Name = "Kontonavn")]
        public string Name { get; set; }
    }
}
