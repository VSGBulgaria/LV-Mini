using AutoMapper;
using Data.Service.Core.Entities;
using Data.Service.Core.Interfaces;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LVMiniApi.Controllers
{
    [Route("api/productgroups")]
    public class ProducGroupsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductGroupRepository _productGroupRepository;

        public ProducGroupsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productGroupRepository = _unitOfWork.ProductGroupRepository;
        }

        [HttpGet]
        public IActionResult GetAllProductGroups()
        {
            var entities = _productGroupRepository.GetAll();

            var productGroups = Mapper.Map<IEnumerable<ProductGroupDto>>(entities);
            return Ok(productGroups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleProductGroup(int id)
        {
            var productGroup = await _productGroupRepository.GetById(id);
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

        [HttpPost("{id}/products/{productId}")]
        public async Task<IActionResult> AddProductToProductGroup(int id, int productId)
        {
            var productGroup = await _productGroupRepository.GetById(id);
            if (productGroup == null)
            {
                return NotFound();
            }

            productGroup.Products.Add(new ProductGroupProduct() { IDProduct = productId });
            await _unitOfWork.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> MakeGroupInactive(int id)
        {
            var productGroup = await _productGroupRepository.GetById(id);

            productGroup.IsActive = false;
            await _unitOfWork.Commit();

            return NoContent();
        }
    }
}