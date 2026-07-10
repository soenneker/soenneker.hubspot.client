using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.Hashing.XxHash;
using Soenneker.HubSpot.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.HubSpot.Client;

///<inheritdoc cref="IHubSpotClientUtil"/>
public sealed class HubSpotClientUtil : IHubSpotClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly string _accessToken;
    private readonly ConcurrentDictionary<string, byte> _clientIds = new();

    private static readonly Uri _prodBaseUrl = new("https://api.hubapi.com/");

    public HubSpotClientUtil(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _accessToken = config.GetValueStrict<string>("HubSpot:Token");
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return Get(_accessToken, cancellationToken);
    }

    public ValueTask<HttpClient> Get(string accessToken, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

        string clientId = GetClientId(accessToken);
        _clientIds.TryAdd(clientId, 0);

        return _httpClientCache.Get(clientId, accessToken, static token => new HttpClientOptions
        {
            BaseAddress = _prodBaseUrl,
            DefaultRequestHeaders = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {token}"},
            }
        }, cancellationToken);
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        foreach (string clientId in _clientIds.Keys)
        {
            _httpClientCache.RemoveSync(clientId);
        }
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        foreach (string clientId in _clientIds.Keys)
        {
            await _httpClientCache.Remove(clientId);
        }
    }

    private static string GetClientId(string accessToken)
    {
        return $"{nameof(HubSpotClientUtil)}:{XxHash3Util.Hash(accessToken)}";
    }
}
