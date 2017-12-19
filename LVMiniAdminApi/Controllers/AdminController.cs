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
        private const int DefaultUserPostId = 0;
        private const int FirstUserIndex = 0;
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
            return Ok(user);
        }

        // PUT: api/Admin
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The model is invalid");
            }
            var usersMatched = _repository
                .GetAll(innerUser => user.Email == innerUser.Email || user.Username == innerUser.Username).ToList();
            if (usersMatched.Count < SingleUserCollectionCount)
            {
                user.Id = DefaultUserPostId;
                var postNewUser = await Post(user);
                return Created("api/admin", postNewUser);
            }
            if (usersMatched.Count == SingleUserCollectionCount && usersMatched[FirstUserIndex].Id == user.Id)
            {
                var userFromDb = usersMatched[FirstUserIndex];
                user.Id = userFromDb.Id;
                await _repository.Update(user);
                return Ok("User was modified");
            }
            return BadRequest("More than one user matched to given credentials.");
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
            await _repository.Delete(resultCollectionUsers[FirstUserIndex].Id);
            return Ok();
            
        }
    }
}
