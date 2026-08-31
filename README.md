[![](https://img.shields.io/nuget/v/soenneker.hubspot.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.hubspot.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.hubspot.client/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.hubspot.client/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.hubspot.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.hubspot.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.hubspot.client/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.hubspot.client/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.HubSpot.Client

Reuse HubSpot HTTP clients authenticated with one or more private app access tokens.

## Install

```bash
dotnet add package Soenneker.HubSpot.Client
```

## Configure a default account

```json
{
  "HubSpot": {
    "Token": "<private app access token>"
  }
}
```

The parameterless `Get()` requires `HubSpot:Token`. You can omit it when every call supplies a token explicitly.

## Register

```csharp
using Soenneker.HubSpot.Client.Registrars;

services.AddHubSpotClientUtilAsSingleton();
```

Use `AddHubSpotClientUtilAsScoped()` when each scope should own its client set. Provider instances use isolated cache keys, so disposing one scope removes only the HTTP clients created by that provider.

## Usage

```csharp
using Soenneker.HubSpot.Client.Abstract;

HttpClient client = await hubSpotClient.Get(cancellationToken);

HttpResponseMessage response = await client.GetAsync(
    "crm/v3/objects/contacts",
    cancellationToken);
response.EnsureSuccessStatusCode();
```

The returned client targets `https://api.hubapi.com/` and sends `Authorization: Bearer <token>`.

To work with multiple HubSpot accounts, pass each private app access token explicitly:

```csharp
HttpClient tenantClient = await hubSpotClient.Get(
    tenantAccessToken,
    cancellationToken);
```

Repeated calls with the same token on the same provider reuse a client. Distinct tokens receive separate clients, preventing credentials from leaking between accounts.

The provider owns returned clients. Let the service container dispose the provider rather than disposing individual cached clients.
