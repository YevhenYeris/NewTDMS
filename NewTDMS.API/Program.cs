using NewTDBMS.LocalAdapter;
using NewTDBMS.Service;
using NewTDBMS.RelationalAdapter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewTDBMS.API.Hateoas.Services;
using Grpc.Net.Client;
using NewTDBMS.API;

//using var channel = GrpcChannel.ForAddress("https://localhost:7159");
//var client = new Greeter.GreeterClient(channel);
//var reply = await client.SayHelloAsync(
//                  new HelloRequest { Name = "GreeterClient" });

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDBContext, LocalDBContext>();
//builder.Services.AddScoped<IDBContext, RelationalDBContext>();
builder.Services.AddScoped<ITDBMSService, TDBMSService>();
builder.Services.AddScoped<DBLinkGetter>();
builder.Services.AddScoped<TableLinkGetter>();
builder.Services.AddScoped<ColumnLinkGetter>();
builder.Services.AddScoped<RowLinkGetter>();

builder.Services.AddDbContext<TDBMSContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NewTDMS")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
