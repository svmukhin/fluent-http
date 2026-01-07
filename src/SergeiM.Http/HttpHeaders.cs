// SPDX-FileCopyrightText: Copyright (c) [2025-2026] [Sergei Mukhin]
// SPDX-License-Identifier: MIT

namespace SergeiM.Http;

/// <summary>
/// Standard HTTP header names.
/// </summary>
public static class HttpHeaders
{
    /// <summary>Accept header - media types acceptable in the response.</summary>
    public const string ACCEPT = "Accept";
    /// <summary>Accept-Charset header - character sets acceptable in the response.</summary>
    public const string ACCEPT_CHARSET = "Accept-Charset";
    /// <summary>Accept-Encoding header - content encodings acceptable in the response.</summary>
    public const string ACCEPT_ENCODING = "Accept-Encoding";
    /// <summary>Accept-Language header - natural languages acceptable in the response.</summary>
    public const string ACCEPT_LANGUAGE = "Accept-Language";
    /// <summary>Authorization header - credentials for HTTP authentication.</summary>
    public const string AUTHORIZATION = "Authorization";
    /// <summary>Cache-Control header - directives for caching mechanisms.</summary>
    public const string CACHE_CONTROL = "Cache-Control";
    /// <summary>Connection header - control options for the current connection.</summary>
    public const string CONNECTION = "Connection";
    /// <summary>Content-Encoding header - encoding transformations applied to the message body.</summary>
    public const string CONTENT_ENCODING = "Content-Encoding";
    /// <summary>Content-Length header - size of the message body in bytes.</summary>
    public const string CONTENT_LENGTH = "Content-Length";
    /// <summary>Content-Type header - media type of the message body.</summary>
    public const string CONTENT_TYPE = "Content-Type";
    /// <summary>Cookie header - stored HTTP cookies.</summary>
    public const string COOKIE = "Cookie";
    /// <summary>Date header - date and time at which the message was originated.</summary>
    public const string DATE = "Date";
    /// <summary>ETag header - identifier for a specific version of a resource.</summary>
    public const string ETAG = "ETag";
    /// <summary>Host header - domain name of the server.</summary>
    public const string HOST = "Host";
    /// <summary>If-Modified-Since header - conditional request based on modification date.</summary>
    public const string IF_MODIFIED_SINCE = "If-Modified-Since";
    /// <summary>If-None-Match header - conditional request based on ETag.</summary>
    public const string IF_NONE_MATCH = "If-None-Match";
    /// <summary>Location header - URI for redirection or resource location.</summary>
    public const string LOCATION = "Location";
    /// <summary>Referer header - address of the previous web page.</summary>
    public const string REFERER = "Referer";
    /// <summary>User-Agent header - user agent string identifying the client.</summary>
    public const string USER_AGENT = "User-Agent";
}
