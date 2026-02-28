using Microsoft.AspNetCore.Mvc;
using DemoAPI.Authority;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DemoAPI.Controllers
{
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthorityController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody]AppCredential credential)
        {
            if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                return Ok(new
                {
                    access_token = CreateToken(credential.ClientId, expiresAt),
                    expires_at = expiresAt
                });
            }
            else
            {
                ModelState.AddModelError("Unauthorised", "You are not authorised.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
        }

        private string CreateToken(string clientId, DateTime expiresAt)
        {
            // The JWT Strucure consists of three parts: Header, Payload, and Signature (Signing key)

            //Algorithm
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["SecurityKey"] ?? string.Empty)),
                SecurityAlgorithms.HmacSha256Signature);

            //Payload
            var app = AppRepository.GetApplicationByClientId(clientId);
            var claimsDictionary = new Dictionary<string, object>
            {
                {"AppName", app?.ApplicationName?? string.Empty },
                {"Read", (app?.Scopes ?? string.Empty).Contains("read")? "true" : "false" },
                {"Write", (app?.Scopes ?? string.Empty).Contains("write")? "true" : "false" },
                {"Delete", (app?.Scopes ?? string.Empty).Contains("delete")? "true" : "false" }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = signingCredentials,
                Claims = claimsDictionary,
                Expires = expiresAt,
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
