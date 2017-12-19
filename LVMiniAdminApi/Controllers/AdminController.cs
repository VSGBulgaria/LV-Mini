using System.Linq;
using System.Threading.Tasks;
using Data.Service.Core;
using Data.Service.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniAdminApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        private const int SingleUserCollectionCount = 1;
        private readonly IUserRepository _repository;

        public AdminController(IUserRepository userRepo)
        {
            _repository = userRepo;
        }

        // GET: api/Admin
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_repository.GetAll());
        }

        // POST: api/Admin
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid User form in post method.");
            }
            if (_repository.GetAll(innerUser => innerUser.Username == user.Username || innerUser.Email == user.Email)
                    .ToList().Count >= SingleUserCollectionCount)
            {
                return BadRequest("Username or Email already exists.");
            }
            await _repository.Insert(user);
            return Ok();
        }

        // PUT: api/Admin
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]User user)
        {
            var debug = "";
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid user form.");
            }

            var matchedUsers = _repository.GetAll(innerUser => innerUser.Username == user.Username || innerUser.Email == user.Email).ToList();
            if (matchedUsers.Count < SingleUserCollectionCount)
            {
                return await Post(user);
            }
            if (matchedUsers.Count > SingleUserCollectionCount)
            {
                return BadRequest("Username or email result list is bigger than a single record.");
            }
            await _repository.Delete(matchedUsers[0].Id);
            await _repository.Update(user);
            return Ok(user);
        }

        // DELETE: api/Admin
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid user form.");
            }
            var resultCollectionUsers = _repository.GetAll(innerUser => innerUser.Username == user.Username || innerUser.Email == user.Email).ToList();
            if (resultCollectionUsers.Count() != SingleUserCollectionCount)
            {
                return BadRequest("Invalid count of users");
            }
            await _repository.Delete(resultCollectionUsers[0].Id);
            return Ok();
        }
    }
}
