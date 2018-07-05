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

namespace RightslineDemoAppDotNetV3
{
    public class RestClient
    {
        static string BaseConnectionString = "http://api-qa.rightsline.com/v2/";
//        static string CatalogItem = "catalog-item/";
//        static string DemoTableItem = "table/2249";
        static HttpClient client = new HttpClient();

        public static void ClearHeadersAndAddAuthentication()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", ConfigSetup.BasicAuthCredentials);
        }

        #region Catalog Item Example Methods

       
        #endregion

        #region Table Example methods

        #endregion

        #region Relationship Example Methods

        #endregion
    }
}