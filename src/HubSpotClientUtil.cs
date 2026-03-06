using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.HubSpot.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.HubSpot.Client;

///<inheritdoc cref="IHubSpotClientUtil"/>
public sealed class HubSpotClientUtil : IHubSpotClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _config;

    private const string _prodBaseUrl = "https://api.hubapi.com/";

    public HubSpotClientUtil(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _config = config;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        // No closure: state passed explicitly + static lambda
        return _httpClientCache.Get(nameof(HubSpotClientUtil), (config: _config, prodBaseUrl: _prodBaseUrl), static state =>
        {
            var apiKey = state.config.GetValueStrict<string>("HubSpot:Token");

            return new HttpClientOptions
            {
                BaseAddress = new Uri(state.prodBaseUrl),
                DefaultRequestHeaders = new Dictionary<string, string>
                {
                    {"Authorization", $"Bearer {apiKey}"},
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        _httpClientCache.RemoveSync(nameof(HubSpotClientUtil));
    }

    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(nameof(HubSpotClientUtil));
    }
}
