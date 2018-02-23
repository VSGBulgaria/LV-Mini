using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LVMiniApi.Filters
{
    internal class ValidateModel : ActionFilterAttribute
    {
        /// <summary>
        /// Validates if the ModelState is valid for the given action.
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
