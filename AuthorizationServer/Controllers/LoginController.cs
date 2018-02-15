using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Data.Service.Core.Interfaces;

namespace AuthorizationServer.Controllers
{
    public class LoginController : Controller
    {
        private IUserRepository _repository;

        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        //Validate Username
        public async Task<bool> CheckUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            var user = await _repository.GetByUsername(name);
            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}