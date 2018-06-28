using RightslineDemoAppDotNetV2.Config;
using System;
using System.Collections.Generic;
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
        public static HttpClient client = new HttpClient();
        public static string BaseConnectionString = "http://api-qa.rightsline.com/v2/";
        public static string DemoCatalogItem = "catalog-item/1401";
        public static string DemoTableItem = "table/292";
        
       

        /// <summary>
        /// Async GET call to Rightsline's V2 API that returns a json string of the catalog-item with the id 1401
        /// Uses Basic Auth Credentials from ConfigSetup class
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetCatalogItemDemoMethod()
        {
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoCatalogItem);
            var result = await stringTask;            
            return result;
        }

        public static async Task<string> GetTable()
        {
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

            var table = new JObject();
            
            
//            var content = new StringContent();
            var stringTask = client.PostAsync(BaseConnectionString + DemoTableItem, content);
            var result = await stringTask;            
            return result;
        }
        public static async Task<string> PutTable()
        {
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoTableItem);
            var result = await stringTask;            
            return result;
        }
        
        public static async Task<string> DeleteTable()
        {
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoTableItem);
            var result = await stringTask;            
            return result;
        }
    }    
}
