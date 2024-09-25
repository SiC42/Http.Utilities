
using System.Text.RegularExpressions;
using InterfaceGenerator;
using Sic.Http.Utilities.UrlBuilding.QueryParameters;

namespace Sic.Http.Utilities.UrlBuilding;

public partial class UrlBuilder : IUrlBuilder<UrlBuilder>, IReadOnlyUrlBuilder
{
  public string Scheme { get; private set; } // todo: validation for http(s)

  private List<string> _segments;
  public IReadOnlyList<string> Segments => _segments.AsReadOnly();
  public string Host { get; private set; }

  private readonly QueryParameterCollection _queryParameters;
  public IReadOnlyQueryParameterCollection QueryParameters => _queryParameters;
  private readonly Dictionary<string, string> _pathDict;
  public IReadOnlyDictionary<string, string> PathValues => _pathDict.AsReadOnly();

  public int Port { get; private set; }


  public UrlBuilder(string url)
: this(new Uri(url))
  {
  }

  public UrlBuilder(Uri url)
  {
    BuilderHelper.CheckScheme(url);

    Scheme = url.Scheme;
    _segments = BuilderHelper.ToCleanSegments(url.Segments);
    Host = url.Host;
    _queryParameters = BuilderHelper.ToParameterCollection(url.Query);
    Port = url.Port;
    _pathDict = [];
  }




  public UrlBuilder AddPath(string pathToAdd)
  {
    _segments.AddRange(BuilderHelper.GetSegments(pathToAdd));
    return this;
  }

  public UrlBuilder AddQuery(string key, params object[] values)
  {
    if (values.Length == 0)
    {
      _queryParameters.Add(new QueryParameter(key));
    }
    _queryParameters.AddRange(values.Select(v => new QueryParameter(key, v)));
    return this;
  }

  public UrlBuilder WithPathValue(string pathKey, object value)
  {
    _pathDict[pathKey] = value!.ToString()!;
    return this;
  }

  public UrlBuilder SetPort(int port)
  {
    BuilderHelper.CheckPort(port);
    Port = port;
    return this;
  }

  public UrlBuilder SetHost(string host)
  {
    BuilderHelper.CheckHost(host);
    Host = host;

    return this;
  }

  public Uri Build()
  {
    return BuilderHelper.Build(this);
  }

}
