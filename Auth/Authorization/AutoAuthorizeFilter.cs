using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Auth.Authorization
{
    public class AutoAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IActionContextAccessor _contextAccessor;

        public AutoAuthorizeFilter(IAuthorizationService authorizationService, IActionContextAccessor contextAccessor)
        {
            _authorizationService = authorizationService;
            _contextAccessor = contextAccessor;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var routeValues = context.RouteData.Values;

            var controller = routeValues["controller"]?.ToString();
            var action = routeValues["action"]?.ToString();

            if (controller == null || action == null)
                return;

            var policyName = $"{controller}.{action}";

            var result = await _authorizationService.AuthorizeAsync(context.HttpContext.User, null, policyName);

            
            if (!result.Succeeded)
            {
                context.Result = new ForbidResult(); // or RedirectToAction("AccessDenied")
            }
        }
    }
}
