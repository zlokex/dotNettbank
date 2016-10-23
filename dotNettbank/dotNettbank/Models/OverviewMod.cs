using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class OverviewMod
    {

        public List<AccountViewModel> Accounts { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public double Balance { get; set; }
    }
}