using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace RightslineDemoAppDotNetSQS.Config
{
    public class ConfigSetup
    {  
        private const string Algorithm = "AWS4-HMAC-SHA256";
        public static Dictionary<string, string> GetConfigFile()
        {
            var config = new Dictionary<string, string>();
            var folderPath =
                AppContext.BaseDirectory.Substring(0,
                    AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
            var filePath = Path.Combine(folderPath, "Config\\config.json");
            try
            {
                var credentials = JObject.Parse(File.ReadAllText(filePath));
                var AccountId = credentials.GetValue("AccountId").ToString();
                var QueueName = credentials.GetValue("QueueName").ToString();
                var AccessKey = credentials.GetValue("AccessKey").ToString();
                var SecretKey = credentials.GetValue("SecretKey").ToString();
                var Region = credentials.GetValue("Region").ToString();
                
                config.Add("AccountId", AccountId);
                config.Add("QueueName", QueueName);
                config.Add("AccessKey", AccessKey);
                config.Add("SecretKey", SecretKey);
                config.Add("Region", Region);
                config.Add("Algorithm", Algorithm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Please ensure that you have a valid config.json file in the " + folderPath +
                                  "Config folder.");
            }

            return config;
        }


    }
}