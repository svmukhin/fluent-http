// SPDX-FileCopyrightText: Copyright (c) [2025-2026] [Sergei Mukhin]
// SPDX-License-Identifier: MIT

using SergeiM.Http.Wire;

namespace SergeiM.Http.Tests.Wire;

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
