using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace RightslineDemoAppDotNetV2.Config
{
    public static class ConfigSetup
    {        
        public static string BasicAuthCredentials { get; set; }
        /// <summary>
        /// This method sets up the credentials for the V2 API by reading a config.json file located in the Config folder
        /// </summary>
        public static void GetConfigFile()
        {            
            var folderPath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            var filePath = Path.Combine(folderPath, "Config\\config.json");
            try
            {
                var credentials = JObject.Parse(File.ReadAllText(filePath));                
                BasicAuthCredentials = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials.GetValue("user") + ":" + credentials.GetValue("password")));
                Console.WriteLine(BasicAuthCredentials);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Please ensure that you have a valid config.json file in the " + folderPath + "Config folder.");
            }          
        }
    }
}
