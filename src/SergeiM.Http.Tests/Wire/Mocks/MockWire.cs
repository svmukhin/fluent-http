// SPDX-FileCopyrightText: Copyright (c) [2025] [Sergei Mukhin]
// SPDX-License-Identifier: MIT

namespace SergeiM.Http.Tests.Wire.Mocks;

public class MockWire : IWire
{
    private readonly Func<string, string, Dictionary<string, string>, string?, Task<HttpResponseMessage>> _handler;

    public MockWire(Func<string, string, Dictionary<string, string>, string?, Task<HttpResponseMessage>> handler)
    {
        _handler = handler;
    }

    public Task<HttpResponseMessage> SendAsync(string method, string uri, Dictionary<string, string> headers, string? body = null)
    {
        return _handler(method, uri, headers, body);
    }

    public HttpResponseMessage Send(string method, string uri, Dictionary<string, string> headers, string? body = null)
    {
        return SendAsync(method, uri, headers, body).GetAwaiter().GetResult();
    }
}
