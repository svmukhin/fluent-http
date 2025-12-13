// SPDX-FileCopyrightText: Copyright (c) [2025] [Sergei Mukhin]
// SPDX-License-Identifier: MIT

namespace FluentHttp;

/// <summary>
/// Represents an HTTP response with methods for inspection and follow-up requests.
/// </summary>
public interface IResponse
{
    /// <summary>
    /// Gets the HTTP status code of the response.
    /// </summary>
    int StatusCode { get; }

    /// <summary>
    /// Gets the response body content as a string.
    /// </summary>
    string Content { get; }

    /// <summary>
    /// Converts this response to a specific response type for specialized processing (e.g., JSON, XML).
    /// </summary>
    /// <typeparam name="T">The target response type that implements IResponse.</typeparam>
    /// <returns>The response converted to the specified type.</returns>
    T As<T>() where T : IResponse;

    /// <summary>
    /// Asserts that the response has the expected status code.
    /// </summary>
    /// <param name="expectedStatus">The expected HTTP status code.</param>
    /// <returns>The current response instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the actual status code doesn't match the expected value.</exception>
    IResponse AssertStatus(int expectedStatus);

    /// <summary>
    /// Retrieves the value of a specific response header.
    /// </summary>
    /// <param name="name">The header name to retrieve.</param>
    /// <returns>The header value, or null if the header doesn't exist.</returns>
    string? GetHeader(string name);

    /// <summary>
    /// Creates a new request with the specified header, using the same base URI as the current response.
    /// </summary>
    /// <param name="name">The header name to add to the new request.</param>
    /// <param name="value">The header value to add to the new request.</param>
    /// <returns>A new request instance with the specified header.</returns>
    IRequest Header(string name, string value);

    /// <summary>
    /// Creates a new request following a relative URI from the response.
    /// Useful for navigating HATEOAS links.
    /// </summary>
    /// <param name="href">The relative URI to follow.</param>
    /// <returns>A new request instance for the specified URI.</returns>
    IRequest Rel(string href);
}
