using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RightslineDemoAppDotNetSQS.Config;


namespace RightslineDemoAppDotNetSQS
{
    //For the purpose of this demo app, we will not be using the Amazon AWS SDK
    //We recommend using the SDK but we have created this example in case you are restricted from using it
    public class RestClient
    {
        private static string BaseUrl = "https://sqs.us-west-2.amazonaws.com";

        private static string MyUrl =
            "https://sqs.us-west-2.amazonaws.com/013474081760/v2_qa_div29.fifo/?Action=ReceiveMessage&MaxNumberOfMessages=10";

        static ConfigSetup _configSetup = new ConfigSetup();
        static HttpClient client = new HttpClient();

        public static async Task<String> Get()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, MyUrl);
            var newRequestMessage = await _configSetup.Sign(requestMessage, "sqs");
            var results = await client.SendAsync(newRequestMessage);
            
            return results.Content.ToString();
        }
    }
}