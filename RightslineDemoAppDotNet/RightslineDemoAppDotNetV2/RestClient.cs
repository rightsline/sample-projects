using Newtonsoft.Json.Linq;
using RightslineDemoAppDotNetV2.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace RightslineDemoAppDotNetV2
{
    public class RestClient
    {
        public static string BaseConnectionString = "http://api-qa.rightsline.com/v2/";
        public static string DemoCatalogItem = "catalog-item/1552";
        public static string DemoTableItem = "table/292";

        static HttpClient client = new HttpClient();


        #region Catalog Item Example Methods
        /// <summary>
        /// Async GET call to Rightsline's V2 API that returns a json string of the catalog-item with a specified ID
        /// Uses Basic Auth Credentials from ConfigSetup class
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetCatalogItemDemoMethod()
        
        {            
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoCatalogItem);
            var result = await stringTask;            
            return result;
        }
        
        /// <summary>
        /// Async POST call to Rightsline's V2 API that creates a new Catalog Item
        /// </summary>
        /// <returns>ID of created Item</returns>
        public static async Task<string> PostCatalogItemDemoMethod()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var jsonObj = File.ReadAllText("CatalogItemPostExample.json");
            //jsonObj["title"] = "hvo-test-dotnet4";
            Console.WriteLine(jsonObj);
            var postTask = client.PostAsync(BaseConnectionString + "catalog-item", new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        #endregion

        #region Table Example methods
        public static async Task<string> GetTable()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoTableItem);
            var result = await stringTask;            
            return result;
        }
        public static async Task<string> PostTable()
        {
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);

            var jsonObj = File.ReadAllText("TablePostExample.json");
            
            var postTask = client.PostAsync(BaseConnectionString + "table", new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;            
            return result.Content.ReadAsStringAsync().Result;
        }
        public static async Task<string> PutTable()
        {
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var jsonObj = File.ReadAllText("TablePostExample.json");
            
            var postTask = client.PostAsync(BaseConnectionString + DemoTableItem, new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;            
            return result.Content.ReadAsStringAsync().Result;
        }
        
        public static async Task<string> DeleteTable()
        {
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoTableItem);
            var result = await stringTask;            
            return result;
        }
        #endregion
        
    }    
}
