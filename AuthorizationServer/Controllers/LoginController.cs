using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthorizationServer.Controllers
{
    public class LoginController : Controller
    {
        //Validate Username
        public async Task<bool> CheckUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:53920/api/users/" + name).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}