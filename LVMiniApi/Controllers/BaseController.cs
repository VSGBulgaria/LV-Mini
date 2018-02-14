using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LVMiniApi.Controllers
{
    /// <summary>
    /// Combines base functionality needed throughout the API.
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// A Url constant used for AutoMapper surrogate keys.
        /// </summary>
        public const string Urlhelper = "URLHELPER";

        /// <summary>
        /// An instance of IMapper to be injected in the children classes.
        /// </summary>
        protected IMapper Mapper;

        /// <summary>
        /// Configure the UrlHelper for the surrogate key.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            context.HttpContext.Items[Urlhelper] = Url;
        }
    }
}
