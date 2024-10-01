
using System.Text.RegularExpressions;
using Sic.Http.Utilities.UrlBuilding.QueryParameters;

namespace Sic.Http.Utilities.UrlBuilding;

/// <summary>
/// Simple buidler for building URLs.
/// </summary>
public class UrlBuilder : IUrlBuilder<UrlBuilder>, IReadOnlyUrlBuilder
{

  /// <inheritdoc />
  public string Scheme { get; private set; }

  /// <inheritdoc />
  public int Port { get; private set; }

  /// <inheritdoc />
  public string Host { get; private set; }

  private readonly List<string> _segments;

  /// <inheritdoc />
  public IReadOnlyList<string> Segments => _segments.AsReadOnly();
  private readonly Dictionary<string, string> _pathValues;

  /// <inheritdoc />
  public IReadOnlyDictionary<string, string> PathValues => _pathValues.AsReadOnly();

  private readonly QueryParameterCollection _queryParameters;

  /// <inheritdoc />
  public IReadOnlyQueryParameterCollection QueryParameters => _queryParameters;

  /// <summary>
  /// Initializes a new instance of <see cref="UrlBuilder"/>.
  /// </summary>
  public UrlBuilder(string url)
: this(new Uri(url))
  {
  }

  /// <summary>
  /// Initializes a new instance of <see cref="UrlBuilder"/>.
  /// </summary>
  public UrlBuilder(Uri url)
  {
    BuilderHelper.CheckScheme(url);

    Scheme = url.Scheme;
    _segments = BuilderHelper.ToCleanSegments(url.Segments);
    Host = url.Host;
    _queryParameters = BuilderHelper.ToParameterCollection(url.Query);
    Port = url.Port;
    _pathValues = [];
  }

  /// <inheritdoc />
  public UrlBuilder SetScheme(string scheme)
  {
    BuilderHelper.CheckSchemeType(scheme);
    Scheme = scheme;

    return this;
  }

  /// <inheritdoc />
  public UrlBuilder SetHost(string host)
  {
    BuilderHelper.CheckHost(host);
    Host = host;

    return this;
  }

  /// <inheritdoc />
  public UrlBuilder SetPort(int port)
  {
    BuilderHelper.CheckPort(port);
    Port = port;
    return this;
  }

  /// <inheritdoc />
  public UrlBuilder AddPath(string pathToAdd)
  {
    _segments.AddRange(BuilderHelper.GetSegments(pathToAdd));
    return this;
  }

  /// <inheritdoc />
  public UrlBuilder WithPathValue(string pathKey, object value)
  {
    _pathValues[pathKey] = value!.ToString()!;
    return this;
  }

  /// <inheritdoc />
  public UrlBuilder AddQuery(string key, params object[] values)
  {
    if (values.Length == 0)
    {
      _queryParameters.Add(new QueryParameter(key));
    }
    _queryParameters.AddRange(values.Select(v => new QueryParameter(key, v)));
    return this;
  }

  /// <inheritdoc />
  public Uri Build()
  {
    return BuilderHelper.Build(this);
  }

  /// <summary>
  /// Returns an immutable version of this <see cref="UrlBuilder"/>.
  /// This can be used as a base for further modifying the URL without changing the state of the instance of this <see cref="UrlBuilder"/>
  /// </summary>
  /// <returns>An immutable version of this <see cref="UrlBuilder"/>.</returns>
  /// <seealso cref="ImmutableUrlBuilder"/>
  public ImmutableUrlBuilder AsImmutable()
  {
    return new ImmutableUrlBuilder(
      scheme: Scheme,
      host: Host,
      port: Port,
      segments: Segments,
      pathValues: PathValues,
      queryParameters: QueryParameters
    );
  }

}
