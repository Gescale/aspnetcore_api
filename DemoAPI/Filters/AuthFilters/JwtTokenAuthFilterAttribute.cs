
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DemoAPI.Attributes;
using DemoAPI.Authority;
using System.Security.Claims;

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

            var claims = await Authenticator.VerifyTokenAsync(tokenString, securityKey);
            
            if (claims == null)
            {
                //Error 401 not authorised, the server doesnt know who you are
                context.Result = new UnauthorizedResult(); 
            }
            else
            {
                //get the claims requirements
                var requiredClaims = context.ActionDescriptor.EndpointMetadata
                    .OfType<RequiredClaimAttribute>()
                    .ToList();

                if (requiredClaims != null && requiredClaims.Any())
                {
                    var hasAllRequired = requiredClaims.All(rc => claims.Any(c =>
                        c.Type.Equals(rc.ClaimType, StringComparison.OrdinalIgnoreCase) &&
                        c.Value.Equals(rc.ClaimValue, StringComparison.OrdinalIgnoreCase)));

                    if (!hasAllRequired)
                    {
                        //403 Forbidden, server knows who you are but you dont have the required permissions to access this resource
                        context.Result = new StatusCodeResult(403);
                    }
                }

                else
                {
                    //If the required claims are not in the available claims, user has no permissions to access resources.
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
