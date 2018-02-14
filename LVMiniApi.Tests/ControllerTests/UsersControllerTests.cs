using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniApi.Controllers;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;
using System.Threading.Tasks;

namespace LVMiniApi.Tests.ControllerTests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _uowMock = new Mock<IUnitOfWork>();
            MapperConfiguration configuration = new MapperConfiguration(a => a.CreateMap<User, UserDto>());
            _mapper = new Mapper(configuration);
        }


        [Test]
        public void GetUserByUsername_WhenGivenExistingUser_ReturnsOkAndTheUser()
        {
            _uowMock.Setup(uow => uow.UserRepository.GetByUsername("simo"))
                .Returns(Task.FromResult(new User { Username = "simo" }));
            var controller = new UsersController(_uowMock.Object, _mapper);

            var result = controller.GetUserByUsername("simo").Result as OkObjectResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(200);

            var value = result.Value as UserDto;
            value.ShouldNotBe(null);
            value.Username.ShouldBeSameAs("simo");
        }

        [Test]
        public void GetUserByUsername_WhenGivenNonExistingUser_ReturnsNotFound()
        {
            _uowMock.Setup(uow => uow.UserRepository.GetByUsername("simo"))
                .Returns(Task.FromResult(new User { Username = "simo" }));
            var controller = new UsersController(_uowMock.Object, _mapper);

            var result = controller.GetUserByUsername("nonExistingUser").Result as NotFoundResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(404);
        }
    }
}
