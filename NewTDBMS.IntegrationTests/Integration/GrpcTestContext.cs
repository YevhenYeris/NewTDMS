using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace NewTDBMS.Tests.Integration;

public class GrpcTestContext<TStartup> where TStartup : class
{
    private readonly Stopwatch _stopwatch;
    private readonly GrpcTestFixture<TStartup> _fixture;
    private readonly ITestOutputHelper _outputHelper;

    public GrpcTestContext(GrpcTestFixture<TStartup> fixture, ITestOutputHelper outputHelper)
    {
        _stopwatch = Stopwatch.StartNew();
        _fixture = fixture;
        _outputHelper = outputHelper;
    }
}
