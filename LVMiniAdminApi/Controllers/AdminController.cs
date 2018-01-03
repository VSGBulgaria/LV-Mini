using Data.Service.Core;
using Data.Service.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IUnitOfWork _unitOfWork;


        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Users;
        }

        // GET: api/Admin
        [HttpGet]
        public IActionResult Get()
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
            await _unitOfWork.Commit(); // Добавил съм Commit.

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
                _repository.Update(user);
                await _unitOfWork.Commit(); // Добавил съм Commit.

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
            _repository.Delete(resultCollectionUsers[FirstUserIndex]);
            await _unitOfWork.Commit(); // Добавил съм Commit.
            return Ok();

        }
    }
}
