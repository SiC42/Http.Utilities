using Sic.Http.Utilities.UrlBuilding.QueryParameters;

namespace Sic.Http.Utilities.UrlBuilding;
/// <summary>
/// This interface existst solely to maintain parrity between the UrlBuilders here.
/// </summary>
internal interface IUrlBuilder<TUrlBuilder> : IReadOnlyUrlBuilder
where TUrlBuilder : IUrlBuilder<TUrlBuilder>
{

  public TUrlBuilder AddPath(string pathToAdd);
  public TUrlBuilder AddQuery(string key, params object[] values);

  public TUrlBuilder WithPathValue(string pathKey, object value);

  public Uri Build();
  public TUrlBuilder SetPort(int port);

  public TUrlBuilder SetHost(string host);
}

public interface IReadOnlyUrlBuilder
{
  public string Scheme { get; }

  public IReadOnlyList<string> Segments { get; }
  public string Host { get; }

  public int Port { get; }

  public IReadOnlyDictionary<string, string> PathValues { get; }


  public IReadOnlyQueryParameterCollection QueryParameters { get; }
}