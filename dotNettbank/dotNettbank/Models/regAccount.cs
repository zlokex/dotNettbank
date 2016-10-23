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
        [RegularExpression(@"^([äÄöÖüÜéÉëËÆØÅæøåA-Za-z]{1,20})$", ErrorMessage = "Vennligst fyll inn kontonavn")]
        public string Name { get; set; }
    }
}
