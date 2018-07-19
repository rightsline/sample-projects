using RightslineDemoAppDotNetSQS.Config;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AWSSignatureV4_S3_Sample.Signers;
using AWSSignatureV4_S3_Sample.Util;
using Microsoft.VisualBasic.CompilerServices;
using RightslineDemoAppDotNetSQS.Config;
using System.Timers;
//
// SOURCE : https://docs.aws.amazon.com/AmazonS3/latest/API/sig-v4-examples-using-sdks.html
//
namespace RightslineDemoAppDotNetSQS
{
    //For the purpose of this demo app, we will not be using the Amazon AWS SDK
    //We recommend using the SDK but we have created this example in case you are restricted from using it
    public class RestClient
    {
        private static string BaseUrl = "https://sqs.us-west-2.amazonaws.com/";
        private static Dictionary<string, string> config = ConfigSetup.GetConfigFile();
        private static int SecondsToPoll = 4;

        public static void GetSQSMessages()
        {
            string region = config["Region"];
            // Construct a virtual hosted style address with the bucket name part of the host address,
            // placing the region into the url if we're not using us-east-1.
            var regionUrlPart = string.Format("-{0}", region);           
            //This is our QA fifo queue, replace it with your queue's url
            var endpointUri = "https://sqs.us-west-2.amazonaws.com/013474081760/v2_qa_div29.fifo/";
            var requestParameters = "Action=ReceiveMessage&MaxNumberOfMessages=10";
            var uri = new Uri(endpointUri+"?"+requestParameters);
            var headers = new Dictionary<string, string>()
            {
                {AWS4SignerBase.X_Amz_Content_SHA256, AWS4SignerBase.EMPTY_BODY_SHA256}
//                {"content-type", "text/plain"}
            };
            var signer = new AWS4SignerForAuthorizationHeader()
            {
                EndpointUri = uri,
                HttpMethod = "GET",
                Service = "sqs",
                Region = region
            };

            var authorization = signer.ComputeSignature(headers, requestParameters, AWS4SignerBase.EMPTY_BODY_SHA256,
                config["AccessKey"], config["SecretKey"]);
            headers.Add("Authorization", authorization);
            
            string response = HttpHelpers.InvokeHttpRequest(uri, "GET", headers, null);
            Console.WriteLine(response);
        }

        [STAThread]
        public static async void StartBackgroundMonitoring()
        {
            Timer t = new Timer(SecondsToPoll * 1000); // 1 sec = 1000, 60 sec = 60000
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Start();
        }
        private static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetSQSMessages();
        }
    }
}