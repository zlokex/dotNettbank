using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotNettbank.Models
{
    

    public class AccountStatement // Kontoutskrift
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

        public List<TransactionViewModel> Transactions { get; set; }

    }
}