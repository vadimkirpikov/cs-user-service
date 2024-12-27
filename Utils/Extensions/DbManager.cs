using CsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CsApi.Utils.Extensions;

public static class DbManager
{
    public static WebApplicationBuilder AddDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DbConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        return builder;
    }
}