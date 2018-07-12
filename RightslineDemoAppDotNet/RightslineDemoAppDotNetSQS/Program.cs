using System;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using RightslineDemoAppDotNetSQS.Config;

namespace RightslineDemoAppDotNetSQS
{
    class Program
    {
        static HttpClient client = new HttpClient();
        private static string BaseUrl = "https://sqs.us-west-2.amazonaws.com";
        RestClient restClient = new RestClient();

        static void Main(string[] args)
        {
            var result = RestClient.Get().Result;
            Console.WriteLine(result);
        }
    }
}