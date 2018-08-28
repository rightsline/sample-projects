using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RightslineDemoAppDotNetSQS.Config;

namespace RightslineDemoAppDotNetSQS
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(RestClient.GetSQSMessages());
            //RestClient.StartBackgroundMonitoring();
            RestClient.DemoMonitor();
            var y = Console.Read();
            
        }
    }
}