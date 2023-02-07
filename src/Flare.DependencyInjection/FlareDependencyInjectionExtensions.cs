using Flare.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flare.DependencyInjection;

public static class FlareDependencyInjectionExtensions
{
    public static IServiceCollection AddFlare(this IServiceCollection services, Action<FlareConfigurator> configurator)
    {
        services.AddSingleton<IFlareClient, FlareClient>();
        
        return services;
    }

    public static IServiceCollection AddFlare(this IServiceCollection services)
    {
        // services.AddSingleton<FlareConfig>();
        services.AddSingleton<IFlareClient, FlareClient>();
        services.AddSingleton<IFlareConfigProvider, FlareConfigProvider>();
        
        return services;
    }
}