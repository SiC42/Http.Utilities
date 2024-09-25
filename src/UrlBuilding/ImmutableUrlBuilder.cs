
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

  private Dictionary<string, string> _pathDict;
  public IReadOnlyDictionary<string, string> PathValues => _pathDict.AsReadOnly();

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
    _pathDict = [];
  }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  private ImmutableUrlBuilder()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  {
  }

  public ImmutableUrlBuilder SetHost(string host)
  {
    BuilderHelper.CheckHost(host);

    return new ImmutableUrlBuilder()
    {
      Scheme = Scheme,
      Host = host,
      Port = Port,
      _segments = _segments,
      _pathDict = new(_pathDict),
      _queryParameters = new(_queryParameters)
    };
  }

  public ImmutableUrlBuilder SetPort(int port)
  {
    BuilderHelper.CheckPort(port);

    return new ImmutableUrlBuilder()
    {
      Scheme = Scheme,
      Host = Host,
      Port = port,
      _segments = _segments,
      _pathDict = new(_pathDict),
      _queryParameters = new(_queryParameters)
    };
  }


  public ImmutableUrlBuilder AddPath(string pathToAdd)
  {
    List<string> newSegmentList = [.. _segments, .. BuilderHelper.GetSegments(pathToAdd)];

    return new ImmutableUrlBuilder()
    {
      Scheme = Scheme,
      Host = Host,
      Port = Port,
      _segments = newSegmentList,
      _pathDict = new(_pathDict),
      _queryParameters = new(_queryParameters)
    };
  }



  public ImmutableUrlBuilder AddQuery(string key, params object[] values)
  {
    var newQueryParameters = new QueryParameterCollection(_queryParameters);

    if (values.Length == 0)
    {
      newQueryParameters.Add(new QueryParameter(key));
    }
    newQueryParameters.AddRange(values.Select(v => new QueryParameter(key, v)));

    return new ImmutableUrlBuilder()
    {
      Scheme = Scheme,
      Host = Host,
      Port = Port,
      _segments = _segments,
      _pathDict = new(_pathDict),
      _queryParameters = newQueryParameters
    };
  }

  public ImmutableUrlBuilder WithPathValue(string pathKey, object value)
  {
    var newPathDict = new Dictionary<string, string>(_pathDict)
    {
      [pathKey] = value!.ToString()!
    };
    return new ImmutableUrlBuilder()
    {
      Scheme = Scheme,
      Host = Host,
      Port = Port,
      _segments = _segments,
      _pathDict = newPathDict,
      _queryParameters = _queryParameters
    };
  }

  public Uri Build()
  {
    return BuilderHelper.Build(this);
  }
}
