using RightslineDemoAppDotNetV2.Config;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RightslineDemoAppDotNetV2
{
    public class RestClient
    {
        public static string BaseConnectionString = "http://api-qa.rightsline.com/v2/";
        public static string DemoCatalogItem = "catalog-item/1401";
        /// <summary>
        /// Async GET call to Rightsline's V2 API that returns a json string of the catalog-item with the id 1401
        /// Uses Basic Auth Credentials from ConfigSetup class
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetCatalogItemDemoMethod()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoCatalogItem);
            var result = await stringTask;            
            return result;
        }
    }    
}
