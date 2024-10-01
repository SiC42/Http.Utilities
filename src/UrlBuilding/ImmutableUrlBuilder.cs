
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Sic.Http.Utilities.UrlBuilding.QueryParameters;

namespace Sic.Http.Utilities.UrlBuilding;

public class ImmutableUrlBuilder : IUrlBuilder<ImmutableUrlBuilder>
{
  public string Scheme { get; private set; } // todo: validation for http(s)

  private List<string> _segments;
  public IReadOnlyList<string> Segments => _segments;
  public string Host { get; private set; }

  private QueryParameterCollection _queryParameters;
  public IReadOnlyQueryParameterCollection QueryParameters => _queryParameters;

  private Dictionary<string, string> _pathValues;
  public IReadOnlyDictionary<string, string> PathValues => _pathValues.AsReadOnly();

  public int Port { get; private set; }


  public ImmutableUrlBuilder(string url)
: this(new Uri(url))
  {
  }

  public ImmutableUrlBuilder(Uri url)
  {
    BuilderHelper.CheckScheme(url);

    Scheme = url.Scheme;
    _segments = BuilderHelper.ToCleanSegments(url.Segments);
    Host = url.Host;
    _queryParameters = BuilderHelper.ToParameterCollection(url.Query);
    Port = url.Port;
    _pathValues = [];
  }

  internal ImmutableUrlBuilder(
    string scheme,
    IReadOnlyList<string> segments,
    string host,
    int port,
    IReadOnlyDictionary<string, string> pathValues,
    IReadOnlyQueryParameterCollection queryParameters)
  {
    Scheme = scheme;
    Host = host;
    Port = port;
    _segments = [.. segments];
    _pathValues = pathValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    _queryParameters = [.. queryParameters];
  }

  public ImmutableUrlBuilder SetScheme(string scheme)
  {
    BuilderHelper.CheckSchemeType(scheme);

    return new ImmutableUrlBuilder(
      scheme: scheme,
      host: Host,
      port: Port,
      segments: _segments,
      pathValues: PathValues,
      queryParameters: QueryParameters
    );
  }

  public ImmutableUrlBuilder SetHost(string host)
  {
    BuilderHelper.CheckHost(host);

    return new ImmutableUrlBuilder(
      scheme: Scheme,
      host: host,
      port: Port,
      segments: _segments,
      pathValues: PathValues,
      queryParameters: QueryParameters
    );
  }

  public ImmutableUrlBuilder SetPort(int port)
  {
    BuilderHelper.CheckPort(port);

    return new ImmutableUrlBuilder(
      scheme: Scheme,
      host: Host,
      port: port,
      segments: _segments,
      pathValues: PathValues,
      queryParameters: QueryParameters
    );
  }


  public ImmutableUrlBuilder AddPath(string pathToAdd)
  {
    List<string> newSegmentList = [.. _segments, .. BuilderHelper.GetSegments(pathToAdd)];

    return new ImmutableUrlBuilder(
      scheme: Scheme,
      host: Host,
      port: Port,
      segments: newSegmentList,
      pathValues: PathValues,
      queryParameters: QueryParameters
    );
  }



  public ImmutableUrlBuilder AddQuery(string key, params object[] values)
  {
    var newQueryParameters = new QueryParameterCollection(_queryParameters);

    if (values.Length == 0)
    {
      newQueryParameters.Add(new QueryParameter(key));
    }
    newQueryParameters.AddRange(values.Select(v => new QueryParameter(key, v)));

    return new ImmutableUrlBuilder(
      scheme: Scheme,
      host: Host,
      port: Port,
      segments: Segments,
      pathValues: PathValues,
      queryParameters: newQueryParameters
    );
  }

  public ImmutableUrlBuilder WithPathValue(string pathKey, object value)
  {
    var newPathDict = new Dictionary<string, string>(_pathValues)
    {
      [pathKey] = value!.ToString()!
    };

    return new ImmutableUrlBuilder(
      scheme: Scheme,
      host: Host,
      port: Port,
      segments: Segments,
      pathValues: newPathDict,
      queryParameters: QueryParameters
    );
  }

  public Uri Build()
  {
    return BuilderHelper.Build(this);
  }
}
