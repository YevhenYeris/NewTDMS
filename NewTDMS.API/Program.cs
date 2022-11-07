using NewTDBMS.LocalAdapter;
using NewTDBMS.Service;
using NewTDBMS.RelationalAdapter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<IDBContext, LocalDBContext>();
builder.Services.AddScoped<IDBContext, RelationalDBContext>();
builder.Services.AddScoped<ITDBMSService, TDBMSService>();
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
