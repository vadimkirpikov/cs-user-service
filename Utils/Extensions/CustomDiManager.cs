using CsApi.Repositories.Implementations;
using CsApi.Repositories.Interfaces;
using CsApi.Services.Implementations;
using CsApi.Services.Interfaces;
using CsApi.Utils.Implementations;
using CsApi.Utils.Interfaces;

namespace CsApi.Utils.Extensions;

public static class CustomDiManager
{
    public static WebApplicationBuilder InjectDependencies(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IJwtUtils, JwtUtils>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ISubscriptionRepository, SubscriptionRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ISubscriptionService, SubscriptionService>();
        return builder;
    }
}