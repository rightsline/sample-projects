using Newtonsoft.Json.Linq;
using RightslineDemoAppDotNetV3.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AWSSignatureV4_S3_Sample.Signers;
using AWSSignatureV4_S3_Sample.Util;
using System.Net;
using System.Security.Cryptography;

namespace RightslineDemoAppDotNetV3
{
    public class RestClient
    {
        static string BaseConnectionString = "http://api-dev.rightsline.com/v3/";
        static string sessionTokenUrl = "auth/temporary-credentials";
        static string Region = "us-east-1";
        static HttpClient client = new HttpClient();
        public static void ClearHeaders()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
        }
        /// <summary>
        /// POST call to generate temporary credentials according to Amazon's security protocols
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetSessionToken()
        {
            ClearHeaders();
            //x-api-key is included in the headers, access and secret keys are included in the body
            client.DefaultRequestHeaders.Add("x-api-key", ConfigSetup.ApiKey);
            JObject credentials = new JObject();
            credentials.Add("accessKey", ConfigSetup.AccessKey);
            credentials.Add("secretKey", ConfigSetup.SecretKey);
            var postTask = client.PostAsync(BaseConnectionString + sessionTokenUrl,
                new StringContent(credentials.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;
            //returns a json string consisting of temporary access key, secret key, date, and security token
            return result.Content.ReadAsStringAsync().Result;
        }
        public static string GenerateGetRequestMethod(string entityType, string entityId)
        {
            //Generates a JSON object from the session token JSON string 
            JObject credentials = JObject.Parse(GetSessionToken().Result);            
            
            var endpointUri = BaseConnectionString + entityType + "/" + entityId;
            var uri = new Uri(endpointUri);
            var headers = new Dictionary<string, string>()
            {                
                {"content-type", "application/x-www-form-urlencoded"},
                {"x-amz-security-token", credentials.GetValue("sessionToken").ToString() },
                {"x-api-key",  ConfigSetup.ApiKey}
            };

            //Use the AWS4 signer to generate the signer and sign the request
            var signer = new AWS4SignerForAuthorizationHeader()
            {
                EndpointUri = uri,
                HttpMethod = "GET",
                Service = "execute-api",
                Region = Region
            };
            //Uses the generated accessKey and secretKey from GenerateSessionToken(), NOT FROM CONFIG
            var authorization = signer.ComputeSignature(headers, "", AWS4SignerBase.EMPTY_BODY_SHA256,
                credentials.GetValue("accessKey").ToString(), credentials.GetValue("secretKey").ToString());
            Console.WriteLine(authorization);
            //Add the signed authorization to headers
            headers.Add("Authorization", authorization);
            string response = HttpHelpers.InvokeHttpRequest(uri, "GET", headers, null);
            //Console.WriteLine(response);
            return response;
        }
        public static string GeneratePostRequestMethod(string entityType, string jsonFilePath)
        {
            //Generates a JSON object from the session token JSON string 
            JObject credentials = JObject.Parse(GetSessionToken().Result);
            
            //Get the JSON for the entity being created and hash it for the POST request
            var jsonObject = File.ReadAllText(jsonFilePath);
            var encodedJsonBytes = Encoding.UTF8.GetBytes(jsonObject);
            var hashedJsonString = ComputeSHA256Hash(encodedJsonBytes);            
            
            var endpointUri = BaseConnectionString + entityType;
            var uri = new Uri(endpointUri);
            var headers = new Dictionary<string, string>()
            {                
                {"content-type", "application/json"},
                {"x-amz-security-token", credentials.GetValue("sessionToken").ToString() },
                {"x-api-key",  ConfigSetup.ApiKey}
            };

            //Use the AWS4 signer to generate the signer and sign the request
            var signer = new AWS4SignerForAuthorizationHeader()
            {
                EndpointUri = uri,
                HttpMethod = "POST",
                Service = "execute-api",
                Region = Region
            };
            //Uses the generated accessKey and secretKey from GenerateSessionToken(), NOT FROM CONFIG
            var authorization = signer.ComputeSignature(headers, "", hashedJsonString,
                credentials.GetValue("accessKey").ToString(), credentials.GetValue("secretKey").ToString());
            //Add the signed authorization to headers
            headers.Add("Authorization", authorization);
            string response = HttpHelpers.InvokeHttpRequest(uri, "POST", headers, jsonObject);
            //Console.WriteLine(response);
            return response;
        }
        public static string ComputeSHA256Hash(byte[] toBeEncoded)
        {
            var hasher = SHA256.Create();
            var hashedBytes = hasher.ComputeHash(toBeEncoded);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
        public static string GenerateDeleteRequestMethod(string entityType, string entityId)
        {
            //Generates a JSON object from the session token JSON string 
            JObject credentials = JObject.Parse(GetSessionToken().Result);
            
            var endpointUri = BaseConnectionString + entityType + "/" + entityId;
            var uri = new Uri(endpointUri);
            var headers = new Dictionary<string, string>()
            {                
                {"content-type", "application/x-www-form-urlencoded"},
                {"x-amz-security-token", credentials.GetValue("sessionToken").ToString() },
                {"x-api-key",  ConfigSetup.ApiKey}
            };

            //Use the AWS4 signer to generate the signer and sign the request
            var signer = new AWS4SignerForAuthorizationHeader()
            {
                EndpointUri = uri,
                HttpMethod = "DELETE",
                Service = "execute-api",
                Region = Region
            };
            //Uses the generated accessKey and secretKey from GenerateSessionToken(), NOT FROM CONFIG
            var authorization = signer.ComputeSignature(headers, "", AWS4SignerBase.EMPTY_BODY_SHA256,
                credentials.GetValue("accessKey").ToString(), credentials.GetValue("secretKey").ToString());
            //Add the signed authorization to headers
            headers.Add("Authorization", authorization);
            string response = HttpHelpers.InvokeHttpRequest(uri, "DELETE", headers, null);
            //Console.WriteLine(response);
            return response;
        }
        public static string GeneratePutRequestMethod(string entityType, string entityId, string jsonFilePath)
        {
            //Generates a JSON object from the session token JSON string 
            JObject credentials = JObject.Parse(GetSessionToken().Result);
            
            //Get the JSON for the entity being updated and hash it for the PUT request
            var jsonObject = File.ReadAllText(jsonFilePath);
            var encodedJsonBytes = Encoding.UTF8.GetBytes(jsonObject);
            var hashedJsonString = ComputeSHA256Hash(encodedJsonBytes);

            
            var endpointUri = BaseConnectionString + entityType + "/" + entityId;
            var uri = new Uri(endpointUri);
            var headers = new Dictionary<string, string>()
            {
                {"content-type", "application/json"},
                {"x-amz-security-token", credentials.GetValue("sessionToken").ToString() },
                {"x-api-key",  ConfigSetup.ApiKey}
            };

            //Use the AWS4 signer to generate the signer and sign the request
            var signer = new AWS4SignerForAuthorizationHeader()
            {
                EndpointUri = uri,
                HttpMethod = "PUT",
                Service = "execute-api",
                Region = Region
            };
            //Uses the generated accessKey and secretKey from GenerateSessionToken(), NOT FROM CONFIG
            var authorization = signer.ComputeSignature(headers, "", hashedJsonString,
                credentials.GetValue("accessKey").ToString(), credentials.GetValue("secretKey").ToString());
            //Add the signed authorization to headers
            headers.Add("Authorization", authorization);
            string response = HttpHelpers.InvokeHttpRequest(uri, "PUT", headers, jsonObject);
            //Console.WriteLine(response);
            return response;
        }
        #region Catalog Item Example Methods      
        /// <summary>
        /// Calls a helper method to build a GET request for a catalog-item with a specified ID
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Returns a JSON string of the catalog-item's properties</returns>
        public static string GetCatalogItemMethod(string entityId)
        {
            return GenerateGetRequestMethod("catalog-item", entityId);
        }
        public static string PostCatalogItemMethod(string filePath)
        {
            return GeneratePostRequestMethod("catalog-item", filePath);
        }
        public static string DeleteCatalogItemMethod(string entityId)
        {
            return GenerateDeleteRequestMethod("catalog-item", entityId);
        }
        public static string UpdateCatalogItemMethod(string entityId, string filePath)
        {
            return GeneratePutRequestMethod("catalog-item", entityId, filePath);
        }
        #endregion

        #region Table Example methods
        /// <summary>
        /// Calls a helper method to build a GET request for a table with a specified ID
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns>Returns a JSON string of the table</returns>
        public static string GetTableMethod(string tableId)
        {
            return GenerateGetRequestMethod("table", tableId);
        }
        public static string PostTableMethod(string filePath)
        {
            return GeneratePostRequestMethod("table", filePath);
        }
        public static string DeleteTableMethod(string entityId)
        {
            return GenerateDeleteRequestMethod("table", entityId);
        }
        public static string UpdateTableMethod(string entityId, string filePath)
        {
            return GeneratePutRequestMethod("table", entityId, filePath);
        }
        #endregion

        #region Relationship Example Methods

        public static string GetRelationshipMethod(string relationshipId)
        {
            return GenerateGetRequestMethod("relationship", relationshipId);
        }
        public static string PostRelationshipMethod(string filePath)
        {
            return GeneratePostRequestMethod("relationship", filePath);
        }
        public static string DeleteRelationshipMethod(string entityId)
        {
            return GenerateDeleteRequestMethod("relationship", entityId);
        }
        public static string UpdateRelationshipMethod(string entityId, string filePath)
        {
            return GeneratePutRequestMethod("relationship", entityId, filePath);
        }
        #endregion
    }
}