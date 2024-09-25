namespace Sic.Http.Utilities.UrlBuilding.QueryParameters;
public interface IReadOnlyQueryParameterCollection : IReadOnlyCollection<QueryParameter>
{

  public string ToQueryString();
}