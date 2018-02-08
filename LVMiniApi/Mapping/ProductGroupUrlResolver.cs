using AutoMapper;
using Data.Service.Core.Entities;
using LVMiniApi.Controllers;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniApi.Mapping
{
    public class ProductGroupUrlResolver : IValueResolver<ProductGroup, ProductGroupDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductGroupUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(ProductGroup source, ProductGroupDto destination, string destMember, ResolutionContext context)
        {
            var url = (IUrlHelper)_httpContextAccessor.HttpContext.Items[BaseController.Urlhelper];
            return url.Link("ProductGroupGet", new { name = source.Name.ToLower() });
        }
    }
}