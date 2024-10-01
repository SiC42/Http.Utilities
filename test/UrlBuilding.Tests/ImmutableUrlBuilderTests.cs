using System.Net;
using Sic.Http.Utilities.UrlBuilding;
namespace Sic.Http.Utilities.UrlBuilding.Tests;

internal class ImmutableUrlBuilderTest : UrlBuilderTestBase<ImmutableUrlBuilder>
{
    protected override ImmutableUrlBuilder InitBuilder(string urlStr = "http://test.de")
    {
        return new ImmutableUrlBuilder(urlStr);
    }


    [Test]
    public void WhenAllMethodsCalled_OriginalBuilderNotChanged()
    {
        var initalUrl = new Uri("http://test.de/{Hello}?base=setup");
        var builder = new ImmutableUrlBuilder(initalUrl)
            .WithPathValue("Hello", "hello");

        var finalUrl = builder
            .SetScheme("https")
            .SetHost("new-host.com")
            .SetPort(4242)
            .WithPathValue("Hello", "hi")
            .AddPath("world")
            .AddPath("word")
            .AddQuery("this", "is")
            .AddQuery("a", "test")
            .Build();

        Assert.Multiple(() =>
        {
            Assert.That(builder.Build().ToString(), Is.EqualTo("http://test.de/hello?base=setup"));
            Assert.That(finalUrl.ToString(), Is.EqualTo("https://new-host.com:4242/hi/world/word?base=setup&this=is&a=test"));
        });
    }
}