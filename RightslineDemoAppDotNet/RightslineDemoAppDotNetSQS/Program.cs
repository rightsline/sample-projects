using System;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using RightslineDemoAppDotNetSQS.Config;

namespace RightslineDemoAppDotNetSQS
{
    class Program
    {
        static void Main(string[] args)
        {
            //RestClient.GetSQSMessages();
            RestClient.StartBackgroundMonitoring();
            var y = Console.Read();
        }
    }
}