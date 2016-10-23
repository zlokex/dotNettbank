using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL
{

    public class BankContext : DbContext
    {
        public BankContext() : base("name=Bank")
        {
            //Database.CreateIfNotExists();
            Database.SetInitializer<BankContext>(new BankInitializer());
            //Database.SetInitializer<BankContext>(new CreateDatabaseIfNotExists<BankContext>());
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PostalArea> PostalAreas { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}