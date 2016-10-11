using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbank.Models
{
    public class PostalArea
    {
        [Key]
        public int PostCode { get; set; } // Postnr
        public string Area { get; set; } // Poststed
    }
}