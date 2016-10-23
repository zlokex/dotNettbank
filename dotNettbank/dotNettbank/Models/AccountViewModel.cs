using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class AccountViewModel
    {
        [Display(Name="Konto")]
        public string Type { get; set; }
        [Display(Name = "Kontonummer")]
        public string AccountNo { get; set; }
        [Display(Name = "Balanse")]
        public double Balance { get; set; }
    }
}