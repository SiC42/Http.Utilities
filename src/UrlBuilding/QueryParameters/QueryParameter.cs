namespace Sic.Http.Utilities.UrlBuilding;
/// <summary>
/// Represents a query parameter, i.e. a key with its value (if any).
/// </summary>
public readonly struct QueryParameter
{
  private readonly string queryParam;

  /// <summary>
  /// Initializes a new instance of <see cref="QueryParameter"/>.
  /// </summary>
  public QueryParameter(string queryParam)
  {
    var split = queryParam.IndexOf("=");
    if (split is -1)
    {
      Key = queryParam;
    }
    else
    {
      Key = queryParam[0..split];
      Value = queryParam[Math.Min(split + 1, queryParam.Length - 1)..^1];
    }

    this.queryParam = queryParam;
  }

  /// <summary>
  /// Initializes a new instance of <see cref="QueryParameter"/>.
  /// </summary>
  public QueryParameter(string key, object value)
  {
    Key = key;
    Value = value.ToString();
    queryParam = $"{key}={value}";
  }

  /// <summary>
  /// Key of the query parameter.
  /// </summary>
  public string Key { get; }

  /// <summary>
  /// Value of the query parameter (if any)
  /// </summary>
  public string? Value { get; }

  /// <summary>
  /// Returns the query parameter as a URL string representation (unencoded).
  /// </summary>
  /// <returns>The query parameter as a URL string representation (unencoded).</returns>
  /// <example>Queryparameter "mykey" with value "myvalue" becomes "mykey=myvalue"</example>
  public override string ToString()
  {
    return queryParam;
  }
}