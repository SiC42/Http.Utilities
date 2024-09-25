using System.Net;
using Sic.Http.Utilities.UrlBuilding;
namespace Sic.Http.Utilities.UrlBuilding.Tests;

internal class ImmutableUrlBuilderTest : UrlBuilderTestBase<ImmutableUrlBuilder>
{
    protected override ImmutableUrlBuilder InitBuilder(string urlStr = "http://test.de")
    {
        return new ImmutableUrlBuilder(urlStr);
    }
}