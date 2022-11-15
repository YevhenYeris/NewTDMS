using Microsoft.EntityFrameworkCore;
using NewTDBMS.gRPC.Services;
using NewTDBMS.LocalAdapter;
using NewTDBMS.RelationalAdapter;
using NewTDBMS.Service;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
//builder.Services.AddScoped<IDBContext, RelationalDBContext>();
builder.Services.AddScoped<IDBContext, LocalDBContext>();
builder.Services.AddScoped<ITDBMSService, TDBMSService>();
//builder.Services.AddDbContext<TDBMSContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NewTDMS")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ColumnService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
