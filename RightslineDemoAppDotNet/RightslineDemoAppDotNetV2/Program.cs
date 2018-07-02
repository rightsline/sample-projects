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
            //string x = RestClient.GetCatalogItemDemo().Result;
            //Console.WriteLine(x);
            //string y = RestClient.PostCatalogItemDemo().Result;
            //Console.WriteLine("New Catalog Item ID: " + y);
            //string PutRequestResult = RestClient.PutCatalogItemDemo(1555).Result;
            //Console.WriteLine(PutRequestResult);
            string DeleteRequestResult = RestClient.DeleteCatalogItemDemo(1588).Result;
            Console.WriteLine(DeleteRequestResult);
            //string postTableResult = RestClient.PostTable().Result;
            //Console.WriteLine(postTableResult);
            //string putTableResult = RestClient.PutTable().Result;
            //Console.WriteLine(putTableResult);
        }
    }
}
