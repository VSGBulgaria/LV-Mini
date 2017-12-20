using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Service.Core.Entities;
using LVMiniApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LVMiniApi.Models
{
    public class UserUrlResolver : IValueResolver<User, UserModel, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(User source, UserModel destination, string destMember, ResolutionContext context)
        {
            var url = (IUrlHelper) _httpContextAccessor.HttpContext.Items[BaseController.Urlhelper];
            return url.Link("UserGet", new {username = source.Username});
        }
    }
}
