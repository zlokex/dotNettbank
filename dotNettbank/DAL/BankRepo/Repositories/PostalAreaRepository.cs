using DAL.Log;
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
        // DB context
        BankContext db;
        public PostalAreaRepository(BankContext bankContext)
        {
            db = bankContext;
        }

        // GET SINGLE MODEL
        public PostalArea getPostalAreaByCode(string postCode)
        {
            // Default value for string is null (if no customer is found)
            return db.PostalAreas.FirstOrDefault(p => p.PostCode == postCode);
        }

        // GET LIST OF MODELS

        public List<PostalArea> getAll()
        {
            return db.PostalAreas.ToList();
        }

        public List<PostalArea> getListByArea(string area)
        {
            return db.PostalAreas.Where(p => p.Area == area).ToList();
        }

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
                string log = "Failed to add postal area.\t" + e.Message + "\t" + e.StackTrace.ToString();
                Debug.Write(log);
                new LogErrors().errorLog(log);
                return false;
            }
        }

        // UPDATE
    }
}