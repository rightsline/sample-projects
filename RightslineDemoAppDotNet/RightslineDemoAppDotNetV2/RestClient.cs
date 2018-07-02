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
        public static string DemoTableItem = "table/2249";
        static HttpClient client = new HttpClient();

        public static void ClearHeadersAndAddAuthentication()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);
        }

        #region Catalog Item Example Methods

        /// <summary>
        /// Async GET call to Rightsline's V2 API that returns a json string of the catalog-item with a specified ID
        /// Uses Basic Auth Credentials from ConfigSetup class
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetCatalogItemDemoMethod()

        {
            ClearHeadersAndAddAuthentication();
            var stringTask = client.GetStringAsync(BaseConnectionString + DemoCatalogItem);
            var result = await stringTask;
            return result;
        }

        /// <summary>
        /// Async POST call to Rightsline's V2 API that creates a new Catalog Item
        /// </summary>
        /// <returns>ID of created Item</returns>
        public static string CatalogItemEpisodePostExample = "Catalog Item Example JSON/CatalogItemEpisodePOST.json";

        private const string CatalogItemFeaturePostExample = "Catalog Item Example JSON/CatalogItemFeaturePOST.json";

        public static async Task<string> PostCatalogItemDemoMethod()
        {
            ClearHeadersAndAddAuthentication();
            var jsonObj = File.ReadAllText(CatalogItemFeaturePostExample);
            var postTask = client.PostAsync(BaseConnectionString + "catalog-item",
                new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        #endregion

        #region Table Example methods

        private const string TablePostExample = "Table Example JSON/TablePostExample.json";
        private const string TablePutExample = "Table Example JSON/TablePutExample.json";

        public static async Task<string> GetTable(int tableId)
        {
            ClearHeadersAndAddAuthentication();

            var stringTask = client.GetStringAsync(BaseConnectionString + "table/" + tableId);
            var result = await stringTask;
            return result;
        }

        /// <summary>
        /// Async POST call to Rightsline's V2 API that creates a new Table
        /// Find the JSON submitted for this call under /Table Example JSON
        /// Final list of all possible tables that can be posted can be found under /Table Commented Example JSON
        /// </summary>
        /// <returns>ID of created Table</returns>
        public static async Task<string> PostTable()
        {
            ClearHeadersAndAddAuthentication();

            var jsonObj = File.ReadAllText(TablePostExample);

            var postTask = client.PostAsync(BaseConnectionString + "table",
                new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Async PUT call to Rightsline's V2 API that creates a new Catalog Item
        /// PUT calls should not be sent over with relationship information 
        /// </summary>
        /// <returns>ID of created Item</returns>
        public static async Task<string> PutTable(int tableId)
        {
            ClearHeadersAndAddAuthentication();

            var jsonObj = File.ReadAllText(TablePutExample);

            var putTask = client.PutAsync(BaseConnectionString + "table/" + tableId,
                new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await putTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Async DELETE call to Rightsline's V2 API that creates a new Table
        /// 
        /// </summary>
        /// <returns>ID of created Table</returns>
        public static async Task<string> DeleteTable(int tableId)
        {
            ClearHeadersAndAddAuthentication();
            var deleteTask = client.DeleteAsync(BaseConnectionString + "table/" + tableId);
            var result = await deleteTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        #endregion

        #region Relationship Example Methods

        public static async Task<string> GetRelationships()
        {
            ClearHeadersAndAddAuthentication();
            var getTask = client.GetAsync(BaseConnectionString + "relationship");
            var result = await getTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        public static async Task<string> PostRelationships()
        {
            ClearHeadersAndAddAuthentication();
            var getTask = client.GetAsync(BaseConnectionString + "relationship");
            var result = await getTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        public static async Task<string> PutRelationships()
        {
            // PUT IS NOT SUPPORTED
            throw new NotImplementedException();
            // NOPE
            // DONT EVEN THINK ABOUT IT
        }

        public static async Task<string> DeleteRelationships()
        {
            ClearHeadersAndAddAuthentication();
            var getTask = client.DeleteAsync(BaseConnectionString + "relationship");
            var result = await getTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        #endregion
    }
}