
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Sic.Http.Utilities.UrlBuilding.QueryParameters;

namespace Sic.Http.Utilities.UrlBuilding;

/// <summary>
/// An immutable version of the <see cref="UrlBuilder"/>.
/// This means that calling methods on this builder do provide an entirely new builder with the expected changes,
/// but do not modify the internal state of this instance.
/// </summary>
public class ImmutableUrlBuilder : IUrlBuilder<ImmutableUrlBuilder>
{
  /// <inheritdoc />
  public string Scheme { get; private set; } // todo: validation for http(s)

  /// <inheritdoc />
  public string Host { get; private set; }

  /// <inheritdoc />
  public int Port { get; private set; }

  private List<string> _segments;
  /// <inheritdoc />
  public IReadOnlyList<string> Segments => _segments;

  private Dictionary<string, string> _pathValues;
  /// <inheritdoc />
  public IReadOnlyDictionary<string, string> PathValues => _pathValues.AsReadOnly();

  private QueryParameterCollection _queryParameters;
  /// <inheritdoc />
  public IReadOnlyQueryParameterCollection QueryParameters => _queryParameters;

  /// <summary>
  /// Initializes a new instance of <see cref="ImmutableUrlBuilder"/>.
  /// </summary>
  public ImmutableUrlBuilder(string url)
: this(new Uri(url))
  {
  }

  /// <summary>
  /// Initializes a new instance of <see cref="ImmutableUrlBuilder"/>.
  /// </summary>
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

  /// <summary>
  /// Initializes a new instance of <see cref="ImmutableUrlBuilder"/>.
  /// </summary>
  /// <remarks>Used internally to provide a way to deep copy the builder.</remarks>
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

  /// <inheritdoc />
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

  /// <inheritdoc />
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

  /// <inheritdoc />
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

  /// <inheritdoc />
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

  /// <inheritdoc />
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

  /// <inheritdoc />
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

  /// <inheritdoc />
  public Uri Build()
  {
    return BuilderHelper.Build(this);
  }
}
