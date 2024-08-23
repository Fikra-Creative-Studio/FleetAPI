using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace Fleet.Filters;

public class TokenFilter : IAsyncActionFilter
{
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

                    context.HttpContext.Items["userId"] = userIdClaim;
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
        else
        {
            context.HttpContext.Response.StatusCode = 401; // Unauthorized
            await context.HttpContext.Response.WriteAsync("Authorization header is missing.");
            return;
        }
        await next();
    }
}
