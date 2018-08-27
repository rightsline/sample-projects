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
            //RestClient.DemoMonitor();
            var y = Console.Read();
            Console.WriteLine(WebUtility.UrlEncode("AQEB9/Gw1pZFcFOKdVa8UZWaisLDR5Hh3HVAHpZvb006Yw47MnwjNxQ8niy0iYRgpB75qIwM64mbhVWR11iWl8NKAtQwY6r1YCzYX1ZsJgPhn8GrSEbbOgErqGqoe9qwaIqcr8y8s4R3gNIuRQvJMIbaC8GnjB2f/WvY0ip Z68vD85QxyaMVvOmwY0ERKcMIoVfykA6h1Y n11Q9lrc1qjBDQ8rftAtO01QS1r3bCRQOImp0IHHU5jLMg0NZTUAkisCxVmbZKvoKD2GvNVGioX5YtzP2rI6qEHwA1gEz5RYsEI="));
        }
    }
}