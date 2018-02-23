using Microsoft.AspNetCore.Mvc;

namespace LVMini.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}