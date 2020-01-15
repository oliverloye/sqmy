using Hangfire;
using System;
using System.Web.Http;
using Umbraco.Web.WebApi;
using ClassLibrary.Solr;
using WebControllers.Helpers;
using Importer.Facade;

namespace UmbracoWebDev.Controllers
{
    
    public class HangfireController : UmbracoApiController
    {
        Program program = new Program();
    
        SolrIndexer si = new SolrIndexer();

        private string en_SqlConnection = ConnectionHelper.GetEnglishConnectionString();
        private string en_DailyUrl = UrlHelper.DailyEnglish();
        private string en_IndexFile = UrlHelper.IndexFileEnglish();

        private string dk_SqlConnection = ConnectionHelper.GetDanishConnectionString();
        private string dk_DailyUrl = UrlHelper.DailyDanish();
        private string dk_IndexFile = UrlHelper.IndexFileDanish();

        /*
         THE FOLLOWING METHODS ARE DIFFERENT WAYS OF ADDING JOBS TO THE HANGFIRE INTERFACE
            http://shop.localhost/umbraco/api/hangfire/ + THE METHODNAME
           THIS WILL PUT THE JOB IN THE METHOD UP FOR EXECUTION
         */


        // GET: Hangfire
        [HttpGet]
        public string Add() 
        {
            RecurringJob.AddOrUpdate("Hangfire test job v5", () => Console.WriteLine("testing hangfire job"), Cron.Minutely);
            RecurringJob.AddOrUpdate("This is to see if it has been updated", () => Console.WriteLine("v2"), Cron.Hourly);

            return "Job method ran!";
            
        }


        [HttpGet]
        public string EnglishDailyIndex()
        {
            //RecurringJob.AddOrUpdate("EN Read daily xmlLink from Icecat", () => program.<FetchIndex(en_DailyUrl, en_SqlConnection), Cron.Daily(00,00),TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate("EN Read daily xmlLink from Icecat", () => program.FetchIndex(en_DailyUrl), Cron.Daily(00, 00), TimeZoneInfo.Local);
            return "Fetching data from Icecat's daily english xml file. x";
        }

        [HttpGet]
        public string DanishDailyIndex()
        {
            //RecurringJob.AddOrUpdate("DK Read daily xmlLink from Icecat", () => program.FetchIndex(dk_DailyUrl, dk_SqlConnection), Cron.Daily(22, 00), TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate("DK Read daily xmlLink from Icecat", () => program.FetchIndex(dk_DailyUrl), Cron.Daily(00, 00), TimeZoneInfo.Local);
            return "Fetching data from Icecat's daily danish xml file.";
        }

        //[HttpGet]
        //public string FetchAndIndexDanishProducts()
        //{
        //    RecurringJob.AddOrUpdate("Checking for new danish products to update", () => program.FetchProducts(dk_SqlConnection), Cron.Daily(00, 00), TimeZoneInfo.Local);
        //    //RecurringJob.AddOrUpdate("DK SolarIndexer Converter", () => si.RunIndexingDanish(dk_SqlConnection), Cron.Daily(05, 00), TimeZoneInfo.Local);
        //    return "Fetching danish products...";
        //}

        [HttpGet]
        public string FetchAndIndexEnglishProducts()
        {
            RecurringJob.AddOrUpdate("Checking for new english products to update", () => program.FetchProducts(), Cron.Daily(00, 00), TimeZoneInfo.Local);
            //RecurringJob.AddOrUpdate("EN SolarIndexer Converter", () => si.RunIndexing(en_SqlConnection), Cron.Daily(05, 00), TimeZoneInfo.Local);
            return "Fetching english products...";
        }

        //[HttpGet]
        //public string EnglishIndex()
        //{
        //    BackgroundJob.Enqueue(() => program.FetchIndex(en_IndexFile, en_SqlConnection));

        //    return "Fetching data from english Icecat's index xml file.";
        //}


        [HttpGet]
        public string EnglishIndex()
        {
            BackgroundJob.Enqueue( () => program.FetchIndex(en_IndexFile));
            return "Fetching data from english Icecat's index xml file.";
        }
        
        [HttpGet]
        public string DanishIndex()
        {
            BackgroundJob.Enqueue( () => program.FetchIndex(dk_IndexFile));
            return "Fetching data from danish Icecat's index xml file.";
        }

      //  [HttpGet]
      //  public string DanishIndex()
      //  {
      ////      BackgroundJob.Enqueue(() => program.FetchIndex(dk_IndexFile, dk_SqlConnection));

      //      return "Fetching data from Icecat's danish index xml file.";
      //  }


        [HttpGet]
        public string FetchFeatures()
        {
            BackgroundJob.Enqueue(() => program.FetchFeatures());
            return "Fetching features from danish products in database.";
        }


        //[HttpGet]
        //public string FetchDanishFeatures()
        //{
        //    //BackgroundJob.Enqueue(() => program.FetchFeatures(dk_SqlConnection));

        //    return "Fetching features from danish products in database.";
        //}

        //[HttpGet]
        //public string FetchEnglishFeatures()
        //{
        //    //BackgroundJob.Enqueue(() => program.FetchFeatures(en_SqlConnection));

        //    return "Fetching features from danish products in database.";
        //}

        [HttpGet]
        public string FetchImages()
        {
            BackgroundJob.Enqueue(() => program.FetchImages());
            return "Fetching images and posts them as blobs to database.";
        }

        //[HttpGet]
        //public string FetchImages()
        //{
        //    BackgroundJob.Enqueue(() => program.FetchImages(en_SqlConnection));
        //    return "Fetching images and posts them as blobs to database.";
        //}

        [HttpGet]
        public string FetchCategories()
        {
            BackgroundJob.Enqueue(() => program.FetchCategories());
            return "Fethcing english categories...";
        }

        //[HttpGet]
        //public string FetchEnglishCategories()
        //{
        //    //BackgroundJob.Enqueue(() => program.FetchCategories(en_SqlConnection));
        //    return "Fethcing english categories...";
        //}

        //[HttpGet]
        //public string FetchDanishCategories()
        //{
        //    //BackgroundJob.Enqueue(() => program.FetchCategories(dk_SqlConnection));
        //    return "Fethcing danish categories...";
        //}

        [HttpGet]
        public string  Schedule()
        {
            //BackgroundJob.Schedule(() => program.FetchIndex(en_IndexFile, en_SqlConnection), TimeSpan.FromMinutes(240));
            //BackgroundJob.Schedule(() => program.FetchIndex(dk_IndexFile, dk_SqlConnection), TimeSpan.FromMinutes(240));
            return "Scheuled all jobs";
        }

        [HttpGet]
        public string PopulateEnglishDB()
        {
            RecurringJob.AddOrUpdate("Populating Whole DB", () => program.PopulateWholeDatabase(), Cron.Daily(00, 00), TimeZoneInfo.Local);
            //RecurringJob.AddOrUpdate("EN SolarIndexer Converter", () => si.RunIndexing(en_SqlConnection), Cron.Daily(05, 00), TimeZoneInfo.Local);
            return "Fetching english products...";
        }

    }
}