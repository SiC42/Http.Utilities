using System.Collections;

internal class QueryParameters : IEnumerable<QueryParameter>
{
  private readonly Dictionary<string, List<QueryParameter>> _dictionary = new();

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