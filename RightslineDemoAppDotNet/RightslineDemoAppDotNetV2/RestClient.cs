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
        static string BaseConnectionString = "http://api-qa.rightsline.com/v2/";
        static string CatalogItem = "catalog-item/";
        static string DemoTableItem = "table/2249";
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
        /// <param name="catalogItemId"></param>"
        /// <returns></returns>
        public static string DemoCatalogItemFeature = "catalog-item/1565";
        public static string DemoCatalogItemEpisode = "catalog-item/1555";
        public static async Task<string> GetCatalogItemDemoMethod(int catalogItemId)
        {
            ClearHeadersAndAddAuthentication();
            var stringTask = client.GetStringAsync(BaseConnectionString + CatalogItem + catalogItemId);
            var result = await stringTask;
            return result;
        }       
        
        public static string CatalogItemEpisodePostExample = "Catalog Item Example JSON/CatalogItemEpisodePOST.json";
        public static string CatalogItemFeaturePostExample = "Catalog Item Example JSON/CatalogItemFeaturePOST.json";    
        /// <summary>
        /// Async POST call to Rightsline's V2 API that creates a new Catalog Item
        /// </summary>
        /// <returns>ID of created Item</returns>
        public static async Task<string> PostCatalogItemDemoMethod()
        {
            ClearHeadersAndAddAuthentication();
            var jsonObj = File.ReadAllText(CatalogItemFeaturePostExample);
            var postTask = client.PostAsync(BaseConnectionString + CatalogItem,
                new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        public static string CatalogItemEpisodePutExample = "Catalog Item Example JSON/CatalogItemEpisodePUT.json";
        public static string CatalogItemFeaturePutExample = "Catalog Item Example JSON/CatalogItemFeaturePUT.json";
        /// <summary>
        /// Async PUT call to Rightsline's V2 API that will update a Catalog-Item with a given ID
        /// IDs are passed in as a query string in the URL
        /// </summary>
        /// <param name="catalogItemId"></param>
        /// <returns>JSON object as string of the modified Catalog-Item</returns>
        public static async Task<string> PutCatalogItemDemo(int catalogItemId)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);
            var jsonObj = File.ReadAllText(CatalogItemEpisodePutExample);            
            var putTask = client.PutAsync(BaseConnectionString  + CatalogItem + catalogItemId, new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await putTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Async DELETE call to Rightsline's V2 API that will delete a Catalog-Item with a given ID
        /// IDs are passed in as a query string in the URL
        /// </summary>
        /// <param name="catalogItemId"></param>
        /// <returns></returns>
        public static async Task<string> DeleteCatalogItemDemo(int catalogItemId)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var deleteTask = client.DeleteAsync(BaseConnectionString + CatalogItem + catalogItemId);
            var result = await deleteTask;
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

        public static string RelationshipPostExample = "Relationship Example JSON/RelationshipPost.json";
        
        /// <summary>
        /// Async GET call to Rightsline's V2 API that will return details of a relationship
        /// IDs are passed in as a query string in the URL
        /// </summary>
        /// <param name="relationshipId"></param>
        /// <returns>JSON</returns>
        public static async Task<string> GetRelationship(int relationshipId)
        {
            ClearHeadersAndAddAuthentication();
            var getTask = client.GetAsync(BaseConnectionString + "relationship/"+relationshipId);
            var result = await getTask;
            return result.Content.ReadAsStringAsync().Result;
        }
        /// <summary>
        /// Async POST call to Rightsline's V2 API that will create a new relationship
        /// Relationships can be created between any two entities
        /// </summary>
        /// <returns>relationshipId</returns>
        public static async Task<string> PostRelationships()
        {
            ClearHeadersAndAddAuthentication();            
            var jsonObj = File.ReadAllText(RelationshipPostExample);

            var getTask = client.PostAsync(BaseConnectionString + "relationship", new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            
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

        /// <summary>
        /// Async DELETE call to Rightsline's V2 API that will delete a relationship between two items
        /// IDs are passed in as a query string in the URL
        /// </summary>
        /// <param name="relationshipId"></param>
        /// <returns>On success, returns ""</returns>
        public static async Task<string> DeleteRelationships(int relationshipId)
        {
            ClearHeadersAndAddAuthentication();
            var getTask = client.DeleteAsync(BaseConnectionString + "relationship/"+relationshipId);
            var result = await getTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        #endregion
        
        
    }
}