using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniApi.Controllers;
using LVMiniApi.Helpers;
using LVMiniApi.Models;
using LVMiniApi.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LVMiniApi.Tests.ControllerTests
{
    [TestFixture]
    public class ProductGroupsControllerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private IMapper _mapper;
        private Mock<ITypeHelperService> _typeHelperService;

        [SetUp]
        public void SetUp()
        {
            _typeHelperService = new Mock<ITypeHelperService>();
            _uowMock = new Mock<IUnitOfWork>();
        }

        [Test]
        public void GetAllProductGroups_WhenCalled_ReturnOkAndACollection()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(a => a.CreateMap<ProductGroup, ProductGroupDto>());
            _typeHelperService.Setup(t => t.TypeHasProperties<ProductGroupDto>(null)).Returns(true);
            _mapper = new Mapper(configuration);
            _uowMock.Setup(uow => uow.ProductGroupRepository.GetAll(null))
                .Returns(new List<ProductGroup> { new ProductGroup { Name = "ProductGroupTest" } });
            var controller = new ProductGroupsController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result = controller.GetAllProductGroups(new ProductGroupResourceParameters { Fields = null }) as OkObjectResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(200);

            var content = result.Value as List<ExpandoObject>;
            content.ShouldNotBe(null);
            content.ShouldBeOfType(typeof(List<ExpandoObject>));
            content.Count.ShouldBe(1);
        }

        [Test]
        public void GetSingleProductGroup_UnexistingProductGroup_ReturnsNotFound()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(a => a.CreateMap<ProductGroup, ProductGroupDto>());
            _typeHelperService.Setup(t => t.TypeHasProperties<ProductGroupDto>(null)).Returns(true);
            _mapper = new Mapper(configuration);
            _uowMock.Setup(uow => uow.ProductGroupRepository.GetProductGroupByName("ProductGroupOne"))
                .Returns(Task.FromResult(new ProductGroup { Name = "ProductGroupOne" }));
            var controller = new ProductGroupsController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result = controller.GetProductGroup("NonExistingGroup", null).Result as NotFoundResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(404);
        }

        [Test]
        public void GetSingleProductGroup_ExistingProductGroup_ReturnsOkAndProductGroup()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(a => a.CreateMap<ProductGroup, ProductGroupDto>());

            _typeHelperService.Setup(t => t.TypeHasProperties<ProductGroupDto>(null)).Returns(true);
            _mapper = new Mapper(configuration);

            _uowMock.Setup(uow => uow.ProductGroupRepository.ProductGroupExists("ProductGroupOne"))
                .Returns(Task.FromResult(true));

            _uowMock.Setup(uow => uow.ProductGroupRepository.GetProductGroupByName("ProductGroupOne"))
                .Returns(Task.FromResult(new ProductGroup { Name = "ProductGroupOne" }));

            var controller = new ProductGroupsController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result = controller.GetProductGroup("ProductGroupOne", null).Result as OkObjectResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(200);


            var value = result.Value as ExpandoObject;
            value.ShouldNotBe(null);
            var content = value as IDictionary<string, object>;
            value.ShouldNotBe(null);
            object name = "ProductGroupOne";
            content.TryGetValue("Name", out name).ShouldBe(true);
        }

        [Test]
        public void AddProductGroup_AlreadyExistingProductGroup_ReturnsBadRequest()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(a => a.CreateMap<ProductGroup, ProductGroupDto>());
            _mapper = new Mapper(configuration);
            _uowMock.Setup(uow => uow.ProductGroupRepository.ProductGroupExists("GroupOne"))
                .Returns(Task.FromResult(true));
            var controller = new ProductGroupsController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result = controller.AddProductGroup(new CreateProductGroupDto { Name = "GroupOne", Products = { 2, 3 } }).Result;
            result.ShouldBeOfType(typeof(BadRequestResult));
        }

        [Test]
        public void AddProductGroup_SuccessfulPost_ReturnsCreatedAtRoute()
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
            var controller = new ProductGroupsController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result = controller.AddProductGroup(new CreateProductGroupDto { Name = "TestGroup", Products = { 2, 3 } }).Result;
            result.ShouldNotBe(null);
            result.ShouldBeOfType(typeof(CreatedAtRouteResult));

            var cast = result as CreatedAtRouteResult;
            var content = cast.Value as ProductGroupDto;
            content.Name.ShouldBe("TestGroup");
        }

        public void AddProductToProductGroup_SuccessfulPost_ReturnsOkWithUpdatedProductGroup()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(delegate (IMapperConfigurationExpression expression)
                {
                    expression.CreateMap<ProductGroup, ProductGroupDto>();
                });
            _mapper = new Mapper(configuration);


        }

        [Test]
        public void UpdateGroup_SuccessfulPatch_ReturnsOkWithUpdatedProductGroup()
        {
            MapperConfiguration configuration =
                new MapperConfiguration(delegate (IMapperConfigurationExpression expression)
                {
                    expression.CreateMap<ProductGroup, ProductGroupDto>();
                    expression.CreateMap<UpdateProductGroupDto, ProductGroup>();
                });
            _mapper = new Mapper(configuration);

            _uowMock.Setup(uow => uow.ProductGroupRepository.ProductGroupExists("ProductGroupOne"))
                .Returns(Task.FromResult(true));
            _uowMock.Setup(uow => uow.ProductGroupRepository.GetProductGroupByName("ProductGroupOne"))
                .Returns(Task.FromResult(new ProductGroup { Name = "ProductGroupOne" }));
            _uowMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(true));

            var controller = new ProductGroupsController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result =
                controller.UpdateGroup("ProductGroupOne", new UpdateProductGroupDto { Name = "ProductGroupTwo" }).Result as OkObjectResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(200);

            var content = result.Value as ProductGroupDto;
            content.ShouldNotBe(null);
            content.Name.ShouldBe("ProductGroupTwo");
        }

        [Test]
        public void DeleteProductGroup_SuccessfulDelete_ReturnsNoContent()
        {
            _uowMock.Setup(uow => uow.ProductGroupRepository.ProductGroupExists("ProductGroup"))
                .Returns(Task.FromResult(true));
            _uowMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(true));
            var controller = new ProductGroupsController(_uowMock.Object, _mapper, _typeHelperService.Object);

            var result = controller.DeleteProductGroup("ProductGroup").Result as NoContentResult;
            result.ShouldNotBe(null);
            result.StatusCode.ShouldBe(204);
        }
    }
}
