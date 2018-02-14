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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVMiniApi.Tests.ControllerTests
{
    [TestFixture]
    public class ProductGroupsControllerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _uowMock = new Mock<IUnitOfWork>();
        }

        [Test]
        public void GetAllProductGroups_WhenCalled_ReturnOkAndACollection()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(a => a.CreateMap<ProductGroup, ProductGroupDto>());
            _mapper = new Mapper(configuration);
            _uowMock.Setup(uow => uow.ProductGroupRepository.GetAll(null))
                .Returns(new List<ProductGroup> { new ProductGroup { Name = "ProductGroupTest" } });
            var controller = new ProductGroupsController(_uowMock.Object, _mapper);

            var result = controller.GetAllProductGroups() as OkObjectResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(200);

            var content = result.Value as List<ProductGroupDto>;
            content.ShouldNotBe(null);
            content.ShouldBeOfType(typeof(List<ProductGroupDto>));
            content.Count.ShouldBe(1);
        }

        [Test]
        public void GetSingleProductGroup_UnexistingProductGroup_ReturnsNotFound()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(a => a.CreateMap<ProductGroup, ProductGroupDto>());
            _mapper = new Mapper(configuration);
            _uowMock.Setup(uow => uow.ProductGroupRepository.GetProductGroupByName("ProductGroupOne"))
                .Returns(Task.FromResult(new ProductGroup { Name = "ProductGroupOne" }));
            var controller = new ProductGroupsController(_uowMock.Object, _mapper);

            var result = controller.GetSingleProductGroup("NonExistingGroup").Result as NotFoundResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(404);
        }

        [Test]
        public void GetSingleProductGroup_ExistingProductGroup_ReturnsOkAndProductGroup()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(a => a.CreateMap<ProductGroup, ProductGroupDto>());
            _mapper = new Mapper(configuration);
            _uowMock.Setup(uow => uow.ProductGroupRepository.GetProductGroupByName("ProductGroupOne"))
                .Returns(Task.FromResult(new ProductGroup { Name = "ProductGroupOne" }));
            var controller = new ProductGroupsController(_uowMock.Object, _mapper);

            var result = controller.GetSingleProductGroup("ProductGroupOne").Result as OkObjectResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(200);


            var content = result.Value as ProductGroupDto;
            content.ShouldNotBe(null);
            content.Name.ShouldBe("ProductGroupOne");
        }

        [Test]
        public void AddProductGroup_AlreadyExistingProductGroup_ReturnsBadRequest()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(a => a.CreateMap<ProductGroup, ProductGroupDto>());
            _mapper = new Mapper(configuration);
            _uowMock.Setup(uow => uow.ProductGroupRepository.ProductGroupExists("GroupOne"))
                .Returns(Task.FromResult(true));
            var controller = new ProductGroupsController(_uowMock.Object, _mapper);

            var result = controller.AddProductGroup(new CreateProductGroupDto { Name = "GroupOne", Products = { 2, 3 } }).Result;
            result.ShouldBeOfType(typeof(BadRequestResult));
        }

        [Test]
        public void AddProductGroup_CommitFailed_ReturnsInternalServerError()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(delegate (IMapperConfigurationExpression expression)
                    {
                        expression.CreateMap<ProductGroup, ProductGroupDto>();
                        expression.CreateMap<CreateProductGroupDto, ProductGroup>()
                            .ForMember(pg => pg.Products,
                                opt => opt.MapFrom(pgd => pgd.Products.Select(id => new ProductGroupProduct() { IDProduct = id })))
                            .ReverseMap(); ;
                    });
            _mapper = new Mapper(configuration);

            var productGroup = new ProductGroup
            {
                IDProductGroup = 1,
                IsActive = true,
                Name = "TestGroup",
                Products = new List<ProductGroupProduct>()
            };
            _uowMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(true));
            _uowMock.Setup(uow => uow.ProductGroupRepository.Insert(productGroup))
                .Returns(Task.CompletedTask);
            _uowMock.Setup(uow => uow.ProductGroupRepository.GetProductGroupByName("TestGroup"))
                .Returns(Task.FromResult(new ProductGroup { Name = "TestGroup" }));
            var controller = new ProductGroupsController(_uowMock.Object, _mapper);

            var result = controller.AddProductGroup(new CreateProductGroupDto { Name = "TestGroup", Products = { 2, 3 } }).Result;
            result.ShouldNotBe(null);
            result.ShouldBeOfType(typeof(OkObjectResult));

            var cast = result as OkObjectResult;
            var content = cast.Value as ProductGroupDto;
            content.Name.ShouldBe("TestGroup");
        }
    }
}
