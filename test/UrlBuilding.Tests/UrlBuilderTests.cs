using System.Net;
using Sic.Http.Utilities.UrlBuilding;
namespace Sic.Http.Utilities.UrlBuilding.Tests;

internal class UrlBuilderTest : UrlBuilderTestBase<UrlBuilder>
{
    protected override UrlBuilder InitBuilder(string urlStr = "http://test.de")
    {
        return new UrlBuilder(urlStr);
    }
}