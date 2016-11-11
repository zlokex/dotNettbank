using BLL.AdminService;
using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dotNettbankAdmin.Models
{
    public class PaymentVM
    {
        [Display(Name = "Dato")]
        public DateTime DueDate { get; set; } // Forfallsdato



        [Required(ErrorMessage = "Beløpet må fylles ut")]
        [Display(Name = "Beløp")]
        public double Amount { get; set; } // Beløp

        [Display(Name = "KID")]
        public string Message { get; set; } // KID eller melding

        [CustomValidation(typeof(PaymentVM), "accountExist")]
        [Required(ErrorMessage = "Må fylles ut")]
        [Display(Name = "Fra konto")]
        public string FromAccountNo { get; set; }

        [CustomValidation(typeof(PaymentVM), "accountExist")]
        [Required(ErrorMessage = "Må fylles ut")]
        [Display(Name = "Til Konto")]
        public string ToAccountNo { get; set; }

        public static ValidationResult accountExist(string test)
        {
            AdminService db = new AdminService();
            Account acc = db.getAccountByAccountNo(test);
            if(acc != null)
            {
                return ValidationResult.Success;
            }else
            {
                return new ValidationResult("Ingen kontoer med '" + test + "' som kontonr");
            }
        }
    }
}