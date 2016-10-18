using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    

    public class AccountStatement // Kontoutskrift
    {
        public List<AccountViewModel> Accounts { get; set; }

        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }

        public List<TransactionViewModel> Transactions { get; set; }

    }
}