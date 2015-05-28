using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TokenAuth.Api.Services
{
    public static class FacebookService
    {
        private const string AccessTokenUri = "https://graph.facebook.com/oauth/access_token";
        private const string GraphApiUri = "https://graph.facebook.com/me";

        public static async Task<string> GetProfile(string accessToken)
        {
            string requestUri = string.Format("{0}?access_token={1}", GraphApiUri, accessToken);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                // Haven't tested this
                Dictionary<string, object> responseData = await response.Content.ReadAsAsync<Dictionary<string, object>>();
                string id = HttpUtility.UrlDecode((string)responseData["id"]);

                return id;
            }
        }

        public static async Task<string> GetAccessToken(string clientId, string redirectUri, string code)
        {
            string accessToken = null;

            Dictionary<string, object> requestParams = new Dictionary<string, object>
            {
                { "client_id", clientId },
                { "redirect_uri", redirectUri },
                { "client_secret", "MyS3cr#tFac3bookK3y!" },
                { "code", code }
            };

            string requestUri = AccessTokenUri + "?";

            foreach (KeyValuePair<string, object> param in requestParams)
            {
                requestUri += string.Format("{0}={1}&", param.Key, param.Value);
            }

            requestUri = requestUri.Substring(0, requestUri.Length - 1);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                // Haven't tested this
                Dictionary<string, object> responseData = await response.Content.ReadAsAsync<Dictionary<string, object>>();
                accessToken = HttpUtility.UrlDecode((string)responseData["accessToken"]);
            }

            return accessToken;
        }
    }
}
