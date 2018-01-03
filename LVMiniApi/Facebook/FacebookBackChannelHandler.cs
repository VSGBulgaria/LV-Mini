using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LVMiniApi.Facebook
{
    class FacebookBackChannelHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
            {
                request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("?access token", "&access token"));
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
