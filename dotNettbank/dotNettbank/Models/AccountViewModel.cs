using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class AccountViewModel
    {
        public string Type { get; set; }
        public string AccountNo { get; set; }
        public double Balance { get; set; }
    }
}