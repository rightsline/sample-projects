using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RightslineDemoAppDotNetSQS.Config
{
    public class ConfigSetup
    {
        public static string AccountId { get; set; }
        public static string QueueName { get; set; }
        public static string AccessKey { get; set; }
        public static string SecretKey { get; set; }
        public static string Region = "us-west-2";
        
        public static void GetConfigFile()
        {
            var folderPath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
            var filePath = Path.Combine(folderPath, "Config\\config.json");
            try
            {
                var credentials = JObject.Parse(File.ReadAllText(filePath));
                AccountId = credentials.GetValue("AccountId").ToString();
                QueueName = credentials.GetValue("QueueName").ToString();
                AccessKey = credentials.GetValue("AccessKey").ToString();
                SecretKey = credentials.GetValue("SecretKey").ToString();              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Please ensure that you have a valid config.json file in the " + folderPath + "Config folder.");
            }
        }
        private static string Encode()
        {
            
        }

    }
}
