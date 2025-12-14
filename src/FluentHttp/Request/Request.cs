// SPDX-FileCopyrightText: Copyright (c) [2025] [Sergei Mukhin]
// SPDX-License-Identifier: MIT

using FluentHttp.Response;
using FluentHttp.Wire;

namespace FluentHttp.Request;

/// <summary>
/// Fluent HTTP request builder.
/// </summary>
public class BaseRequest : IRequest
{
    /// <summary>HTTP GET method constant.</summary>
    public const string GET = "GET";
    /// <summary>HTTP POST method constant.</summary>
    public const string POST = "POST";
    /// <summary>HTTP PUT method constant.</summary>
    public const string PUT = "PUT";
    /// <summary>HTTP DELETE method constant.</summary>
    public const string DELETE = "DELETE";
    /// <summary>HTTP PATCH method constant.</summary>
    public const string PATCH = "PATCH";
    /// <summary>HTTP HEAD method constant.</summary>
    public const string HEAD = "HEAD";
    /// <summary>HTTP OPTIONS method constant.</summary>
    public const string OPTIONS = "OPTIONS";

    private readonly string _baseUri;
    private UriBuilder? _uriBuilder;
    private string _method = GET;
    private readonly Dictionary<string, string> _headers;
    private string? _body;
    private IWire _wire;

    /// <summary>
    /// Creates a new HTTP request with the specified base URI.
    /// </summary>
    public BaseRequest(string uri) : this(uri, new HttpWire())
    {
    }

    /// <summary>
    /// Creates a new HTTP request with the specified base URI and wire.
    /// </summary>
    public BaseRequest(string uri, IWire wire)
    {
        _baseUri = uri;
        _headers = [];
        _wire = wire;
    }

    /// <inheritdoc/>
    public UriBuilder Uri()
    {
        _uriBuilder ??= new UriBuilder(this, _baseUri);
        return _uriBuilder;
    }

    /// <inheritdoc/>
    public IRequest Method(string method)
    {
        _method = method;
        return this;
    }

    /// <inheritdoc/>
    public IRequest Header(string name, string value)
    {
        _headers[name] = value;
        return this;
    }

    /// <inheritdoc/>
    public IRequest Body(string body)
    {
        _body = body;
        return this;
    }

    /// <inheritdoc/>
    public IRequest Through(IWire wire)
    {
        _wire = wire;
        return this;
    }

    /// <inheritdoc/>
    public async Task<BaseResponse> FetchAsync()
    {
        var uri = _uriBuilder?.Build() ?? _baseUri;
        var response = await _wire.SendAsync(_method, uri, _headers, _body);
        return new BaseResponse(response);
    }

    /// <inheritdoc/>
    public BaseResponse Fetch()
    {
        return FetchAsync().GetAwaiter().GetResult();
    }
}
