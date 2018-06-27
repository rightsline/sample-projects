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
            string x = RestClient.GetCatalogItemDemoMethod().Result;
            Console.WriteLine(x);
        }
    }
}
