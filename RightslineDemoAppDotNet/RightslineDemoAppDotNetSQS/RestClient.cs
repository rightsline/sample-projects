﻿using RightslineDemoAppDotNetSQS.Config;
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
using Newtonsoft.Json;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
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
        private static int SecondsToPoll = 10;
        private static string Regex = "\\{(.*?)\\}<";
        private static int MessagesToReceieve = 10;

        //This returns an XML string from the SQS queue that contains the 10 most recent messages. 
        public static string GetSQSMessages()
        {
            string region = config["Region"];
            // Construct a virtual hosted style address with the bucket name part of the host address,
            // placing the region into the url if we're not using us-east-1.
            var regionUrlPart = string.Format("-{0}", region);           
            //This is our QA fifo queue, replace it with your queue's url
            var endpointUri = "https://sqs.us-west-2.amazonaws.com/013474081760/v2_qa_div29.fifo/";
            var requestParameters = "Action=ReceiveMessage&MaxNumberOfMessages=" + MessagesToReceieve;
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
            //Console.WriteLine(response);
            return response;
        }

        [STAThread]
        public static async void StartBackgroundMonitoring()
        {
            //This is a timer that will poll the SQS queue every X seconds
            Timer t = new Timer(SecondsToPoll * 1000); // 1 sec = 1000, 60 sec = 60000
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Start();
        }
        private static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var messages = GetSQSMessages();
            var jsonobjects = FilterXML(messages);
            Notify(jsonobjects, "relationship");
        }
        public static void DemoMonitor()
        {
            var messages = GetSQSMessages();
            var jsonobjects = FilterXML(messages);
            Notify(jsonobjects, "relationship");
        }
        //Uses regex to filter the XML then converts it to JSON and builds JSON objects out of the results
        private static List<JObject> FilterXML(string messages)
        {
            Regex filterRegex = new Regex(Regex);
            var filteredMessages = filterRegex.Matches(messages);
            var messageValues = new List<string>();
            foreach (Match message in filteredMessages)
            {
                messageValues.Add(message.Value.Substring(0, message.Length - 1));
            }            
            var jsonObjects = new List<JObject>();
            foreach (string message in messageValues)
            {
                jsonObjects.Add(JObject.Parse(message));
            }
            return jsonObjects;
        }
        /// <summary>
        /// Writes to console whenever a message regarding a given an entity shows up.
        /// </summary>
        /// <param name="entityName"></param>
        private static void Notify(List<JObject> messages, string entityName)
        {
            Console.WriteLine("Poll at " + DateTimeOffset.UtcNow);
            int numMessages = 0;
            foreach(JObject message in messages)
            {
                if (message["entityUrl"].ToString().Contains(entityName))
                {
                    numMessages++;
                    Console.WriteLine("A " + entityName + " was " + message["action"] + ", URL: " + message["entityUrl"]);
                }
                //Console.WriteLine(message["action"] +  "  " + message["entityUrl"] );
                numMessages++;
            }
            if(numMessages == 0)
            {
                Console.WriteLine("No messages regarding " + entityName +" entities were recieved");
            }
        }
    }
}