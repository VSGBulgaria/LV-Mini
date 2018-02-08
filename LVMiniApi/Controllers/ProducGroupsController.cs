using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVMiniApi.Controllers
{
    [Route("api/productgroups")]
    public class ProducGroupsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductGroupRepository _productGroupRepository;

        public ProducGroupsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productGroupRepository = _unitOfWork.ProductGroupRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllProductGroups()
        {
            var entities = _productGroupRepository.GetAll();

            var productGroups = Mapper.Map<IEnumerable<ProductGroupDto>>(entities);
            return Ok(productGroups);
        }

        [HttpGet("{name}", Name = "ProductGroupGet")]
        public async Task<IActionResult> GetSingleProductGroup(string name)
        {
            var productGroup = await _productGroupRepository.GetProductGroupByName(name);
            if (productGroup == null)
            {
                return NotFound();
            }

            var prodcutGroupToReturn = Mapper.Map<ProductGroupDto>(productGroup);
            return Ok(prodcutGroupToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductGroup([FromBody] CreateProductGroupDto productGroup)
        {
            if (await _productGroupRepository.ProductGroupExists(productGroup.Name))
            {
                return BadRequest();
            }

            var entity = Mapper.Map<ProductGroup>(productGroup);
            await _productGroupRepository.Insert(entity);

            if (!await _unitOfWork.Commit())
            {
                return new StatusCodeResult(500);
            }

            var createdProductGroup = await _productGroupRepository.GetById(entity.IDProductGroup);
            var groupToReturn = Mapper.Map<ProductGroupDto>(createdProductGroup);

            return Ok(groupToReturn);
        }

        [HttpPost("{name}/products/{productCode}")]
        public async Task<IActionResult> AddProductToProductGroup(string name, string productCode)
        {
            var productGroup = await _productGroupRepository.GetProductGroupByName(name);
            if (productGroup == null)
            {
                return NotFound();
            }

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

        [HttpPatch("{name}")]
        public async Task<IActionResult> UpdateGroup(string name, [FromBody] UpdateProductGroupDto productGroup)
        {
            var productGroupEntity = await _productGroupRepository.GetProductGroupByName(name);
            if (productGroupEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(productGroup, productGroupEntity);
            await _unitOfWork.Commit();

            var modelToReturn = Mapper.Map<ProductGroupDto>(productGroupEntity);
            return Ok(modelToReturn);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteProductGroup(string name)
        {
            var productGroup = await _productGroupRepository.GetProductGroupByName(name);

            _productGroupRepository.Delete(productGroup);
            await _unitOfWork.Commit();

            return NoContent();
        }

        [HttpDelete("{name}/products/{productCode}")]
        public async Task<IActionResult> RemoveProductFromGroup(string name, string productCode)
        {
            var productGroup = await _productGroupRepository.GetProductGroupByName(name);
            if (productGroup == null)
            {
                return NotFound();
            }

            var product = await _productGroupRepository.GetProductByCode(productCode);
            if (product == null)
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