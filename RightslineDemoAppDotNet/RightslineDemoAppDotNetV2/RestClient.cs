using Newtonsoft.Json.Linq;
using RightslineDemoAppDotNetV2.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RightslineDemoAppDotNetV2
{
    public class RestClient
    {
        public static string BaseConnectionString = "http://api-qa.rightsline.com/v2/";
        static HttpClient client = new HttpClient();
        #region Catalog Item Example Methods
        /// <summary>
        /// Async GET call to Rightsline's V2 API that returns a json string of the catalog-item with a specified ID
        /// Uses Basic Auth Credentials from ConfigSetup class
        /// </summary>
        /// <returns></returns>
        public static string DemoCatalogItemFeature = "catalog-item/1565";
        public static string DemoCatalogItemEpisode = "catalog-item/1576";
        public static async Task<string> GetCatalogItemDemoMethod()
        {            
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoCatalogItemEpisode);
            var result = await stringTask;            
            return result;
        }

        /// <summary>
        /// Asynce POST call to Rightsline's V2 API that creates a new Catalog Item
        /// </summary>
        /// <returns>ID of created Item</returns>
        public static string CatalogItemEpisodePostExample = "Catalog Item Example JSON/CatalogItemEpisodePOST.json";
        public static string CatalogItemFeaturePostExample = "Catalog Item Example JSON/CatalogItemFeaturePOST.json";
        public static async Task<string> PostCatalogItemDemoMethod()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var jsonObj = File.ReadAllText(CatalogItemFeaturePostExample);
            var postTask = client.PostAsync(BaseConnectionString + "catalog-item", new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        #endregion
    }    
}
