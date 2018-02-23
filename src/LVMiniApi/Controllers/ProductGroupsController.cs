using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniApi.Filters;
using LVMiniApi.Helpers;
using LVMiniApi.Models;
using LVMiniApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVMiniApi.Controllers
{
    /// <summary>
    /// Controller for manipulating ProductGroups and the products in them.
    /// </summary>
    [Route("api/productgroups")]
    [Produces("application/json")]
    public class ProductGroupsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly ITypeHelperService _typeHelperService;

        /// <summary>
        /// Injects the services needed through constructor injection.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">AutoMapper's Mapper class.</param>
        /// <param name="typeHelperService"></param>
        public ProductGroupsController(IUnitOfWork unitOfWork, IMapper mapper, ITypeHelperService typeHelperService)
        {
            _unitOfWork = unitOfWork;
            _typeHelperService = typeHelperService;
            _productGroupRepository = _unitOfWork.ProductGroupRepository;
            Mapper = mapper;
        }

        /// <summary>
        /// Gets all existing ProductGroups in the database.
        /// </summary>
        /// <returns>Http 200 OK and a collection of ProductGroups.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductGroupDto>), 200, StatusCode = StatusCodes.Status200OK)]
        public IActionResult GetAllProductGroups(ProductGroupResourceParameters resourceParameters)
        {
            if (!_typeHelperService.TypeHasProperties<ProductGroupDto>(resourceParameters.Fields))
            {
                return BadRequest();
            }

            var entities = _productGroupRepository.GetAll();

            var productGroups = Mapper.Map<IEnumerable<ProductGroupDto>>(entities).ShapeData(resourceParameters.Fields);
            return Ok(productGroups);
        }

        /// <summary>
        /// Gets a specific ProductGroup based on its name.
        /// </summary>
        /// <param name="name">The name of the ProductGroup.</param>
        /// <param name="fields">The fields by which you want to shape the data.</param>
        /// <returns>
        /// Http 200 OK and a single ProductGroup of the requested group.
        /// Http 404 NotFound if there is no such group.
        /// </returns>
        [HttpGet("{name}", Name = "ProductGroupGet")]
        [ProducesResponseType(typeof(ProductGroupDto), 200, StatusCode = StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductGroup(string name, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<ProductGroupDto>(fields))
            {
                return BadRequest();
            }

            if (!await _productGroupRepository.ProductGroupExists(name))
            {
                return NotFound();
            }

            var productGroup = await _productGroupRepository.GetProductGroupByName(name);

            var productGroupToReturn = Mapper.Map<ProductGroupDto>(productGroup);
            return Ok(productGroupToReturn.ShapeData(fields));
        }

        /// <summary>
        /// Creates a new ProductGroup with an optional initial collection of products. It could be empty as well.
        /// </summary>
        /// <param name="productGroup">The ProductGroup information needed to create a new one.</param>
        /// <returns>
        /// Http 201 Created with the created ProductGroup.
        /// Http 400 BadRequest if the group already exists.
        /// Http 500 if the insert to the database fails.
        /// </returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductGroupDto), 201, StatusCode = StatusCodes.Status201Created)]
        public async Task<IActionResult> AddProductGroup([FromBody] CreateProductGroupDto productGroup)
        {
            if (await _productGroupRepository.ProductGroupExists(productGroup.Name))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            var entity = Mapper.Map<ProductGroup>(productGroup);
            await _productGroupRepository.Insert(entity);

            if (!await _unitOfWork.Commit())
            {
                return new StatusCodeResult(500);
            }

            var createdProductGroup = await _productGroupRepository.GetProductGroupByName(entity.Name);

            return CreatedAtRoute("ProductGroupGet", new { name = createdProductGroup.Name.ToLower() }, Mapper.Map<ProductGroupDto>(createdProductGroup));
        }

        /// <summary>
        /// Adds an existing product to an existing ProductGroup.
        /// </summary>
        /// <param name="name">The ProductGroup name.</param>
        /// <param name="productCode">The product code.</param>
        /// <returns>
        /// Http 200 OK, the ProductGroup with an updated list of products.
        /// Http 404 NotFound if either the ProductGroup or Product don't exist.
        /// </returns>
        [HttpPost("{name}/products/{productCode}")]
        [ProducesResponseType(typeof(ProductGroupDto), 200, StatusCode = StatusCodes.Status200OK)]
        public async Task<IActionResult> AddProductToProductGroup(string name, string productCode)
        {
            if (!await _productGroupRepository.ProductGroupExists(name))
            {
                return NotFound();
            }

            var productGroup = await _productGroupRepository.GetProductGroupByName(name);

            var product = await _productGroupRepository.GetProductByCode(productCode);
            if (product == null)
            {
                return NotFound();
            }

            productGroup.Products.Add(new ProductGroupProduct { IDProduct = product.IDProduct });
            await _unitOfWork.Commit();

            var updatedProductGroup = await _productGroupRepository.GetById(productGroup.IDProductGroup);
            var groupToReturn = Mapper.Map<ProductGroupDto>(updatedProductGroup);
            return Ok(groupToReturn);
        }

        /// <summary>
        /// Updates an existing ProductGroup's information.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="productGroup">The fields you want ot update. All are optional.</param>
        /// <returns>
        /// Http 200 OK with the updated ProductGroup.
        /// Http 404 NotFound if the group doesn't exist.
        /// </returns>
        [HttpPatch("{name}")]
        [ValidateModel]
        [ProducesResponseType(typeof(ProductGroupDto), 200, StatusCode = StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateGroup(string name, [FromBody] UpdateProductGroupDto productGroup)
        {
            if (!await _productGroupRepository.ProductGroupExists(name))
            {
                return NotFound();
            }

            var productGroupEntity = await _productGroupRepository.GetProductGroupByName(name);

            Mapper.Map(productGroup, productGroupEntity);
            await _unitOfWork.Commit();

            var modelToReturn = Mapper.Map<ProductGroupDto>(productGroupEntity);
            return Ok(modelToReturn);
        }

        /// <summary>
        /// Deletes an existing ProductGroup and the reference to it's products.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <returns>
        /// Http 204 NoContent if the delete was successful.
        /// Http 404 NotFound if the group does not exist.
        /// </returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(204, StatusCode = StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProductGroup(string name)
        {
            if (!await _productGroupRepository.ProductGroupExists(name))
            {
                return NotFound();
            }

            var productGroup = await _productGroupRepository.GetProductGroupByName(name);

            _productGroupRepository.Delete(productGroup);
            await _unitOfWork.Commit();

            return NoContent();
        }

        /// <summary>
        /// Deletes a Product from a ProductGroup.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="productCode">The code of the Product.</param>
        /// <returns>
        /// Http 200 OK if the product was deleted and the group with an updated list of products.
        /// Http 404 NotFound if either the ProductGroup or the Product don't exist or the ProductGroup doesn't contain the product.
        /// </returns>
        [HttpDelete("{name}/products/{productCode}")]
        [ProducesResponseType(typeof(ProductGroupDto), 200, StatusCode = StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveProductFromGroup(string name, string productCode)
        {
            if (!await _productGroupRepository.ProductGroupExists(name))
            {
                return NotFound();
            }

            var productGroup = await _productGroupRepository.GetProductGroupByName(name);

            var product = await _productGroupRepository.GetProductByCode(productCode);
            if (product == null)
            {
                return NotFound();
            }

            if (!await _productGroupRepository.ProductGroupContainsProduct(name, productCode))
            {
                return NotFound();
            }

            productGroup.Products.Remove(productGroup.Products.FirstOrDefault(pg => pg.IDProduct == product.IDProduct));
            await _unitOfWork.Commit();

            var updatedProductGroup = await _productGroupRepository.GetById(productGroup.IDProductGroup);

            var groupToReturn = Mapper.Map<ProductGroupDto>(updatedProductGroup);

            return Ok(groupToReturn);
        }
    }
}