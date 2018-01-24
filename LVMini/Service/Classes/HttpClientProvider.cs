using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LVMini.Service.Classes
{
    public static class HttpClientProvider
    {
        private static readonly HttpContextAccessor HttpContextAccessor = new HttpContextAccessor();
        private static readonly HttpClient _httpClient = new HttpClient();

        public static HttpClient HttpClient => Client();

        static HttpClientProvider()
        { }

        private static HttpClient Client()
        {
            HttpContext context = HttpContextAccessor.HttpContext;
            string accessToken = context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;

            if (!string.IsNullOrWhiteSpace(accessToken))
                _httpClient.SetBearerToken(accessToken);

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }
    }
}
