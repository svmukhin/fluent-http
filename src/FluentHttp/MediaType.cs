// SPDX-FileCopyrightText: Copyright (c) [2025] [Sergei Mukhin]
// SPDX-License-Identifier: MIT

namespace FluentHttp;

/// <summary>
/// Common media types for HTTP content.
/// </summary>
public static class MediaType
{
    /// <summary>JSON media type - application/json.</summary>
    public const string APPLICATION_JSON = "application/json";
    /// <summary>XML media type - application/xml.</summary>
    public const string APPLICATION_XML = "application/xml";
    /// <summary>URL-encoded form data media type - application/x-www-form-urlencoded.</summary>
    public const string APPLICATION_FORM_URLENCODED = "application/x-www-form-urlencoded";
    /// <summary>HTML media type - text/html.</summary>
    public const string TEXT_HTML = "text/html";
    /// <summary>Plain text media type - text/plain.</summary>
    public const string TEXT_PLAIN = "text/plain";
    /// <summary>XML text media type - text/xml.</summary>
    public const string TEXT_XML = "text/xml";
    /// <summary>Multipart form data media type - multipart/form-data.</summary>
    public const string MULTIPART_FORM_DATA = "multipart/form-data";
}
