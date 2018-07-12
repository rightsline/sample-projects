using RightslineDemoAppDotNetSQS.Config;
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

        private static string BaseUrl = "https://sqs.us-west-2.amazonaws.com/";

        public static void CreateUrl()
        {
            string url = BaseUrl + ConfigSetup.AccountId + "/" + ConfigSetup.QueueName + "/";
        }
    }
}