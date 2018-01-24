using System.Net.Http;

namespace LVMini.Service.Interfaces
{
    public interface IHttpClientProvider
    {
        HttpClient Client();
    }
}
