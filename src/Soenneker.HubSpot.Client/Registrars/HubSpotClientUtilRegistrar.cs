using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.HubSpot.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.HubSpot.Client.Registrars;

/// <summary>
/// Registers the cached HubSpot HTTP client provider.
/// </summary>
public static class HubSpotClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="HubSpotClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddHubSpotClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddSingleton<IHubSpotClientUtil, HubSpotClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="HubSpotClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddHubSpotClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddScoped<IHubSpotClientUtil, HubSpotClientUtil>();

        return services;
    }
}
