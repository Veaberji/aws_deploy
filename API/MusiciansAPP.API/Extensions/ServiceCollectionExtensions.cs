using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusiciansAPP.API.Configs;
using MusiciansAPP.API.Services;
using MusiciansAPP.BL.Services.Albums;
using MusiciansAPP.BL.Services.Artists;
using MusiciansAPP.BL.Services.CSV;
using MusiciansAPP.BL.Services.Tracks;
using MusiciansAPP.DAL;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.DAL.WebDataProvider.ImageProvider;

namespace MusiciansAPP.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed(uri =>
                        new Uri(uri).Host == AppConfigs.Host)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("Content-Disposition");
            }));

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        AddAPIServices(services);
        AddBLServices(services);
        AddDALServices(services, config);

        services.AddAutoMapper(cfg =>
            cfg.AddMaps(
                "MusiciansAPP.API",
                "MusiciansAPP.BL",
                "MusiciansAPP.DAL"));

        return services;
    }

    public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration config)
    {
#if DEBUG
        services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(
                config.GetConnectionString("DefaultConnection")));
#else
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));
#endif

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static void AddAPIServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSingleton<IPagingHelper, PagingHelper>();
        services.AddSingleton<IThemeDataProvider, ThemeDataProvider>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddScoped<IErrorHandler, ErrorHandler>();
    }

    private static void AddBLServices(IServiceCollection services)
    {
        services.AddScoped<IArtistsService, ArtistsService>();
        services.AddScoped<IAlbumsService, AlbumsService>();
        services.AddScoped<ITracksService, TracksService>();
        services.AddScoped<ICSVService<ArtistBL>, ArtistsCSVService>();
    }

    private static void AddDALServices(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IHttpClient, HttpClientWrapper>();
        services.AddSingleton<IDataLogger<IUnitOfWork>, DBLogger>();
        services.AddSingleton<IDataLogger<IWebDataProvider>, WebDataProviderLogger>();
        services.AddScoped<IImageProvider, WikipediaDataProvider>();

#if DEBUG
        var apiKey = config["Secrets:LastFmApiKey"];
#else
        var apiKey = Environment.GetEnvironmentVariable("LASTFM_KEY");
#endif

        services.AddScoped<IWebDataProvider>(p =>
            ActivatorUtilities.CreateInstance<LastFmDataProvider>(p, apiKey));
    }
}