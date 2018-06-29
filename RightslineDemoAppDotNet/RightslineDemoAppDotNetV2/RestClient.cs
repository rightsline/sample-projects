﻿using Newtonsoft.Json.Linq;
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
        static string DemoTableItem = "table/292";
        static HttpClient client = new HttpClient();
        #region Catalog Item Example Methods
        /// <summary>
        /// Async GET call to Rightsline's V2 API that returns a json string of the catalog-item with a specified ID
        /// Uses Basic Auth Credentials from ConfigSetup class
        /// </summary>
        /// <param name="id"></param>"
        /// <returns></returns>
        public static string DemoCatalogItemFeature = "catalog-item/1565";
        public static string DemoCatalogItemEpisode = "catalog-item/1555";
        public static async Task<string> GetCatalogItemDemo(int id)        
        {            
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.GetStringAsync(BaseConnectionString + CatalogItem + id);
            var result = await stringTask;            
            return result;
        }        
        
        public static string CatalogItemEpisodePostExample = "Catalog Item Example JSON/CatalogItemEpisodePOST.json";
        public static string CatalogItemFeaturePostExample = "Catalog Item Example JSON/CatalogItemFeaturePOST.json";    
        /// <summary>
        /// Async POST call to Rightsline's V2 API that creates a new Catalog Item
        /// </summary>
        /// <returns>ID of created Item</returns>
        public static async Task<string> PostCatalogItemDemo()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var jsonObj = File.ReadAllText(CatalogItemFeaturePostExample);
            var postTask = client.PostAsync(BaseConnectionString + "catalog-item", new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        public static string CatalogItemEpisodePutExample = "Catalog Item Example JSON/CatalogItemEpisodePUT.json";
        public static string CatalogItemFeaturePutExample = "Catalog Item Example JSON/CatalogItemFeaturePUT.json";
        /// <summary>
        /// Async PUT call to Rightsline's V2 API that will update a Catalog-Item with a given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JSON object as string of the modified Catalog-Item</returns>
        public static async Task<string> PutCatalogItemDemo(int id)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);
            var jsonObj = File.ReadAllText(CatalogItemEpisodePutExample);
            var putTask = client.PutAsync(BaseConnectionString  + CatalogItem + id, new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await putTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Async DELETE call to Rightsline's V2 API that will delete a Catalog-Item with a given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<string> DeleteCatalogItemDemo(int id)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var deleteTask = client.DeleteAsync(BaseConnectionString + CatalogItem + id);
            var result = await deleteTask;
            return result.Content.ReadAsStringAsync().Result;
        }
        #endregion

        #region Table Example methods        
        public static string TablePostExample = "Table Example JSON/TablePostExample.json";
        public static string TablePutExample = "Table Example JSON/TablePutExample.json";        
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

            var jsonObj = File.ReadAllText(TablePostExample);
            
            var postTask = client.PostAsync(BaseConnectionString + "table", new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;            
            return result.Content.ReadAsStringAsync().Result;
        }
        public static async Task<string> PutTable()
        {
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var jsonObj = File.ReadAllText(TablePutExample);
            
            var putTask = client.PutAsync(BaseConnectionString + DemoTableItem, new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json"));
            var result = await putTask;            
            return result.Content.ReadAsStringAsync().Result;
        }
        
        public static async Task<string> DeleteTable()
        {
            client.DefaultRequestHeaders.Accept.Clear();            
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);            
            var stringTask = client.DeleteAsync(BaseConnectionString + DemoTableItem);
            var result = await stringTask;            
            return result.Content.ReadAsStringAsync().Result;
        }
        #endregion
        
    }    
}
