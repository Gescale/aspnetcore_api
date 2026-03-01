
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
//using DemoAPI.Attributes;
using DemoAPI.Authority;

namespace DemoAPI.Filters.AuthFilters
{
    public class JwtTokenAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //Steps to follow
            //:1. GetHashCode Authorisation header from the request
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string tokenString = token.ToString();

            //:2. Get rid of the bearer prefix
            if (tokenString.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                tokenString = tokenString.Substring("Bearer ".Length).Trim();
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            //:3. Get configuration and the Secret Key (not working properly)
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();  
            var securityKey = configuration?["SecurityKey"]??string.Empty;

            //:4. Verify the Token
            if (!await Authenticator.VerifyTokenAsync(tokenString, securityKey))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
