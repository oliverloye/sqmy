using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using Umbraco.Web;
using UmbracoWebDev;

[assembly: OwinStartup("UmbracoStandardOwinStartup", typeof(Startup))]
namespace UmbracoWebDev
{


    //THIS IS MAGIC AND JUST WORKS!!!! WOOOOOOOOOH!!!!
    //TO GET THIS TO WORK IN YOUR UMBRACO PROJECT, OVERWRITE AND ADD THE FOLLOWING TO YOU UMBRACO Web.Config File
    /*   
          <add key="owin:appStartup" value="UmbracoStandardOwinStartup" />
          <add key="umbracoReservedPaths" value="~/umbraco,~/install/,~/hangfire" />  
    */

    public class Startup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            //ensure the default options are configured
            base.Configuration(app);


            
            // Configure hangfire
            var options = new SqlServerStorageOptions {
                PrepareSchemaIfNecessary = true,
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero
            };
            var connectionString = "server=PRAKTIKANTSQL01\\MSSQLSERVER2017;database=grp2_hangfire_db;user id = umbraco2; password='umbraco2'";

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(connectionString, options);

            // Give hangfire a URL and start the server                
            app.UseHangfireDashboard("/hangfire");
            app.UseHangfireServer();
        }
    }
}
