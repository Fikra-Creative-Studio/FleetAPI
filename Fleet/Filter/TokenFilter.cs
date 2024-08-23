using Fleet.Helpers;
using Fleet.Interfaces.Service;
using Fleet.Models;
using Fleet.Service;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Fleet.Filters;

public class TokenFilter(ILoggedUser loggedUser, IConfiguration configuration) : IAsyncActionFilter
{
    private string Secret { get => configuration.GetValue<string>("Crypto:Secret"); }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        if (request.Headers.ContainsKey("Authorization"))
        {
            var authHeader = request.Headers.Authorization.ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : authHeader;

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "user")?.Value;

                    loggedUser.UserId = int.Parse(CriptografiaHelper.DescriptografarAes(userIdClaim, Secret));
                }
                catch
                {
                    context.HttpContext.Response.StatusCode = 401; // Unauthorized
                    await context.HttpContext.Response.WriteAsync("Invalid token.");
                    return;
                }
            }
            else
            {
                context.HttpContext.Response.StatusCode = 401; // Unauthorized
                await context.HttpContext.Response.WriteAsync("Token is missing.");
                return;
            }
        }
        else if(!context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute))) // se não contem autorização e o endpoint nao é anonimo
        {
            context.HttpContext.Response.StatusCode = 401; // Unauthorized
            await context.HttpContext.Response.WriteAsync("Authorization header is missing.");
            return;
        }

        await next();
    }
}
