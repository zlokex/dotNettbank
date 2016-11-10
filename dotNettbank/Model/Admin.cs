using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNettbank.Model
{
    public class Admin
    {
        [Key]
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }

        
    }
}
