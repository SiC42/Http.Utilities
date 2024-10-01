# Http.Utilities
**This is a work in progress**
This repository provides packages to handle various aspects of HTTP calls, especially with `HttpClient`.


## Url.Building
The package provides a way to handle building URLs.

### Simple URL builder
This implementation can be used to build URLs on the fly.

#### Examples
Basic Usage

```csharp
var builder = new UrlBuilder("http://mydomain.com/test?base=query")
            .SetScheme("https")
            .SetPort(4242)
            .AddPath("hello/big")
            .AddPath("world")
            .AddQuery("this", "is")
            .AddQuery("a", "test");

Uri url = builder.Build(); // https://mydomain.com:4242/test/hello/big/world?base=query&this=is&a=test
```
Query Parameter Usage
```csharp
var builder = new UrlBuilder("https://mydomain.com")
  .AddQuery("multiparam", "one", "two")
  .AddQuery("multiparam", "three")
  .AddQuery("queryWithoutValue");


Uri url = builder.Build(); // https://mydomain.com?multiparam=one&multiparam=two&multiparam=three&queryWithoutValue
```
Path Variables can also be used to later replace the variable you need. This is especially useful for [`ImmutableUrlBuilder`s](#immutable-url-builder).
```csharp
var builder = new UrlBuilder("https://mydomain.com")
  .AddPath("{Greet}")
  .AddPath("world");

int i = 5;
if(i % 2 == 0)
{
  builder = builder.WithPathValue("Greet", "hi");
}
else
{
  builder = builder.WithPathValue("Greet", "hello");
}

Uri url = builder.Build(); // https://mydomain.com/hello/world
```

### Immutable URL builder
While the above used `UrlBuilder` is efficient, it has the problem that the internal state may be modified, e.g. when setting a path.
```csharp
var builder = new UrlBuilder("https://mydomain.com")

var builder2 = builder.AddPath("pathfor2")

var url1 = builder.Build(); // https://mydomain.com/pathfor2
var url2 = builder2.Build(); // https://mydomain.com/pathfor2
```
This is especially annoying when the builder is initialized in the constructor and then further mutated in methods, e.g.

```csharp
public MyClient
{
  private UrlBuilder _urlBuilder;
  public MyClient(IOptions<MyClientOptions> options)
  {
    _urlBuilder = new UrlBuilder(options.Value.Url)
    .SetPort(8080)
    .AddPath("orders");
  }


  public Order GetOrder(string orderId)
  {
    var url = _urlBuilder.AddPath(orderId).Build(); // ❌ may have multiple order ID paths when method called multiple times
    // GET order ...
  }

  public List<Order> GetOrders()
  {
    var url = _urlBuilder.Build(); // ❌ may have one or more order IDs added as path when GetOrder was called before
    // GET order ...
  }
}
```
For these cases, the `ImmutableUrlBuilder` can be used.
```csharp
var builder = new ImmutableUrlBuilder("https://mydomain.com")

var builder2 = builder.AddPath("pathfor2")

var url1 = builder.Build(); // https://mydomain.com
var url2 = builder2.Build(); // https://mydomain.com/pathfor2
```
You also can switch from a normal `UrlBuilder` to the `ImmutableUrlBuilder` using the `AsImmutable()`-method.
Using above `MyClient`-example, you may use this pattern:

```csharp
public MyClient
{
  private ImmutableUrlBuilder _urlBuilder;
  public MyClient(IOptions<MyClientOptions> options)
  {
    _urlBuilder = new UrlBuilder(options.Value.Url) // e.g. https://my-domain.com
    .SetPort(8080)
    .AddPath("orders")
    .AsImmutable();
  }


  public Order GetOrder(string orderId)
  {
    var url = _urlBuilder.AddPath(orderId).Build(); // URL will always be https://my-domain.com/orders/<orderId>
    // GET order ...
  }

  public List<Order> GetOrders()
  {
    var url = _urlBuilder.Build(); // URL will always be https://my-domain.com/orders
    // GET order ...
  }
}
```

