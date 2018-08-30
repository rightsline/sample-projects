using RightslineDemoAppDotNetV3.Config;
using System;
using System.IO;
using System.Text;

namespace RightslineDemoAppDotNetV3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ConfigSetup.GetConfigFile();

            //Replace the following IDs with your own
            //Console.WriteLine(RestClient.GetTableMethod("1890"));
            //Console.WriteLine(RestClient.GetCatalogItemMethod("1541"));

            //Console.WriteLine("New table created with id: " + RestClient.PostCatalogItemMethod("Catalog Item Example JSON/CatalogItemEpisodePOST.json"));
            //Console.WriteLine(RestClient.DeleteCatalogItemMethod("1547"));
            //Console.WriteLine(RestClient.UpdateCatalogItemMethod("1542", "Catalog Item Example JSON/CatalogItemEpisodePUT.json"));

            //Console.WriteLine(RestClient.PostRelationshipMethod("Relationship Example JSON/RelationshipPost.json"));
        }
    }
}
