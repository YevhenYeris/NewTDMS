using Microsoft.EntityFrameworkCore;
using NewTDBMS.gRPC;
using NewTDBMS.gRPC.Services;
using NewTDBMS.LocalAdapter;
using NewTDBMS.RelationalAdapter;
using NewTDBMS.Service;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run();
