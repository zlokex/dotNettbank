using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{

    public class Account
    {
        [Key]
        public int AccountNo { get; set; }
        public double Balance { get; set; }

        public Customer Owner { get; set; } // Eier av konto

    }
}