using CineAPI.Common;
using CineAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace CineAPI.Infraestructure;

public static class Extensions
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
    {
        // Agregar varios middlewares
        builder.UseMiddleware<ErrorHandlerMiddleware>();
        builder.UseMiddleware<TokenExpirationMiddleware>();

        return builder;
    }

    public static DateTime ParseDate(this string date, string? format = null)
        => DateTime.ParseExact(date, format ?? "dd/MM/yyyy", CultureInfo.InvariantCulture);

    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
    }

    public static string GetClaimValue(this ClaimsPrincipal userClaimsPrincipal, string claimType)
           => userClaimsPrincipal.Claims
               .Where(claim => claim.Type.Equals(claimType, StringComparison.InvariantCultureIgnoreCase))
               .Select(claim => claim.Value)
               .FirstOrDefault();

    public static int GetUserIdFromHttpContext(this IHttpContextAccessor httpContextAccessor)
    {
        return GetUserIdFromHttpContext(httpContextAccessor.HttpContext);
    }

    public static int GetUserIdFromHttpContext(this HttpContext httpContext)
    {
        var idFromClaims = httpContext?.User.GetClaimValue("UserId");
        if (
            !string.IsNullOrWhiteSpace(idFromClaims) && int.TryParse(idFromClaims, out int idPersona))
        {
            return idPersona;
        }

        throw new CineException("Identificador de usuario inválido.");
    }
}
