using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GymMangamentSystem.Apis.Helpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                context.Result = new JsonResult(new { message = "You are not authorized to access this resource." })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }

}
