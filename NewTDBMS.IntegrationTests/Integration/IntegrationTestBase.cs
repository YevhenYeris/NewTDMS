using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using NewTDBMS.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace NewTDBMS.Tests.Integration;

public class IntegrationTestBase : IClassFixture<GrpcTestFixture<Startup>>, IDisposable
{
    private GrpcChannel? _channel;
    private GrpcTestContext<Startup>? _testContext;

    protected GrpcTestFixture<Startup> Fixture { get; set; }

    protected GrpcChannel Channel => _channel ??= CreateChannel();

    protected GrpcChannel CreateChannel()
    {
        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = Fixture.Handler
        });
    }

    public IntegrationTestBase(GrpcTestFixture<Startup> fixture, ITestOutputHelper outputHelper)
    {
        Fixture = fixture;
        _testContext = Fixture.GetTestContext(outputHelper);
    }

    public void Dispose()
    {
        _channel = null;
    }
}
