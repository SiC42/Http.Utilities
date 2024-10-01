using Sic.Http.Utilities.UrlBuilding.QueryParameters;

namespace Sic.Http.Utilities.UrlBuilding;
/// <summary>
/// Represents a generic interface for building URLs with an immutable pattern. 
/// This interface exists to maintain consistency between different URL builder implementations 
/// by enforcing a common API.
/// </summary>
/// <typeparam name="TUrlBuilder">
/// The specific type of the URL builder implementing this interface. This ensures that each 
/// method returns the correct type of builder, allowing for method chaining in a fluent style.
/// </typeparam>
internal interface IUrlBuilder<TUrlBuilder> : IReadOnlyUrlBuilder
where TUrlBuilder : IUrlBuilder<TUrlBuilder>
{
  /// <summary>
  /// Sets the scheme (i.e., "http", "https") for the URL.
  /// </summary>
  /// <param name="scheme">The URL scheme to set.</param>
  /// <returns>The current builder instance with the updated scheme.</returns>  
  public TUrlBuilder SetScheme(string scheme);

  /// <summary>
  /// Sets the host (e.g., "example.com") for the URL.
  /// </summary>
  /// <param name="host">The host name or IP address to set.</param>
  /// <returns>The current builder instance with the updated host.</returns>
  public TUrlBuilder SetHost(string host);

  /// <summary>
  /// Sets the port for the URL (e.g., 80 for HTTP, 443 for HTTPS).
  /// </summary>
  /// <param name="port">The port number to set.</param>
  /// <returns>The current builder instance with the updated port.</returns>
  public TUrlBuilder SetPort(int port);

  /// <summary>
  /// Adds a path segment to the URL.
  /// </summary>
  /// <param name="pathToAdd">The path segment to append to the URL.</param>
  /// <returns>The current builder instance with the added path segment.</returns>
  public TUrlBuilder AddPath(string pathToAdd);

  /// <summary>
  /// Adds or replaces a path value for a named placeholder in the URL.
  /// Typically used for substituting values in templated paths (e.g., "/users/{userId}").
  /// </summary>
  /// <param name="pathKey">The name of the path placeholder (e.g., "userId").</param>
  /// <param name="value">The value to substitute for the placeholder.</param>
  /// <returns>The current builder instance with the updated path value.</returns>
  /// <remarks>Due not that the given value is invoked with its <see cref="object.ToString()"/>-method.</remarks>
  public TUrlBuilder WithPathValue(string pathKey, object value);

  /// <summary>
  /// Adds a query parameter to the URL. 
  /// Multiple values for the same key are supported, and will be serialized as key=value1&amp;key=value2.
  /// </summary>
  /// <param name="key">The query parameter key.</param>
  /// <param name="values">The values for the query parameter. Can include multiple values.</param>
  /// <returns>The current builder instance with the added query parameter(s).</returns>
  public TUrlBuilder AddQuery(string key, params object[] values);

  /// <summary>
  /// Builds and returns the constructed <see cref="Uri"/> from the current URL components.
  /// </summary>
  /// <returns>A new <see cref="Uri"/> instance representing the built URL.</returns>
  public Uri Build();

}

/// <summary>
/// Represents a read-only interface for accessing the components of a URL.
/// This interface is designed to provide a structured way to inspect various parts of a URL, 
/// such as the scheme, host, port, path segments, path values, and query parameters, 
/// without allowing modifications.
/// </summary>
public interface IReadOnlyUrlBuilder
{
  /// <summary>
  /// Gets the scheme (protocol) of the URL (e.g., "http", "https").
  /// </summary>
  /// <value>
  /// The URL scheme as a string.
  /// </value>
  public string Scheme { get; }

  /// <summary>
  /// Gets the host name or IP address of the URL (e.g., "example.com", "localhost").
  /// </summary>
  /// <value>
  /// The host component of the URL as a string.
  /// </value>
  public string Host { get; }

  /// <summary>
  /// Gets the port number used by the URL.
  /// </summary>
  /// <value>
  /// The port number as an integer.
  /// </value>
  public int Port { get; }

  /// <summary>
  /// Gets a list of path segments that make up the URL's path.
  /// Each segment represents a part of the URL path (e.g., "/users/123" 
  /// would have "users" and "123" as segments).
  /// </summary>
  /// <value>
  /// A read-only list of path segments in the URL.
  /// </value>
  public IReadOnlyList<string> Segments { get; }


  /// <summary>
  /// Gets a read-only dictionary of path values. 
  /// Path values represent key-value pairs that are used to replace 
  /// templated placeholders in the URL's path (e.g., "/users/{userId}" with "userId" 
  /// as a key and its corresponding value).
  /// </summary>
  /// <value>
  /// A read-only dictionary where the keys are path placeholders and the values 
  /// are the replacements used in the final URL.
  /// </value>
  public IReadOnlyDictionary<string, string> PathValues { get; }

  /// <summary>
  /// Gets the collection of query parameters associated with the URL.
  /// Query parameters are key-value pairs that appear after the "?" in the URL 
  /// (e.g., "?key1=value1&amp;key2=value2").
  /// </summary>
  /// <value>
  /// A read-only collection of query parameters.
  /// </value>
  public IReadOnlyQueryParameterCollection QueryParameters { get; }
}