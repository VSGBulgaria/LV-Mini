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
        public IActionResult Get()
        {
            var entities = _productGroupRepository.GetAll();

            var productGroups = Mapper.Map<IEnumerable<DispalyProductGroupDto>>(entities);
            return Ok(productGroups);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductGroup([FromBody] ProductGroupDto productGroup)
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

            var productGroupToReturn = Mapper.Map<ProductGroupDto>(entity);
            return Ok(productGroupToReturn);
        }

        [HttpPost("{id}/products/{productId}")]
        public async Task<IActionResult> AddProductToProductGroup(int id, int productId)
        {
            var product = await _productGroupRepository.GetById(id);
            ProductGroupProduct productGroup = new ProductGroupProduct
            {
                IDProductGroup = product.IDProductGroup,
                IDProduct = productId
            };

            product.Products.Add(productGroup);
            await _unitOfWork.Commit();
            return Ok();
        }
    }
}