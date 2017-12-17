using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniApi.Controllers
{
    using Data.Service.Entities;
    using Data.Service.Repositories.UserRepository;
    using MappedModels;

    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<JsonResult> Get(int id)
        {
            var userById =await this._userRepository.GetById(id);
            if (userById != null)
            {
               return Json(AutoMapper.Mapper.Instance.Map<User,BasicUser>(userById)); 
            }
            return Json("There is no such User");
        }
        
        // POST: api/Users
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
