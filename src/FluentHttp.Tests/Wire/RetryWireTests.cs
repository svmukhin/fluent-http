// SPDX-FileCopyrightText: Copyright (c) [2025] [Sergei Mukhin]
// SPDX-License-Identifier: MIT

using System.Net;
using FluentHttp.Tests.Wire.Mocks;
using FluentHttp.Wire;

namespace FluentHttp.Tests.Wire;

[TestClass]
public class RetryWireTests
{
    [TestMethod]
    public async Task SendAsync_ShouldReturnOnFirstSuccess()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var response = await new RetryWire(
            mockWire,
            3,
            TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(1, attemptCount);
    }

    [TestMethod]
    public async Task SendAsync_ShouldRetryOnServerError()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            if (attemptCount < 3)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var response = await new RetryWire(
            mockWire,
            3,
            TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(3, attemptCount);
    }

    [TestMethod]
    public async Task SendAsync_ShouldRetryOnTooManyRequests()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            if (attemptCount < 2)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.TooManyRequests));
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var response = await new RetryWire(
            mockWire,
            3,
            TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(2, attemptCount);
    }

    [TestMethod]
    public async Task SendAsync_ShouldNotRetryOnClientError()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
        });
        var response = await new RetryWire(
            mockWire,
            3,
            TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.AreEqual(1, attemptCount);
    }

    [TestMethod]
    public async Task SendAsync_ShouldRespectMaxRetries()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        });
        var response = await new RetryWire(
            mockWire,
            2,
            TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.AreEqual(3, attemptCount); // Initial + 2 retries
    }

    [TestMethod]
    public async Task SendAsync_ShouldRetryOnHttpRequestException()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            if (attemptCount < 3)
            {
                throw new HttpRequestException("Network error");
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var response = await new RetryWire(
            mockWire,
            3,
            TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(3, attemptCount);
    }

    [TestMethod]
    public async Task SendAsync_ShouldThrowAfterMaxRetriesOnException()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            throw new HttpRequestException("Network error");
        });
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await new RetryWire(
                mockWire,
                2,
                TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        });
        Assert.AreEqual(3, attemptCount);
    }

    [TestMethod]
    public async Task SendAsync_ShouldUseCustomRetryLogic()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            if (attemptCount < 2)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var response = await new RetryWire(
            mockWire,
            3,
            TimeSpan.FromMilliseconds(10),
            (response) => response.StatusCode == HttpStatusCode.NotFound).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(2, attemptCount);
    }

    [TestMethod]
    public void Constructor_ShouldUseDefaultSettings()
    {
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var retryWire = new RetryWire(mockWire);
        Assert.IsNotNull(retryWire);
    }

    [TestMethod]
    public async Task SendAsync_ShouldRetryOnTaskCanceledException()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            if (attemptCount < 2)
            {
                throw new TaskCanceledException("Request timeout");
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var response = await new RetryWire(
            mockWire,
            3,
            TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(2, attemptCount);
    }

    [TestMethod]
    public async Task SendAsync_ShouldRetryOnTimeoutException()
    {
        var attemptCount = 0;
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            attemptCount++;
            if (attemptCount < 2)
            {
                throw new TimeoutException("Request timeout");
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var response = await new RetryWire(
            mockWire,
            3,
            TimeSpan.FromMilliseconds(10)).SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(2, attemptCount);
    }
}
