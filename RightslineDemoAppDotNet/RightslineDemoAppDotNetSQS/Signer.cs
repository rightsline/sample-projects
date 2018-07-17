
using RightslineDemoAppDotNetSQS.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RightslineDemoAppDotNetSQS
{
    class Signer
    {
        private const string algorithm = "AWS4-HMAC-SHA256";
        private static SHA256 sha256;
        public Signer()
        {
            sha256 = SHA256.Create();
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
        private string SHA256Hash(string targetString)
        {
            var sha256Hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(targetString));
            return ToHexString(sha256Hash);
        }

        private static byte[] HMACSHA256Hash(byte[] key, string targetString)
        {
            var hmacHash = new HMACSHA256(key);
            hmacHash.Initialize();
            return hmacHash.ComputeHash(Encoding.UTF8.GetBytes(targetString));
        }

        private static byte[] CreateSignatueKey(string key, string dateStamp, string serviceName)
        {
            
            byte[] kSecret = Encoding.UTF8.GetBytes("AWS4" + key);
            byte[] kDate = HMACSHA256Hash(kSecret, dateStamp);
            byte[] kRegion = HMACSHA256Hash(kDate, ConfigSetup.Region);
            byte[] kService = HMACSHA256Hash(kRegion, serviceName);
            byte[] kSigning = HMACSHA256Hash(kService, "aws4_request");
            return kSigning;
        }
        public async Task<HttpRequestMessage> Sign(HttpRequestMessage request, string service )
        {
            var t = DateTimeOffset.UtcNow;
            var amzdate = t.ToString("yyyyMMddTHHmmssZ");
            request.Headers.Add("x-amz-date", amzdate);
            var datestamp = t.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            var canonical_request = new StringBuilder();
            canonical_request.Append(request.Method + "\n");
            canonical_request.Append(request.RequestUri.AbsolutePath + "\n");

            var canonicalQueryParams = GetCanonicalQueryParams(request);

            canonical_request.Append(canonicalQueryParams + "\n");

            var signedHeadersList = new List<string>();

            foreach (var header in request.Headers.OrderBy(x => x.Key.ToLower()))
            {
                canonical_request.Append(header.Key.ToLower());
                canonical_request.Append(":");
                canonical_request.Append(string.Join(",", header.Value.Select(s => s.Trim())));
                canonical_request.Append("\n");
                signedHeadersList.Add(header.Key.ToLower());
            }

            canonical_request.Append("\n");

            var signed_headers = string.Join(";", signedHeadersList);

            canonical_request.Append(signed_headers + "\n");

            var content = "";
            if (request.Content != null)
            {
                content = await request.Content.ReadAsStringAsync();
            }
            var payload_hash = SHA256Hash(content);
            canonical_request.Append(payload_hash);
            var credential_scope = $"{datestamp}/{ConfigSetup.QueueName}/{service}/aws4_request";
            var string_to_sign = $"{algorithm}\n{amzdate}\n{credential_scope}\n" + SHA256Hash(canonical_request.ToString());
            var signing_key = CreateSignatueKey( ConfigSetup.SecretKey, datestamp, service);
            var signature = ToHexString(HMACSHA256Hash(signing_key, string_to_sign));

            request.Headers.TryAddWithoutValidation("Authorization", $"{algorithm} Credential={ConfigSetup.AccessKey}/{credential_scope}, SignedHeaders={signed_headers}, Signature={signature}");

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
                
    }
}
