// SPDX-FileCopyrightText: Copyright (c) [2025] [Sergei Mukhin]
// SPDX-License-Identifier: MIT

using System.Net;
using FluentHttp.Tests.Wire.Mocks;

namespace FluentHttp.Tests.Wire;

/// <summary>
/// Base class for wire tests with common test scenarios.
/// </summary>
public abstract class WireTestBase
{
    protected abstract IWire CreateWire(HttpClient client);

    [TestMethod]
    public async Task SendAsync_ShouldSendBasicGetRequest()
    {
        var handler = new MockHttpMessageHandler((request) =>
        {
            Assert.AreEqual(HttpMethod.Get, request.Method);
            Assert.AreEqual("https://api.example.com/test", request.RequestUri?.ToString());
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Success")
            };
        });
        var response = await CreateWire(new HttpClient(handler)).SendAsync("GET", "https://api.example.com/test", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("Success", content);
    }

    [TestMethod]
    public async Task SendAsync_ShouldIncludeHeaders()
    {
        var handler = new MockHttpMessageHandler((request) =>
        {
            Assert.IsTrue(request.Headers.Contains("X-Custom-Header"));
            Assert.AreEqual("CustomValue", request.Headers.GetValues("X-Custom-Header").First());
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var response = await CreateWire(new HttpClient(handler)).SendAsync("GET", "https://api.example.com", new Dictionary<string, string>
        {
            ["X-Custom-Header"] = "CustomValue"
        });
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task SendAsync_ShouldIncludeBody()
    {
        var handler = new MockHttpMessageHandler(async (request) =>
        {
            var body = await request.Content!.ReadAsStringAsync();
            Assert.AreEqual("{\"test\":\"data\"}", body);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var response = await CreateWire(new HttpClient(handler)).SendAsync("POST", "https://api.example.com", [], "{\"test\":\"data\"}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public void Send_ShouldWorkSynchronously()
    {
        var handler = new MockHttpMessageHandler((request) =>
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var response = CreateWire(new HttpClient(handler)).Send("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
