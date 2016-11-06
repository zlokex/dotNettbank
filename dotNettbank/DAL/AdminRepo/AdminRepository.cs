using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNettbank.Model;
using dotNettbank.DAL;

namespace DAL.AdminRepo
{
    public class AdminRepository : IAdminRepository
    {
        public Admin getAdmin(string username)
        {
            using (var db = new BankContext())
            {
                var admin = db.Admins.Where(a => a.Username == username).First();
                return admin;
            }
        }

        public bool adminExists(string username)
        {
            using (var db = new BankContext())
            {
                Admin foundAdmin = db.Admins.FirstOrDefault
                (a => a.Username == username);
                if (foundAdmin == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public List<Payment> getAllPayments()
        {
            using (var db = new BankContext())
            {
                return db.Payments.ToList();
            }
        }

        public List<Payment> getPaymentsByFromAccountNo(string fromAccountNo)
        {
            using (var db = new BankContext())
            {
                List<Payment> payments = db.Payments.Where(b => b.FromAccountNo == fromAccountNo).ToList();
                return payments;
            }
        }

        /*
        public bool completePayment(Payment payment)
        {
            using (var db = new BankContext())
            {
                // Hent fra konto og til konto for betaling (for å oppdatere balanse):
                Account fraKonto = payment.FraKonto;
                Account tilKonto = payment.TilKonto;

                // Sjekk at fraKonto har høy nok saldo:
                if (fraKonto.Saldo <= betaling.Belop)
                {

                }
            }
        }
        */
    }
}

