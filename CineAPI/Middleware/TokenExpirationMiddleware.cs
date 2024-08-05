using CineAPI.Common;
using CineAPI.Infraestructure;
using Microsoft.Extensions.Options;

namespace CineAPI.Middleware;

public class TokenExpirationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtSettings _jwtSettings;

    public TokenExpirationMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
    {
        _next = next;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            var tokenExpiration = GetTokenExpirationDate(token);

            if (tokenExpiration < DateTime.UtcNow)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                throw new CineException("Su sesión ha expirado");
            }
        }

        await _next(context);
    }

    private DateTime GetTokenExpirationDate(string token)
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var exp = jwtToken.Payload.Exp;
        return DateTimeOffset.FromUnixTimeSeconds(exp.Value).UtcDateTime;
    }
}
