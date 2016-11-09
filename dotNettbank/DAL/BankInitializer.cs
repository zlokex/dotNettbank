using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using dotNettbank.Model;
using System.Security.Cryptography;
using System.Text;

namespace dotNettbank.DAL
{
    public class BankInitializer : DropCreateDatabaseIfModelChanges<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            generateDefaultAdmin();
            generatePostalAreas(context);
            generateCustomers(context);
            generateAccounts(context);
            generatePayments(context);
            generateTransactions(context);
        }

        // ---------- ADMINS ---------
        public static void generateDefaultAdmin()
        {

            string username = "admin";
            string password = "admin";
            string email = "defaultAdmin@gmail.com";

            byte[] hashedpassword = createHash(password);

            using (var db = new BankContext())
            {
                var nyadmin = new Admin();
                nyadmin.Username = username;
                nyadmin.Password = hashedpassword;
                nyadmin.Email = email;

                db.Admins.Add(nyadmin);
                db.SaveChanges();
            }
        }

        // ---------- POSTAL AREAS ---------
        public static void generatePostalAreas(BankContext context)
        {
            var postalAreas = new List<PostalArea>
            {
                new PostalArea {PostCode="0182",Area="Oslo"},
                new PostalArea {PostCode="0184",Area="Oslo"},
                new PostalArea {PostCode="3219",Area="Sandefjord"}
            };
            postalAreas.ForEach(s => context.PostalAreas.Add(s));
            context.SaveChanges();
        }

        // ---------- CUSTOMERS ---------
        public static void generateCustomers(BankContext context)
        {
            string password = "Test123";
            string salt = generateSalt();
            byte[] hashedPassword = createHash(password + salt);
            var customers = new List<Customer>
            {
                new Customer
                {
                    BirthNo = "01018912345",
                    FirstName = "Hans",
                    LastName = "Hansen",
                    Address = "Vindalveien 43",
                    PhoneNo = "94486775",
                    PostCode = "3219",
                    Password = hashedPassword,
                    Salt = salt
                },
                new Customer
                {
                    BirthNo = "01010199887",
                    FirstName = "Per",
                    LastName = "Persen",
                    Address = "Storgata 60",
                    PhoneNo = "95008989",
                    PostCode = "0182",
                    Password = hashedPassword,
                    Salt = salt
                },
                new Customer
                {
                    BirthNo = "031276105610",
                    FirstName = "Jens",
                    LastName = "Jensen",
                    Address = "Storgata 71",
                    PhoneNo = "90678345",
                    PostCode = "0182",
                    Password = hashedPassword,
                    Salt = salt
                }
            };
            customers.ForEach(s => context.Customers.Add(s));
            context.SaveChanges();
        }

        // ---------- ACCOUNTS ---------
        public static void generateAccounts(BankContext context)
        {
            var accounts = new List<Account>
            {
                new Account {AccountNo="10000000000",Balance=10000,Type="Spare",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="10000000001",Balance=10000,Type="BSU",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="10000000002",Balance=10000,Type="Bruk1",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="10000000003",Balance=10000,Type="Bruk2",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="10000000004",Balance=10000,Type="Bruk3",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="10000000005",Balance=10000,Type="Bruk4",InterestRate=1.2,OwnerBirthNo="01018912345"},

                new Account {AccountNo="10000000006",Balance=10000,Type="Spare1",InterestRate=1.2,OwnerBirthNo="01010199887"},
                new Account {AccountNo="10000000007",Balance=10000,Type="Spare2",InterestRate=1.2,OwnerBirthNo="01010199887"},
                new Account {AccountNo="10000000008",Balance=10000,Type="Spare3",InterestRate=1.2,OwnerBirthNo="01010199887"},
                new Account {AccountNo="10000000009",Balance=10000,Type="Spare4",InterestRate=1.2,OwnerBirthNo="01010199887"},
                new Account {AccountNo="10000000010",Balance=10000,Type="Spare5",InterestRate=1.2,OwnerBirthNo="01010199887"},
            };
            accounts.ForEach(s => context.Accounts.Add(s));
            context.SaveChanges();
        }

        // ---------- PAYMENTS ---------
        public static void generatePayments(BankContext context)
        {
            DateTime dateNow = DateTime.Now;
            DateTime datePast = new DateTime(2016, 10, 20);
            DateTime dateFuture = new DateTime(2017, 10, 20);

            var payments = new List<Payment>
            {
                new Payment {PaymentID=1,DateAdded=dateNow,DueDate=dateNow,Amount=100.12,Message="Beskrivelse",FromAccountNo="10000000000",ToAccountNo="10000000010"},
                new Payment {PaymentID=2,DateAdded=dateNow,DueDate=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="10000000000",ToAccountNo="10000000010"},
                new Payment {PaymentID=3,DateAdded=datePast,DueDate=datePast,Amount=1000,Message="Beskrivelse",FromAccountNo="10000000001",ToAccountNo="10000000010"},
                new Payment {PaymentID=4,DateAdded=datePast,DueDate=datePast,Amount=200,Message="Beskrivelse",FromAccountNo="10000000002",ToAccountNo="10000000009"},
                new Payment {PaymentID=5,DateAdded=dateFuture,DueDate=dateFuture,Amount=100.54,Message="Beskrivelse",FromAccountNo="10000000000",ToAccountNo="10000000010"},
                new Payment {PaymentID=6,DateAdded=dateFuture,DueDate=dateFuture,Amount=100,Message="Beskrivelse",FromAccountNo="10000000000",ToAccountNo="10000000010"},

                new Payment {PaymentID=7,DateAdded=dateNow,DueDate=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
                new Payment {PaymentID=8,DateAdded=dateNow,DueDate=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
                new Payment {PaymentID=9,DateAdded=datePast,DueDate=datePast,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
                new Payment {PaymentID=10,DateAdded=datePast,DueDate=datePast,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
                new Payment {PaymentID=11,DateAdded=dateFuture,DueDate=dateFuture,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
            };

            payments.ForEach(s => context.Payments.Add(s));
            context.SaveChanges();
        }

        // ---------- TRANSACTIONS ---------
        public static void generateTransactions(BankContext context)
        {
            DateTime dateNow = DateTime.Now;
            DateTime datePast = new DateTime(2016, 10, 20);
            DateTime dateFuture = new DateTime(2017, 10, 20);
            var transactions = new List<Transaction>
            {
                new Transaction {TransactionID=1,DatePayed=dateNow,Date=dateNow,Amount=100.12,Message="Beskrivelse",FromAccountNo="10000000000",ToAccountNo="10000000010"},
                new Transaction {TransactionID=2,DatePayed=dateNow,Date=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="10000000000",ToAccountNo="10000000010"},
                new Transaction {TransactionID=3,DatePayed=datePast,Date=datePast,Amount=1000,Message="Beskrivelse",FromAccountNo="10000000001",ToAccountNo="10000000010"},
                new Transaction {TransactionID=4,DatePayed=datePast,Date=datePast,Amount=200,Message="Beskrivelse",FromAccountNo="10000000002",ToAccountNo="10000000009"},
                new Transaction {TransactionID=5,DatePayed=dateFuture,Date=dateFuture,Amount=100.54,Message="Beskrivelse",FromAccountNo="10000000000",ToAccountNo="10000000010"},
                new Transaction {TransactionID=6,DatePayed=dateFuture,Date=dateFuture,Amount=100,Message="Beskrivelse",FromAccountNo="10000000000",ToAccountNo="10000000010"},

                new Transaction {TransactionID=7,DatePayed=dateNow,Date=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
                new Transaction {TransactionID=8,DatePayed=dateNow,Date=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
                new Transaction {TransactionID=9,DatePayed=datePast,Date=datePast,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
                new Transaction {TransactionID=10,DatePayed=datePast,Date=datePast,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
                new Transaction {TransactionID=11,DatePayed=dateFuture,Date=dateFuture,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="10000000000"},
            };
            transactions.ForEach(s => context.Transactions.Add(s));
            context.SaveChanges();
        }


        //--- PRIVATE METHODS ---

        private static string generateSalt()
        {
            byte[] randomArray = new byte[10];
            string randomString;

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            randomString = Convert.ToBase64String(randomArray);
            return randomString;
        }

        //TODO Lag en try catch for tilfellet hvor passord ikke er skrevet inn
        private static byte[] createHash(string innStreng)
        {
            byte[] innData, utData;
            var algoritme = SHA256.Create();
            innData = Encoding.UTF8.GetBytes(innStreng);
            utData = algoritme.ComputeHash(innData);
            return utData;
        }

        private string HashString(string innStreng)
        {
            byte[] hash = createHash(innStreng);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}