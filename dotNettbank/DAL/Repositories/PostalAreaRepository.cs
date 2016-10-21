using dotNettbank.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace dotNettbank.DAL.Repositories
{
    public class PostalAreaRepository
    {
        // Database Context
        BankContext db = new BankContext();

        // GET SINGLE MODEL

        public PostalArea addPostalArea(string postcode)
        {
            return null;
        }

        // GET LIST OF MODELS

        // INSERT / DELETE

        public bool addPostalArea(PostalArea postalArea)
        {
            try
            {
                db.PostalAreas.Add(postalArea);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e.Message);
                return false;
            }

        }

        // UPDATE
    }
}