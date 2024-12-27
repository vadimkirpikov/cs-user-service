using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CsApi.Models.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CsApi.Utils.Extensions;

public static class Authorizer
{
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        builder.Services.Configure<JwtSettings>(jwtSettings).AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey("admin-secret-key+admin-secret-key"u8.ToArray()),
                };
            });
        return builder;
    }
}