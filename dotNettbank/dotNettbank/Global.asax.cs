using dotNettbank.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace dotNettbank
{
    public class MvcApplication : System.Web.HttpApplication
    {

        private static System.Timers.Timer sTimer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //Database.SetInitializer<BankContext>(new BankInitializer());
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            try
            {
                SetTimer();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void SetTimer()
        {
            DateTime tomorow = DateTime.Now.AddDays(1);
            DateTime midnight = new DateTime(tomorow.Year, tomorow.Month, tomorow.Day, 0, 0, 0); // Starting first midnight
            TimeSpan d = midnight - DateTime.Now;

            sTimer.Enabled = true;
            sTimer.Interval = d.TotalMilliseconds;
            sTimer.Elapsed += new System.Timers.ElapsedEventHandler(sTimer_Elapsed);
            sTimer.Start();
        }

        static void sTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            sTimer.Interval = TimeSpan.FromHours(24).TotalMilliseconds; // Set interval to once every 24 hours:

            // Call method in BLL to complete due payments:
        }
    }
}
