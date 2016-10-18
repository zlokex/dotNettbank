using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using dotNettbank.Model;

namespace dotNettbank.DAL
{
    public class BankInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            var postalAreas = new List<PostalArea>
            {
                new PostalArea {PostCode="0182",Area="Oslo"},
                new PostalArea {PostCode="0184",Area="Oslo"}
            };
            postalAreas.ForEach(s => context.PostalAreas.Add(s));
            context.SaveChanges();

            var customers = new List<Customer>
            {
                new Customer
                {
                    BirthNo = "12345123456",
                    FirstName = "André",
                    LastName = "Hovda",
                    Address = "Vindalveien 43",
                    PhoneNo = "94486775",
                    PostCode = "3219",
                    PostalArea = new PostalArea {PostCode="3219",Area="Sandefjord"},
                    Password = new byte[] {0x20, 0x20}
                }
            };
            customers.ForEach(s => context.Customers.Add(s));
            context.SaveChanges();
        }
    }
}