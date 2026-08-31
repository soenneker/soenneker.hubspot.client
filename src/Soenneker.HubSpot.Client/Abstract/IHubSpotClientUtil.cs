using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.HubSpot.Client.Abstract;

/// <summary>
/// Provides cached HubSpot HTTP clients authenticated with private app access tokens.
/// </summary>
public interface IHubSpotClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets a client authenticated with the configured <c>HubSpot:Token</c>.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the configured client.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a client configured for a specific HubSpot private app access token.
    /// </summary>
    /// <param name="accessToken">The HubSpot private app access token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the configured client.</returns>
    ValueTask<HttpClient> Get(string accessToken, CancellationToken cancellationToken = default);
}
