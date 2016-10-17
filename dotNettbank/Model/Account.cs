using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dotNettbank.Model
{

    public class Account
    {
        [Key]
        public int AccountNo { get; set; }
        public double Balance { get; set; }

        public virtual Customer Owner { get; set; } // Eier av konto

    }
}