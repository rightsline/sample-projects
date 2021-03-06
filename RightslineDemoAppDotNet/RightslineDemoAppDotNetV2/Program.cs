﻿using RightslineDemoAppDotNetV2.Config;
using System;

namespace RightslineDemoAppDotNetV2
{
    /// <summary>
    /// All the IDs you see here are specific to the QA environment
    /// CHANGE THE IDs TO MATCH YOUR ENVIRONMENT
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ConfigSetup.GetConfigFile();
            //These IDs are for our QA database, change them 
            int featureID = 1565;
            int episodeID = 1555;
            int deleteID = 1591;
            Console.WriteLine("Rightsline Demo v0.0.1");
            Console.WriteLine("");
            //string CatalogItemGetJson = RestClient.GetCatalogItemDemoMethod(1587).Result;
            //Console.WriteLine(CatalogItemGetJson);
            //string CatalogItemPostID = RestClient.PostCatalogItemDemoMethod().Result;
            //Console.WriteLine("New Catalog Item ID: " + CatalogItemPostID);



            //string getTableResult = RestClient.GetTable(2340).Result;
            //Console.WriteLine(getTableResult);
            //string postTableResult = RestClient.PostTable().Result;
            //Console.WriteLine(postTableResult);
            //string putTableResult = RestClient.PutTable(2340).Result;
            //Console.WriteLine(putTableResult);
            //string deleteTable = RestClient.DeleteTable(2340).Result;
            //Console.WriteLine(deleteTable);

            //string getRelationships = RestClient.GetRelationships().Result;
            //Console.WriteLine(getRelationships);
            //string post = RestClient.PostRelationships().Result;
            string post = RestClient.PostTable().Result;
            Console.WriteLine(post);
            


        }
    }
}
