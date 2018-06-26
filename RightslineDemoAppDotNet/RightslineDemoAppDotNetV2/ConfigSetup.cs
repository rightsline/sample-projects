using System;

namespace RightslineDemoAppDotNetV2
{
    public static class ConfigSetup
    {
        public static string FilePath { get; set; }
        public static void GetConfigFilePath()
        {
            Console.WriteLine("Please input the filepath to the config file: ");
            FilePath = Console.ReadLine();
        }
    }
}
