using Auth.Models;
using Microsoft.AspNetCore.Identity;

public class ForcePasswordChangeMiddleware
{
    private readonly RequestDelegate _next;

    public ForcePasswordChangeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var session = context.Session;
        var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var signInManager = context.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();

        var user = await userManager.GetUserAsync(context.User);
        if (user != null && !user.IsPasswordChanged && !context.Request.Path.Value.Contains("/Identity/Account/Manage/ChangePassword"))
        {
            context.Response.Redirect("/Identity/Account/Manage/ChangePassword");
            return;
        }

        await _next(context);
    }
}
