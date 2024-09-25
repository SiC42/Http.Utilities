using System.Net;
using Sic.Http.Utilities.UrlBuilding;
namespace Sic.Http.Utilities.UrlBuilding.Tests;

internal abstract class UrlBuilderTestBase<TUrlBuilder> where TUrlBuilder : IUrlBuilder<TUrlBuilder>
{

    [Test]
    [TestCase("http://www.test.de/this/is?a=big&test")]
    [TestCase("https://www.test2.com/")]
    public void Constructor_WhenGivenUrlString_BuildsUrl(string urlStr)
    {
        // Arrange
        var builder = InitBuilder(urlStr);

        // Act
        var url = builder.Build();

        // Assert
        Assert.That(url, Is.EqualTo(new Uri(urlStr)));
    }

    protected abstract TUrlBuilder InitBuilder(string urlStr = "http://test.de");

    [Test]
    [TestCase("http://www.test.de/this/is?a=big&test")]
    [TestCase("https://www.test.de/")]
    public void Constructor_WhenGivenUrl_BuildsUrl(string urlStr)
    {
        // Arrange
        var actualUri = new Uri(urlStr);
        var builder = new UrlBuilder(actualUri);

        // Act
        var url = builder.Build();

        // Assert
        Assert.That(url, Is.EqualTo(actualUri));
    }

    [Test]
    [TestCase("http", 80)]
    [TestCase("https", 443)]
    public void Constructor_WhenNoPortInStringUrl_PortIsDeterminedByProtocol(string scheme, int port)
    {
        //Arrange & Act
        var builder = InitBuilder($"{scheme}://test.de");

        // Assert
        Assert.That(builder.Port, Is.EqualTo(port));
        Assert.That(builder.Build().Port, Is.EqualTo(port));
    }

    [Test]
    public void Constructor_WhenPortInStringUrl_PortIsSet([Random(0, 65_535, 20)] int port)
    {
        //Arrange & Act
        var builder = InitBuilder($"https://test.de:{port}/more/bla");

        // Assert
        Assert.That(builder.Port, Is.EqualTo(port));
        Assert.That(builder.Build().Port, Is.EqualTo(port));
    }

    [Test]
    [TestCase("localhost")]
    [TestCase("test.com")]
    [TestCase("web-site.te.st")]
    [TestCase("11.1.1.11")] // IPv4
    public void SetHost_SetsHost(string host)
    {
        //Arrange
        var builder = InitBuilder();

        // Act
        builder = builder.SetHost(host);

        // Assert
        Assert.That(builder.Host, Is.EqualTo(host));
        Assert.That(builder.Build().Host, Is.EqualTo(host));
    }

    //Test for IPv6. We need extra test as .NET optimizes the representation of the address
    [Test]
    public void SetHost_WithIpv6_SetsHost()
    {
        //Arrange
        const string ip = "7b7c:2e5b:0ccf:f230:960d:12d6:3342:331a";
        var builder = InitBuilder();

        // Act
        builder = builder.SetHost(ip);

        // Assert
        var parsedIp = IPAddress.Parse(ip);
        Assert.That(IPAddress.Parse(builder.Host), Is.EqualTo(parsedIp));
        Assert.That(IPAddress.Parse(builder.Build().Host), Is.EqualTo(parsedIp));
    }


    [Test]
    public void SetPort_SetsPort([Random(0, 65_535, 20)] int port)
    {
        //Arrange
        var builder = InitBuilder();

        // Act
        builder = builder.SetPort(port);

        // Assert
        Assert.That(builder.Port, Is.EqualTo(port));
        Assert.That(builder.Build().Port, Is.EqualTo(port));
    }


    [Test]
    [TestCase("http://www.test.de/this/is?a=big&test", "some/path", "http://www.test.de/this/is/some/path?a=big&test")]
    [TestCase("http://www.test.de/this/is?a=big&test", "/some/path/", "http://www.test.de/this/is/some/path/?a=big&test")]
    [TestCase("https://www.test.de/", "some/path/", "https://www.test.de/some/path/")]
    [TestCase("https://www.test.de/", "/some/path", "https://www.test.de/some/path")]
    public void AddPath_AddedInBuild(string initalUrl, string pathToAdd, string actualUri)
    {
        // Arrange
        var builder = new UrlBuilder(initalUrl);

        // Act
        var url = builder
        .AddPath(pathToAdd)
        .Build();

        // Assert
        Assert.That(url, Is.EqualTo(new Uri(actualUri)));
    }

    [Test]
    public void WithPathValue_AddsPathValue()
    {
        var builder = InitBuilder();
        var url = builder.AddPath("{First}")
            .AddPath("{Second}")
            .WithPathValue("Second", "World")
            .WithPathValue("First", "Hello")
            .Build();

        Assert.That(url.PathAndQuery, Is.EqualTo("/Hello/World"));
    }

    [Test]
    public void AddQuery_WhenUsedTwice_ValueAdded()
    {
        // Arrange
        var builder = InitBuilder();

        // Act
        var url = builder.AddQuery("test", "one")
            .AddQuery("test", "two")
            .Build();

        // Assert
        Assert.That(url.Query, Is.EqualTo("?test=one&test=two"));
    }

    [Test]
    public void AddQuery_InFluent_ProducesExpectedQuery()
    {
        // Arrange
        var builder = new UrlBuilder("http://test.de?oh=my");

        // Act
        var url = builder.AddQuery("hello", "world")
            .AddQuery("this", "is")
            .AddQuery("a", "small", "tiny")
            .AddQuery("test")
            .Build();

        // Assert
        Assert.That(url.Query, Is.EqualTo("?oh=my&hello=world&this=is&a=small&a=tiny&test"));
    }



}