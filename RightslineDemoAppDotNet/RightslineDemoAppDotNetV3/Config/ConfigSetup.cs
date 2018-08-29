using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace RightslineDemoAppDotNetV3.Config
{
    public static class ConfigSetup
    {        
        public static string AccessKey { get; set; }
        public static string SecretKey { get; set; }
        public static string ApiKey { get; set; }
        /// <summary>
        /// This method sets up the credentials for the V3 API by reading a config.json file located in the Config folder
        /// </summary>
        public static void GetConfigFile()
        {            
            var folderPath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
            var filePath = Path.Combine(folderPath, "Config\\config.json");
            try
            {
                var credentials = JObject.Parse(File.ReadAllText(filePath));
                AccessKey = credentials.GetValue("accessKey").ToString();
                SecretKey = credentials.GetValue("secretKey").ToString();
                ApiKey = credentials.GetValue("xApiKey").ToString();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Please ensure that you have a valid config.json file in the " + folderPath + "Config folder.");
            }          
        }
    }
}
