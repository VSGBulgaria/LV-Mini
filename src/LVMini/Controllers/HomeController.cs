using LVMini.Models;
using LVMini.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace LVMini.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;

        public HomeController(IHttpClientProvider httpClient)
        {
            _client = httpClient.Client();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoanPerformanceDataInquire()
        {
            var data = await _client.GetAsync("http://localhost:53920/api/widgets/loanperformance").Result.Content
                .ReadAsStringAsync();


            return Json(data);
        }

        public async Task<IActionResult> LoanBudgetVersusActualInquire()
        {
            var data = await _client.GetAsync("http://localhost:53920/api/widgets/budgetvsactual").Result.Content
                .ReadAsStringAsync();

            return Json(data);
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
