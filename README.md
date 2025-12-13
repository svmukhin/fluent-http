# FluentHttp

A fluent HTTP client library for .NET that provides a pure OOP
approach to building and executing HTTP requests with method chaining.

## Installation

```bash
dotnet add package FluentHttp
```

## Quick Start

### Simple GET Request

```csharp
var response = new Request("https://api.example.com")
    .Uri().Path("/users").QueryParam("id", 123).Back()
    .Method(Request.GET)
    .Fetch();
```

### JSON Response

```csharp
string userName = new Request("https://api.example.com")
    .Uri().Path("/users").QueryParam("id", 123).Back()
    .Header(HttpHeaders.ACCEPT, MediaType.APPLICATION_JSON)
    .Fetch()
    .As<JsonResponse>()
    .AssertStatus(200)
    .Json().GetJsonObject().GetString("name");
```

### XML Response with XPath

```csharp
string href = new Request("https://api.example.com")
    .Uri().Path("/data").Back()
    .Header(HttpHeaders.ACCEPT, MediaType.TEXT_XML)
    .Fetch()
    .As<XmlResponse>()
    .AssertStatus(200)
    .AssertXPath("/page/links/link[@rel='see']")
    .EvaluateXPath("/page/links/link[@rel='see']/@href");
```

### Complex Chaining Example

```csharp
string name = new Request("https://www.example.com:8080")
    .Uri().Path("/users").QueryParam("id", 333).Back()
    .Method(Request.GET)
    .Header(HttpHeaders.ACCEPT, MediaType.TEXT_XML)
    .Fetch()
    .As<RestResponse>()
    .AssertStatus(200)
    .As<XmlResponse>()
    .AssertXPath("/page/links/link[@rel='see']")
    .Rel("/page/links/link[@rel='see']/@href")
    .Header(HttpHeaders.ACCEPT, MediaType.APPLICATION_JSON)
    .Fetch()
    .As<JsonResponse>()
    .Json().GetJsonObject().GetString("name");
```

## Building

```bash
dotnet build
```

## Testing

```bash
dotnet test
```

## License

See [LICENSE.txt](LICENSE.txt) for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
