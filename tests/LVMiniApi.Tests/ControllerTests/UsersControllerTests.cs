using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniApi.Controllers;
using LVMiniApi.Models;
using LVMiniApi.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace LVMiniApi.Tests.ControllerTests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private IMapper _mapper;
        private Mock<ITypeHelperService> _typeHelperService;

        [SetUp]
        public void SetUp()
        {
            _uowMock = new Mock<IUnitOfWork>();
            MapperConfiguration configuration = new MapperConfiguration(a => a.CreateMap<User, UserDto>());
            _mapper = new Mapper(configuration);
            _typeHelperService = new Mock<ITypeHelperService>();
        }


        [Test]
        public void GetUserByUsername_WhenGivenExistingUser_ReturnsOkAndTheUser()
        {
            _uowMock.Setup(uow => uow.UserRepository.GetByUsername("simo"))
                .Returns(Task.FromResult(new User { Username = "simo" }));
            _typeHelperService.Setup(t => t.TypeHasProperties<UserDto>(null)).Returns(true);
            var controller = new UsersController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result = controller.GetUserByUsername("simo", null).Result as OkObjectResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(200);

            var value = result.Value as ExpandoObject;
            var content = value as IDictionary<string, object>;
            value.ShouldNotBe(null);
            object username = "simo";
            content.TryGetValue("Username", out username).ShouldBe(true);
        }

        [Test]
        public void GetUserByUsername_WhenGivenNonExistingUser_ReturnsNotFound()
        {
            _uowMock.Setup(uow => uow.UserRepository.GetByUsername("simo"))
                .Returns(Task.FromResult(new User { Username = "simo" }));
            _typeHelperService.Setup(t => t.TypeHasProperties<UserDto>(null)).Returns(true);
            var controller = new UsersController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result = controller.GetUserByUsername("nonExistingUser", null).Result as NotFoundResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(404);
        }
    }
}
