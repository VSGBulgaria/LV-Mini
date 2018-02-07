using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LVMini.Service.Classes
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent content)
        {
            if (requestUri == null || content == null)
            {
                throw new ArgumentNullException();
            }

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };

            return client.SendAsync(request);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            if (requestUri == null || content == null)
            {
                throw new ArgumentNullException();
            }

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };

            return client.SendAsync(request);
        }
    }
}
