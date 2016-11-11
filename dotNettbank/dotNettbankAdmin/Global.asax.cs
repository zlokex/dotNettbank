using dotNettbank.DAL;
using EntityFramework.Audit;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
//using Z.EntityFramework.Plus;

namespace dotNettbankAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            System.Data.Entity.Database.SetInitializer(new BankInitializer());
            using (var db = new BankContext())
            {
                //var audit = new Audit();
                //audit.CreatedBy = "ZZZ Projects"; // Optional
                //db.SaveChanges(audit);

                //db.BeginAudit();
                //db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "ALTER DATABASE [" + db.Database.Connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                db.Database.Initialize(true);
                //new BankInitializer().InitializeDatabase(db);
            }
        }
    }
}
