using LVMini.Service.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace LVMini.Controllers
{
    public class BaseController : Controller
    {
        protected readonly HttpClient Client = HttpClientProvider.HttpClient;
    }
}