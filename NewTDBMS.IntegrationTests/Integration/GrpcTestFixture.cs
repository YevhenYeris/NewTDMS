using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace NewTDBMS.Tests.Integration;

public class GrpcTestFixture<TStartup> : IDisposable where TStartup : class
{
    private TestServer? _server;
    private IHost? _host;
    private HttpMessageHandler? _handler;
    private Action<IWebHostBuilder>? _configureWebHost;

    public GrpcTestFixture()
    {
    }

    public void ConfigureWebHost(Action<IWebHostBuilder> configure)
    {
        _configureWebHost = configure;
    }

    private void EnsureServer()
    {
        if (_host == null)
        {
            var builder = new HostBuilder()
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost
                        .UseTestServer()
                        .UseStartup<TStartup>();

                    _configureWebHost?.Invoke(webHost);
                });
            _host = builder.Start();
            _server = _host.GetTestServer();
            _handler = _server.CreateHandler();
        }
    }


    public HttpMessageHandler Handler
    {
        get
        {
            EnsureServer();
            return _handler!;
        }
    }

    public void Dispose()
    {
        _handler?.Dispose();
        _host?.Dispose();
        _server?.Dispose();
    }

    public GrpcTestContext<TStartup> GetTestContext(ITestOutputHelper outputHelper)
    {
        return new GrpcTestContext<TStartup>(this, outputHelper);
    }
}
