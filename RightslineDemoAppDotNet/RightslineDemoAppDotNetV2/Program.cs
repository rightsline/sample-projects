using RightslineDemoAppDotNetV2.Config;
using System;

namespace RightslineDemoAppDotNetV2
{
    class Program
    {
        static void Main(string[] args)
        {            
            ConfigSetup.GetConfigFile();
            RestClient rc = new RestClient();
//            string x = RestClient.GetCatalogItemDemoMethod().Result;
//            Console.WriteLine(x);
//            string y = RestClient.PostCatalogItemDemoMethod().Result;
//            Console.WriteLine("New Catalog Item ID: " + y);
            string postTableResult = RestClient.PutTable().Result;
            Console.WriteLine(postTableResult);
        }
    }
}
