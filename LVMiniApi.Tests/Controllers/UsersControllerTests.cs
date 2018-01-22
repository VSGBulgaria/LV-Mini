using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LVMiniApi.UnitTests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        [Test]
        public async Task Get_GetRequestWithoutParameter_ReturnsHttpOK()
        {
            // Arrange
            using (var client = new HttpClient())
            {
                // Act
                var users = await client.GetAsync("http://localhost:53920/api/users");

                // Assert
                Assert.That(users.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public async Task Get_GetRequestWithValidUsername_ReturnsHttpOK()
        {
            using (var client = new HttpClient())
            {
                var user = await client.GetAsync("http://localhost:53920/api/users/simo");

                Assert.That(user.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public async Task Get_GetRequestWithInvalidUsername_ReturnsHttpNotFound()
        {
            using (var client = new HttpClient())
            {
                var user = await client.GetAsync("http://localhost:53920/api/users/dwasdswadfaws");

                Assert.That(user.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }
    }
}
