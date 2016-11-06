using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class DuePayments
    {
        public List<AccountViewModel> Accounts { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public double Balance { get; set; }

        [Required(ErrorMessage = "Velg en startdato")]
        [Display(Name = "Startdato")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{dd-mm-yyyy}")]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Velg en sluttdato")]
        [Display(Name = "Sluttdato")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{dd-mm-yyyy}")]
        public DateTime toDate { get; set; }
    }
}