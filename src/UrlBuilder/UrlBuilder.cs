
using System.Security.Cryptography.X509Certificates;

namespace Sic.Http;

public class UrlBuilder
{
  public string Scheme { get; private set; } // todo: validation for http(s)

  private List<string> _segments;
  public IReadOnlyList<string> Segments => _segments.AsReadOnly();
  public string Host { get; private set; }

  private readonly QueryParameters _queryParameters;


  public int Port { get; private set; }

  public UrlBuilder(string url)
  : this(new Uri(url))
  {
  }

  public UrlBuilder(Uri url)
  {
    // todo: checks
    Scheme = url.Scheme;
    _segments = url.Segments
    .Select(str => str.TrimEnd('/'))
    .Where(str => str != string.Empty)
    .ToList();
    Host = url.Host;
    _queryParameters = [.. url.Query.Split("&").Select(queryParam => new QueryParameter(queryParam))];
    Port = url.Port;
  }




  public UrlBuilder AddPath(string pathToAdd)
  {
    int index = 0;
    while (true)
    {
      int previousIndex = index;
      index = pathToAdd.IndexOf('/', index);
      if (index is -1)
      {
        _segments.Add(pathToAdd[previousIndex..]);
        break;
      }
      else if (index is not 0) // eg for "/path/to"
      {
        _segments.Add(pathToAdd[previousIndex..index]);
      }
      index++;
    }

    return this;
  }

  public UrlBuilder AddQuery(string key, params string[] values)
  {
    if (values.Length == 0)
    {
      _queryParameters.Add(new QueryParameter(key));
    }
    _queryParameters.AddRange(values.Select(v => new QueryParameter(key, v)));
    return this;
  }

  public Uri Build()
  {
    var builder = new UriBuilder(Scheme, Host, Port, string.Join("/", _segments));
    builder.Query = _queryParameters.ToQueryString();
    return builder.Uri;
  }
}
