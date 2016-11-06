using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbankAdmin.Models
{
    public class AdminSideModel
    {
        public List<Payment> payments { get; set; }

        public AdminSideModel(List<Payment> betalingListen)
        {
            payments = betalingListen;
        }
    }
}