using System.Net;
using FluentHttp.Tests.Wire.Mocks;
using FluentHttp.Wire;

namespace FluentHttp.Tests.Wire;

[TestClass]
public class BasicAuthWireTests
{
    [TestMethod]
    public async Task SendAsync_ShouldAddAuthorizationHeader()
    {
        var receivedHeaders = new Dictionary<string, string>();
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            receivedHeaders = headers;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var response = await new BasicAuthWire(
            mockWire,
            "Bearer test-token").SendAsync("GET", "https://api.example.com", []);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsTrue(receivedHeaders.ContainsKey(HttpHeaders.AUTHORIZATION));
        Assert.AreEqual("Bearer test-token", receivedHeaders[HttpHeaders.AUTHORIZATION]);
    }

    [TestMethod]
    public async Task SendAsync_ShouldNotModifyOriginalHeaders()
    {
        var mockWire = new MockWire((method, uri, headers, body) =>
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });
        var headers = new Dictionary<string, string>
        {
            ["X-Custom"] = "Value"
        };
        var originalCount = headers.Count;
        await new BasicAuthWire(
            mockWire,
            "Bearer test-token").SendAsync("GET", "https://api.example.com", headers);
        Assert.AreEqual(originalCount, headers.Count);
        Assert.IsFalse(headers.ContainsKey(HttpHeaders.AUTHORIZATION));
    }

    [TestMethod]
    public async Task SendAsync_ShouldPreserveExistingHeaders()
    {
        var handler = new MockHttpMessageHandler((request) =>
        {
            Assert.IsTrue(request.Headers.Contains("X-Custom"));
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var response = await new BasicAuthWire(
            new HttpWire(new HttpClient(handler)),
            "Bearer test-token").SendAsync("GET", "https://api.example.com", new Dictionary<string, string>
            {
                ["X-Custom"] = "Value"
            });
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task SendAsync_ShouldPassThroughBody()
    {
        var handler = new MockHttpMessageHandler(async (request) =>
        {
            var body = await request.Content!.ReadAsStringAsync();
            Assert.AreEqual("{\"data\":\"test\"}", body);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var response = await new BasicAuthWire(
            new HttpWire(new HttpClient(handler)),
            "Bearer test-token").SendAsync("POST", "https://api.example.com", [], "{\"data\":\"test\"}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
