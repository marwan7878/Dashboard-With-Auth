using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Auth.Attributes
{
    public class AutoAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IActionContextAccessor _contextAccessor;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AutoAuthorizeFilter(IAuthorizationService authorizationService, IActionContextAccessor contextAccessor, SignInManager<ApplicationUser> signInManager)
        {
            _authorizationService = authorizationService;
            _contextAccessor = contextAccessor;
            _signInManager = signInManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!_signInManager.IsSignedIn(context.HttpContext.User))
            {
                context.Result = new ChallengeResult();
                return;
            }

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
