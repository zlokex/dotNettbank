using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    

    public class AccountStatement // Kontoutskrift
    {
        public List<AccountViewModel> Accounts { get; set; }
        public AccountViewModel Account { get; set; }

        [Required(ErrorMessage = "Velg en startdato")]
        [Display(Name = "Startdato")]
        [DataType(DataType.DateTime)]
        public DateTime fromDate { get; set; }

        [Required(ErrorMessage = "Velg en sluttdato")]
        [Display(Name = "Sluttdato")]
        [DataType(DataType.DateTime)]
        public DateTime toDate { get; set; }

        public List<TransactionViewModel> Transactions { get; set; }

    }
}