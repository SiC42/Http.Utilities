using System.Text.RegularExpressions;
using Sic.Http.Utilities.UrlBuilding;
using Sic.Http.Utilities.UrlBuilding.QueryParameters;

internal static partial class BuilderHelper
{

  public static void CheckScheme(Uri url)
  {
    if (url.Scheme is not ("http" or "https"))
    {
      throw new NotSupportedException("Given URI was not an URL (type http or https). Other types are currently not supported.");
    }
  }

  public static void CheckSchemeType(string scheme)
  {
    if (scheme is not ("http" or "https"))
    {
      throw new NotSupportedException("Given Scheme was not of type http or https. Other types are currently not supported.");
    }
  }

  public static List<string> ToCleanSegments(IReadOnlyCollection<string> segments)
  {
    return segments
    .Select(str => str.TrimEnd('/'))
    .Where(str => str != string.Empty)
    .ToList();
  }

  public static QueryParameterCollection ToParameterCollection(string query)
  {
    return query is ""
          ? []
          : [.. query.Split("&").Select(queryParam => new QueryParameter(queryParam))];
  }

  public static void CheckHost(string host)
  {
    if (Uri.CheckHostName(host) is UriHostNameType.Unknown)
    {
      throw new ArgumentOutOfRangeException(nameof(host), host, "The given string is not a valid host name");
    }
  }
  public static void CheckPort(int port)
  {
    if (port < 0 || port > 65_535)
    {
      throw new ArgumentOutOfRangeException(nameof(port), port, "Port value must be between 0 and 65535.");
    }
  }

  public static IEnumerable<string> GetSegments(string pathToAdd)
  {
    int index = 0;
    while (true)
    {
      int previousIndex = index;
      index = pathToAdd.IndexOf('/', index);
      if (index is -1)
      {
        yield return pathToAdd[previousIndex..];
        break;
      }
      else if (index is not 0) // eg for "/path/to"
      {
        yield return pathToAdd[previousIndex..index];
      }
      index++;
    }
  }

  public static Uri Build(IReadOnlyUrlBuilder builder)
  {
    var uriBuilder = new UriBuilder(builder.Scheme, builder.Host, builder.Port, string.Join("/", builder.Segments.Select(ReplaceVariables(builder.PathValues))))
    {
      Query = builder.QueryParameters.ToQueryString()
    };
    return uriBuilder.Uri;
  }

  private static Func<string, string> ReplaceVariables(IReadOnlyDictionary<string, string> valuePathDict)
  {
    return pathSegment =>
    {
      // due to URL encoding, the symbol "{" and "}" might be tranlated to "%7B" & "%7D"
      // for now we try to replace those values with the original {}
      pathSegment = pathSegment
        .Replace("%7B", "{")
        .Replace("%7D", "}");
      return VariablePattern().Replace(pathSegment, ReplaceKeyWithValue);
    };

    string ReplaceKeyWithValue(Match m) => valuePathDict[m.Value[1..^1]];
  }

  [GeneratedRegex(@"\{\w+\}")]
  private static partial Regex VariablePattern();

}