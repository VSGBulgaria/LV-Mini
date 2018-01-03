using AutoMapper;
using Data.Service.Core.Entities;
using LVMiniApi.Controllers;
using LVMiniApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniApi.Mapping
{
    internal class UserUrlResolver : IValueResolver<User, UserModel, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(User source, UserModel destination, string destMember, ResolutionContext context)
        {
            var url = (IUrlHelper)_httpContextAccessor.HttpContext.Items[BaseController.Urlhelper];
            return url.Link("UserGet", new { username = source.Username });
        }
    }
}
