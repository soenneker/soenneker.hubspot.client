using Soenneker.HubSpot.Client.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.HubSpot.Client.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class HubSpotClientUtilTests : HostedUnitTest
{
    private readonly IHubSpotClientUtil _httpclient;

    public HubSpotClientUtilTests(Host host) : base(host)
    {
        _httpclient = Resolve<IHubSpotClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
