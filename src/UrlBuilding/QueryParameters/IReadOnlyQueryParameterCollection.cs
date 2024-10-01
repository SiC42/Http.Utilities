namespace Sic.Http.Utilities.UrlBuilding.QueryParameters;

/// <summary>
/// Provides a collection of query parameters.
/// </summary>
public interface IReadOnlyQueryParameterCollection : IReadOnlyCollection<QueryParameter>
{
  /// <summary>
  /// Returns the query string representation of this collection.
  /// </summary>
  /// <returns>The query string representation of this collection.</returns>
  public string ToQueryString();
}