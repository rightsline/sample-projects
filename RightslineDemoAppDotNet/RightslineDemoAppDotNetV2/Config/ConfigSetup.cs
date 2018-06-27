using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace RightslineDemoAppDotNetV2.Config
{
    public static class ConfigSetup
    {        
        public static void GetConfigFile()
        {            
            var folderPath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            var filePath = Path.Combine(folderPath, "Config\\config.json");
            try
            {
                var credentials = JObject.Parse(File.ReadAllText(filePath));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Please ensure that you have a valid config.json file in the " + folderPath + "Config folder.");
            }          
        }
    }
}
