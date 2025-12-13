using FluentHttp.Wire;

namespace FluentHttp.Tests.Wire;

[TestClass]
public class HttpWireTests : WireTestBase
{
    protected override IWire CreateWire(HttpClient client)
    {
        return new HttpWire(client);
    }

    [TestMethod]
    public void Constructor_ShouldCreateWithDefaultHttpClient()
    {
        var wire = new HttpWire();
        Assert.IsNotNull(wire);
    }

    [TestMethod]
    public void Constructor_ShouldCreateWithCustomHttpClient()
    {
        var wire = new HttpWire(new HttpClient());
        Assert.IsNotNull(wire);
    }
}
