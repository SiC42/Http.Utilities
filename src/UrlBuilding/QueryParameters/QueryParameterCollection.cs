using System.Collections;

namespace Sic.Http.Utilities.UrlBuilding.QueryParameters;
internal class QueryParameterCollection : IReadOnlyQueryParameterCollection
{
  private readonly Dictionary<string, List<QueryParameter>> _dictionary;

  public int Count => _dictionary.Values.SelectMany(v => v).Count();

  public QueryParameterCollection()
  {
    _dictionary = [];
  }

  public QueryParameterCollection(QueryParameterCollection queryParameters)
  {
    _dictionary = queryParameters._dictionary
      .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
  }

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

  public void AddRange(IEnumerable<QueryParameter> parameters)
  {
    foreach (var parameter in parameters)
    {
      Add(parameter);
    }
  }

  public string ToQueryString()
  {
    return string.Join("&", _dictionary.Values.SelectMany(v => v));
  }

  public IEnumerator<QueryParameter> GetEnumerator()
  {
    return _dictionary.Values
    .SelectMany(q => q)
    .GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}