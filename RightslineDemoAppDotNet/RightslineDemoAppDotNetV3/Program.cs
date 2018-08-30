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
            Console.WriteLine("Rightsline V3 Demo App");
            ConfigSetup.GetConfigFile();

            //Replace the following IDs with your own
            //Console.WriteLine(RestClient.GetTableMethod("1890"));
            //Console.WriteLine(RestClient.GetCatalogItemMethod("1541"));

            var jsonObject = File.ReadAllText("Catalog Item Example JSON/CatalogItemEpisodePOST.json");
            //var encodedJsonBytes = Encoding.UTF8.GetBytes("﻿{    \"parentRelationship\": null,  \"revisionId\": 0,  \"title\": \"Example Episode POST\",  \"template\": {    \"fields\": null,    \"templateId\": 13,    \"templateName\": \"Episode\",    \"processID\": 0,    \"processName\": \"null\"  },  \"status\": {    \"statusId\": 1,    \"statusName\": \"Development\"  },  \"characteristics\": {    \"format\": \"HDCAM\",    \"genre\": [\"Action\", \"Adventure\", \"Animation\"],    \"file_format_type\": \"SD\",    \"subtitle_format\": \"EIA - 708(DTVCC Transport) text stream\",    \"subtitle_language\": \"English(US)\",    \"original_air_date\": \"06 / 25 / 2018\",    \"release_date\": \"06 / 27 / 2018\"  }}");
            //var encodedJsonBytes = Encoding.UTF8.GetBytes("test string 123");
            var encodedJsonBytes = Encoding.UTF8.GetBytes(jsonObject);
            string test = "";
            foreach(byte b in encodedJsonBytes)
            {
                test += b;
            }
            var hashedJsonString = RestClient.ComputeSHA256Hash(encodedJsonBytes);
            Console.WriteLine(hashedJsonString);
            //Console.WriteLine("New Episode created with id: " + RestClient.PostCatalogItemMethod("Catalog Item Example JSON/CatalogItemEpisodePOST.json"));
            //Console.WriteLine(RestClient.DeleteCatalogItemMethod("1547"));
            //Console.WriteLine(RestClient.UpdateCatalogItemMethod("1542", "Catalog Item Example JSON/CatalogItemEpisodePUT.json"));

            //Console.WriteLine(RestClient.PostRelationshipMethod("Relationship Example JSON/RelationshipPost.json"));
        }
    }
}
