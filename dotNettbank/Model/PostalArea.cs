﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dotNettbank.Model
{
    public class PostalArea
    {
        [Key]
        public string PostCode { get; set; } // Postnr
        public string Area { get; set; } // Poststed

        [InverseProperty("PostCode")]
        public virtual IEnumerable<Customer> Customers { get; set; }

    }
}