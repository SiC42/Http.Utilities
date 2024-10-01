using System.Collections;

namespace Sic.Http.Utilities.UrlBuilding.QueryParameters;
internal class QueryParameterCollection : IReadOnlyQueryParameterCollection
{
  private readonly Dictionary<string, List<QueryParameter>> _dictionary;

  /// <summary>
  /// Returns the number of query parameters in this collection.
  /// </summary>
  /// <remarks>Due note that a key mad have multiple values.</remarks>
  public int Count => _dictionary.Values.SelectMany(v => v).Count();

  /// <summary>
  /// Initializes a new instance of <see cref="QueryParameterCollection"/>.
  /// </summary>
  public QueryParameterCollection()
  {
    _dictionary = [];
  }

  /// <summary>
  /// Initializes a new instance of <see cref="QueryParameterCollection"/>.
  /// </summary>
  public QueryParameterCollection(QueryParameterCollection queryParameters)
  {
    _dictionary = queryParameters._dictionary
      .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
  }

  /// <summary>
  /// Adds the given query parameter to the collection.
  /// </summary>
  /// <param name="parameter">The parameter to add to the collection.</param>
  public void Add(QueryParameter parameter)
  {
    if (_dictionary.TryGetValue(parameter.Key, out var parameters))
    {
      parameters.Add(parameter);
    }
    else
    {
      _dictionary[parameter.Key] = [parameter];
    }
  }

  /// <summary>
  /// Adds the given query parameters to the collection.
  /// </summary>
  /// <param name="parameters">The parameters to add to the collection.</param>
  public void AddRange(IEnumerable<QueryParameter> parameters)
  {
    foreach (var parameter in parameters)
    {
      Add(parameter);
    }
  }

  /// <inheritdoc />
  public string ToQueryString()
  {
    return string.Join("&", _dictionary.Values.SelectMany(v => v));
  }

  /// <inheritdoc />
  public IEnumerator<QueryParameter> GetEnumerator()
  {
    return _dictionary.Values
    .SelectMany(q => q)
    .GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}