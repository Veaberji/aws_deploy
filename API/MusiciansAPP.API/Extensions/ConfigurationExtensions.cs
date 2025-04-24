using Microsoft.Extensions.Configuration;
using MusiciansAPP.API.Configs;

namespace MusiciansAPP.API.Extensions;

public static class ConfigurationExtensions
{
    public static IConfiguration BindObjects(this IConfiguration config)
    {
        config.Bind("AppConfigs", new AppConfigs());
        config.Bind("Theme", new Theme());

        return config;
    }
}
