using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusiciansAPP.DAL.DBDataProvider;

namespace MusiciansAPP.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> SetUpDB(this WebApplication app)
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        return app;
    }
}