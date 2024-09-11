using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OasisoftTask.Core.DomainModels;

namespace OasisoftTask.API.Middlewares
{
    public class SecurityStampMiddleware
    {
        private readonly RequestDelegate _next;


        public SecurityStampMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var checkAllowAll = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object;

            if (context.User.Identity.IsAuthenticated && !checkAllowAll)
            {
                var identity = context.User.Identities.FirstOrDefault();
                var claims = identity?.Claims.Where(n => n.Type == "sid").ToList();
                var userId = claims?.FirstOrDefault()?.Value;
                var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var securityStamp = await userManager.GetSecurityStampAsync(user);
                    var requestSecurityStamp = context.User.FindFirst("SecurityStamp")?.Value;
                    if (securityStamp != requestSecurityStamp)
                    {
                        var signInManager = context.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
                        await signInManager.SignOutAsync();
                        context.Response.StatusCode = 401; //UnAuthorized

                        return;
                    }
                }
            }
            await _next(context);
        }
    }
}
