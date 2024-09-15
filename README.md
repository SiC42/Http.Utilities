# UrlBuilder
**This is a work in progress**
This package is provides a convenient way to build URLs for your applications.

## Usage:

```csharp
var builder = new UrlBuilder("http://mydomain.com/test?hello=world")
  .AddQueryParameter("second", "parameter")
  .AddPath("subpath")
  .AddPath("/secondSubPath");

Uri url = builder.Build(); // http://mydomain.com/test/subpath/secondSubPath?hello=world&second=parameter
```

Plans:
- Support for overriding various parts of the URL (e.g. host)
- Support for path variables and values
- Support for a reusable builder (such that you can use the same builder in multiple instances without causing side effects)
- Convenience methods for HttpClient

