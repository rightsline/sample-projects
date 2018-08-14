using Newtonsoft.Json.Linq;
using RightslineDemoAppDotNetV3.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using AWSSignatureV4_S3_Sample.Signers;
using AWSSignatureV4_S3_Sample.Util;
using System.Net;

namespace RightslineDemoAppDotNetV3
{
    public class RestClient
    {
        static string BaseConnectionString = "http://api-qa.rightsline.com/v3/";
        static string sessionToken = "auth/temporary-credentials";
        static string Region = "us-east-1";
        static HttpClient client = new HttpClient();        
        public static void ClearHeaders()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();            
        }

        public async static Task<string> GetSessionToken()
        {
            ClearHeaders();
            client.DefaultRequestHeaders.Add("x-api-key", ConfigSetup.ApiKey);
            JObject credentials = new JObject();
            credentials.Add("accessKey", ConfigSetup.AccessKey);
            credentials.Add("secretKey", ConfigSetup.SecretKey);            
            var postTask = client.PostAsync(BaseConnectionString + sessionToken,
                new StringContent(credentials.ToString(), Encoding.UTF8, "application/json"));
            var result = await postTask;
            return result.Content.ReadAsStringAsync().Result;
        }

        #region Catalog Item Example Methods
        public static string GetCatalogItemMethod(string itemId)
        {
            JObject credentials = JObject.Parse(GetSessionToken().Result);
            Console.WriteLine(credentials.GetValue("sessionToken")) ;
            
            client.DefaultRequestHeaders.Add("x-amz-security-token", credentials.GetValue("sessionToken").ToString());
            client.DefaultRequestHeaders.Add("x-api-key", ConfigSetup.ApiKey);

            // Construct a virtual hosted style address with the bucket name part of the host address,
            // placing the region into the url if we're not using us-east-1.
            var regionUrlPart = string.Format("-{0}", Region);
            //This is our dev url for the api, replace it 
            var endpointUri = "http://api-dev.rightsline.com/v3/catalog-item/" + itemId;
            var uri = new Uri(endpointUri);
            var headers = new Dictionary<string, string>()
            {
                {AWS4SignerBase.X_Amz_Content_SHA256, AWS4SignerBase.EMPTY_BODY_SHA256},
                {"content-type", "application/x-www-form-urlencoded"}
            };
            
            //Use the AWS4 signer to generate the signer and sign the request
            var signer = new AWS4SignerForAuthorizationHeader()
            {
                EndpointUri = uri,
                HttpMethod = "GET",
                Service = "execute-api",
                Region = Region
            };

            var authorization = signer.ComputeSignature(headers, "/", AWS4SignerBase.EMPTY_BODY_SHA256,
                credentials.GetValue("accessKey").ToString(), credentials.GetValue("secretKey").ToString());
            headers.Add("Authorization", authorization);

            string response = HttpHelpers.InvokeHttpRequest(uri, "GET", headers, null);
            //Console.WriteLine(response);
            return response;

        }
       
        #endregion

        #region Table Example methods

        #endregion

        #region Relationship Example Methods

        #endregion
    }
}