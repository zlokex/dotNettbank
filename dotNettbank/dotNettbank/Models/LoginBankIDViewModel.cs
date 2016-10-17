using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class LoginBankIDViewModel
    {
        [Required]
        [Display(Name = "Fødeselsnummer")]
        public string BirthNo { get; set; }

        [Required]
        [Display(Name = "Telefonnummer")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        [Required]
        [Display(Name = "Konfirmasjonskode")]
        [DataType(DataType.Password)]
        public string Confirmation { get; set; }
    }
}