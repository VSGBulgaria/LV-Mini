using System.Net;
using System.Net.Http;
using Data.Service.Core.Entities;
using Data.Service.Persistance;
using Data.Service.Persistance.Repositories;
using LVMiniApi.Controllers;
using LVMiniApiTests.Mocking;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace LVMiniApiTests
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void Registration_InvalidModelState_ReturnsBadRequest()
        {
            var controller = new UserController(new UserRepository(new LvMiniDbContext(new DbContextOptions<LvMiniDbContext>())), new PasswordHasher<IUser>());
            User user = new User()
            {
                Username = "InvalidUser",
                Password = "3124214"
            };

            var a = controller.Register(user);

            Assert.That(a.Result, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
