using Soenneker.HubSpot.Client.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.HubSpot.Client.Tests;

[Collection("Collection")]
public sealed class HubSpotClientUtilTests : FixturedUnitTest
{
    private readonly IHubSpotClientUtil _httpclient;

    public HubSpotClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _httpclient = Resolve<IHubSpotClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
