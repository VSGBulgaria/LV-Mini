﻿using AutoMapper;
using Data.Service.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LVMiniApi.Controllers
{
    public abstract class BaseController : Controller
    {
        public const string Urlhelper = "URLHELPER";
        protected IMapper Mapper;
        protected ILogRepository LogRepository;
        protected IUserRepository UserRepository;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            context.HttpContext.Items[Urlhelper] = Url;
        }
    }
}
