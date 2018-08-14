using RightslineDemoAppDotNetV3.Config;
using System;

namespace RightslineDemoAppDotNetV3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ConfigSetup.GetConfigFile();
            //string sessionToken = RestClient.GetSessionToken().Result;
            //Console.WriteLine(sessionToken);
            Console.WriteLine(RestClient.GetCatalogItemMethod("1051"));
        }
    }
}
