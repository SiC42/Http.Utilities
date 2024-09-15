namespace Sic.Http.Tests;

public class UrlBuilderTests
{

    [Test]
    [TestCase("http://www.test.de/this/is?a=big&test")]
    [TestCase("https://www.test.de/")]
    public void Constructor_WhenGivenUrlString_BuildsUrl(string urlStr)
    {
        // Arrange
        var builder = new UrlBuilder(urlStr);

        // Act
        var url = builder.Build();

        // Assert
        Assert.That(url, Is.EqualTo(new Uri(urlStr)));
    }

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


}