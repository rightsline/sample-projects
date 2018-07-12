using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace RightslineDemoAppDotNetSQS.Config
{
    public class ConfigSetup
    {
        private static string AccountId { get; set; }
        public static string QueueName { get; set; }
        public static string AccessKey { get; set; }
        public static string SecretKey { get; set; }
        public static string Region = "us-west-2";
        private static SHA256 _sha256;
        private const string Algorithm = "AWS4-HMAC-SHA256";

        public static void GetConfigFile()
        {
            var folderPath =
                AppContext.BaseDirectory.Substring(0,
                    AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
            var filePath = Path.Combine(folderPath, "Config\\config.json");
            try
            {
                var credentials = JObject.Parse(File.ReadAllText(filePath));
                AccountId = credentials.GetValue("AccountId").ToString();
                QueueName = credentials.GetValue("QueueName").ToString();
                AccessKey = credentials.GetValue("AccessKey").ToString();
                SecretKey = credentials.GetValue("SecretKey").ToString();
                _sha256 = SHA256.Create();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Please ensure that you have a valid config.json file in the " + folderPath +
                                  "Config folder.");
            }
        }

        private string Hash(string stringToHash)
        {
            var result = _sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            return ToHexString(result);
        }

        private static byte[] HmacSHA256(byte[] key, string data)
        {
            var hashAlgorithm = new HMACSHA256(key);

            return hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private static byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes("AWS4" + key);
            byte[] kDate = HmacSHA256(kSecret, dateStamp);
            byte[] kRegion = HmacSHA256(kDate, regionName);
            byte[] kService = HmacSHA256(kRegion, serviceName);
            byte[] kSigning = HmacSHA256(kService, "aws4_request");
            return kSigning;
        }

        private static string ToHexString(byte[] array)
        {
            var hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        public async Task<HttpRequestMessage> Sign(HttpRequestMessage request, string service)
        {
            GetConfigFile();
            if (string.IsNullOrEmpty(service))
            {
                throw new ArgumentOutOfRangeException(nameof(service), service, "Not a valid service.");
            }

            if (string.IsNullOrEmpty(Region))
            {
                throw new ArgumentOutOfRangeException(nameof(Region), Region, "Not a valid Region.");
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Headers.Host == null)
            {
                request.Headers.Host = request.RequestUri.Host;
            }

            var t = DateTimeOffset.UtcNow;
            var amzdate = t.ToString("yyyyMMddTHHmmssZ");
            request.Headers.Add("x-amz-date", amzdate);
            var datestamp = t.ToString("yyyyMMdd");

            var canonicalRequest = new StringBuilder();
            canonicalRequest.Append(request.Method + "\n");
            canonicalRequest.Append(request.RequestUri.AbsolutePath + "\n");

            var canonicalQueryParams = GetCanonicalQueryParams(request);

            canonicalRequest.Append(canonicalQueryParams + "\n");

            var signedHeadersList = new List<string>();

            foreach (var header in request.Headers.OrderBy(a => a.Key.ToLowerInvariant()))
            {
                canonicalRequest.Append(header.Key.ToLowerInvariant());
                canonicalRequest.Append(":");
                canonicalRequest.Append(string.Join(",", header.Value.Select(s => s.Trim())));
                canonicalRequest.Append("\n");
                signedHeadersList.Add(header.Key.ToLowerInvariant());
            }

            canonicalRequest.Append("\n");

            var signedHeaders = string.Join(";", signedHeadersList);

            canonicalRequest.Append(signedHeaders + "\n");

            var content = "";
            if (request.Content != null)
            {
                content = await request.Content.ReadAsStringAsync();
            }

            var payloadHash = Hash(content);

            canonicalRequest.Append(payloadHash);

            var credentialScope = $"{datestamp}/{Region}/{service}/aws4_request";

            var stringToSign = $"{Algorithm}\n{amzdate}\n{credentialScope}\n" + Hash(canonicalRequest.ToString());

            var signingKey = GetSignatureKey(SecretKey, datestamp, Region, service);
            var signature = ToHexString(HmacSHA256(signingKey, stringToSign));

            request.Headers.TryAddWithoutValidation("Authorization",
                $"{Algorithm} Credential={AccessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}");

            return request;
        }

        private static string GetCanonicalQueryParams(HttpRequestMessage request)
        {
            var querystring = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var keys = querystring.AllKeys.OrderBy(a => a).ToArray();
            var queryParams = keys.Select(key => $"{key}={querystring[key]}");
            var canonicalQueryParams = string.Join("&", queryParams);
            return canonicalQueryParams;
        }

//        public static void EncryptKey()
//        {
//        }
//
//        private static string Encode()
//        {
//        }
    }
}