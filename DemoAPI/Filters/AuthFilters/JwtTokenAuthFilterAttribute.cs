
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

            //:4. Verify the Token and extract te claims
            //if (!await Authenticator.VerifyTokenAsync(tokenString, securityKey))
            //{
            //    context.Result = new UnauthorizedResult();
            //}

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

                //if(requiredClaims != null && requiredClaims.All(rc => claims.Any(c => 
                //c.Type.Equals(rc.ClaimType, StringComparison.OrdinalIgnoreCase) &&
                //c.Value.Equals(rc.ClaimValue, StringComparison.OrdinalIgnoreCase))))
                //{
                //    //403 Forbidden, server knows who you are but you dont have the required permissions to access this resource
                //    context.Result = new StatusCodeResult(403); 
                //}


                
                if (requiredClaims != null && requiredClaims.Any())
                {
                    var hasAllRequired = requiredClaims.All(rc => claims.Any(c =>
                        c.Type.Equals(rc.ClaimType, StringComparison.OrdinalIgnoreCase) &&
                        c.Value.Equals(rc.ClaimValue, StringComparison.OrdinalIgnoreCase)));

                    var counter = 1;
                    Console.WriteLine("Required Claims:");
                    foreach (var _rclaim in requiredClaims)
                    {
                        Console.WriteLine($"{counter}. requiredClaim.Type : " + _rclaim.ClaimType.ToString());
                        Console.WriteLine($"{counter}. requiredClaim.Value : " + _rclaim.ClaimValue.ToString());
                    }

                    counter = 1;
                    Console.WriteLine("Extracted Claims:");
                    foreach (var _claim in claims)
                    {
                        Console.WriteLine($"{counter}. claim.Type : " + _claim.Type.ToString());
                        Console.WriteLine($"{counter}. claim.Value : " + _claim.Value.ToString());
                    }

                        if (!hasAllRequired)
                    {
                        //403 Forbidden, server knows who you are but you dont have the required permissions to access this resource
                        context.Result = new StatusCodeResult(403);
                    }
                }

                else
                {
                    //If there are no required claims, user has no permissions to access resources.
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
