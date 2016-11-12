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
            string email = "defaultAdmin@hardbank.no";

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
                new PostalArea {PostCode="3219",Area="Sandefjord"},
                new PostalArea {PostCode="7130",Area="Brekstad" }
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
                    BirthNo = "03127610561",
                    FirstName = "Jens",
                    LastName = "Jensen",
                    Address = "Storgata 71",
                    PhoneNo = "90678345",
                    PostCode = "0182",
                    Password = hashedPassword,
                    Salt = salt
                },
                new Customer
                {
                    BirthNo = "22058444767",
                    FirstName = "Karl",
                    LastName = "Rædergård",
                    Address = "Grandveien 505",
                    PhoneNo = "72523492",
                    PostCode = "7130",
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
                new Account {AccountNo="71740105555",Balance=63090,Type="Sparekonto",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="61050276597",Balance=250000,Type="BSU",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="61050298882",Balance=145000,Type="BSU 2.0",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="33030944328",Balance=1200,Type="Regningskonto",InterestRate=1.2,OwnerBirthNo="01018912345"},
                new Account {AccountNo="33030938948",Balance=12194,Type="Brukskonto",InterestRate=1.2,OwnerBirthNo="01018912345"},

                new Account {AccountNo="71740188778",Balance=13090,Type="Sparekonto",InterestRate=1.2,OwnerBirthNo="01010199887"},
                new Account {AccountNo="61050291919",Balance=50000,Type="BSU",InterestRate=1.2,OwnerBirthNo="01010199887"},
                new Account {AccountNo="61050243543",Balance=15000,Type="BSU 2.0",InterestRate=1.2,OwnerBirthNo="01010199887"},
                new Account {AccountNo="33030900234",Balance=760,Type="Regningskonto",InterestRate=1.2,OwnerBirthNo="01010199887"},
                new Account {AccountNo="33030950020",Balance=4284,Type="Brukskonto",InterestRate=1.2,OwnerBirthNo="01010199887"},

                new Account {AccountNo="71740170011",Balance=600,Type="Sparekonto",InterestRate=1.2,OwnerBirthNo="031276105610"},
                new Account {AccountNo="61050298882",Balance=10506,Type="BSU",InterestRate=1.2,OwnerBirthNo="031276105610"},
                new Account {AccountNo="61050244012",Balance=0,Type="BSU 2.0",InterestRate=1.2,OwnerBirthNo="031276105610"},
                new Account {AccountNo="33030917112",Balance=259,Type="Regningskonto",InterestRate=1.2,OwnerBirthNo="031276105610"},
                new Account {AccountNo="33030976939",Balance=805,Type="Brukskonto",InterestRate=1.2,OwnerBirthNo="031276105610"},

                new Account {AccountNo="71740122219",Balance=302540,Type="Sparekonto",InterestRate=1.2,OwnerBirthNo="22058444767"},
                new Account {AccountNo="61050290786",Balance=250700,Type="BSU",InterestRate=1.2,OwnerBirthNo="22058444767"},
                new Account {AccountNo="61050295007",Balance=245066,Type="BSU 2.0",InterestRate=1.2,OwnerBirthNo="22058444767"},
                new Account {AccountNo="33030940219",Balance=2300,Type="Regningskonto",InterestRate=1.2,OwnerBirthNo="22058444767"},
                new Account {AccountNo="33030991846",Balance=42030,Type="Brukskonto",InterestRate=1.2,OwnerBirthNo="22058444767"},
            };
            accounts.ForEach(s => context.Accounts.Add(s));
            context.SaveChanges();
        }

        // ---------- PAYMENTS ---------
        public static void generatePayments(BankContext context)
        {
            DateTime dateNow = DateTime.Now;
            DateTime datePast = new DateTime(2016, 10, 20);
            DateTime dateFuture = new DateTime(2016, 12, 24);

            var payments = new List<Payment>
            {
                new Payment {PaymentID=1,DateAdded=dateNow,DueDate=dateNow,Amount=44.50,Message="Takk for pizza i går",FromAccountNo="71740105555",ToAccountNo="33030976939"},
                new Payment {PaymentID=2,DateAdded=dateNow,DueDate=dateNow,Amount=500,Message="Ukelønn for desember",FromAccountNo="71740105555",ToAccountNo="71740170011"},
                new Payment {PaymentID=3,DateAdded=datePast,DueDate=datePast,Amount=1200,Message="Gave fra bestemor",FromAccountNo="61050276597",ToAccountNo="71740170011"},
                new Payment {PaymentID=4,DateAdded=datePast,DueDate=datePast,Amount=299.52,Message="Regning Djuice L",FromAccountNo="33030940219",ToAccountNo="33030950020"},
                new Payment {PaymentID=5,DateAdded=dateFuture,DueDate=dateFuture,Amount=3995,Message="Ny DBS sykkel fra Finn.no",FromAccountNo="71740105555",ToAccountNo="33030976939"},
                new Payment {PaymentID=6,DateAdded=dateFuture,DueDate=dateFuture,Amount=512.32,Message="Biff middag med gutta",FromAccountNo="71740105555",ToAccountNo="33030976939"},

                new Payment {PaymentID=7,DateAdded=dateNow,DueDate=dateNow,Amount=149,Message="DVD-Tarzan",FromAccountNo="33030938948",ToAccountNo="71740105555"},
                new Payment {PaymentID=8,DateAdded=dateNow,DueDate=dateNow,Amount=5000,Message="Årlig innskudd til BSU",FromAccountNo="71740122219",ToAccountNo="61050295007"},
                new Payment {PaymentID=9,DateAdded=datePast,DueDate=datePast,Amount=700,Message="Sparing til bil",FromAccountNo="33030938948",ToAccountNo="33030976939"},
                new Payment {PaymentID=10,DateAdded=datePast,DueDate=datePast,Amount=2900,Message="Helgetur til Kjøbenhavn",FromAccountNo="33030991846",ToAccountNo="33030976939"},
                new Payment {PaymentID=11,DateAdded=dateFuture,DueDate=dateFuture,Amount=350,Message="Kinotur for Frida",FromAccountNo="33030991846",ToAccountNo="33030976939"},
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
                new Transaction {TransactionID=1,DatePayed=dateNow,Date=dateNow,Amount=100.12,Message="Beskrivelse",FromAccountNo="71740105555",ToAccountNo="10000000010"},
                new Transaction {TransactionID=2,DatePayed=dateNow,Date=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="71740105555",ToAccountNo="10000000010"},
                new Transaction {TransactionID=3,DatePayed=datePast,Date=datePast,Amount=1000,Message="Beskrivelse",FromAccountNo="61050276597",ToAccountNo="10000000010"},
                new Transaction {TransactionID=4,DatePayed=datePast,Date=datePast,Amount=200,Message="Beskrivelse",FromAccountNo="61050298882",ToAccountNo="10000000009"},
                new Transaction {TransactionID=5,DatePayed=dateFuture,Date=dateFuture,Amount=100.54,Message="Beskrivelse",FromAccountNo="71740105555",ToAccountNo="10000000010"},
                new Transaction {TransactionID=6,DatePayed=dateFuture,Date=dateFuture,Amount=100,Message="Beskrivelse",FromAccountNo="71740105555",ToAccountNo="10000000010"},

                new Transaction {TransactionID=7,DatePayed=dateNow,Date=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="71740105555"},
                new Transaction {TransactionID=8,DatePayed=dateNow,Date=dateNow,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="71740105555"},
                new Transaction {TransactionID=9,DatePayed=datePast,Date=datePast,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="71740105555"},
                new Transaction {TransactionID=10,DatePayed=datePast,Date=datePast,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="71740105555"},
                new Transaction {TransactionID=11,DatePayed=dateFuture,Date=dateFuture,Amount=100,Message="Beskrivelse",FromAccountNo="10000000010",ToAccountNo="71740105555"},
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