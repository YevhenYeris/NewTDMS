using Microsoft.EntityFrameworkCore;
using NewTDBMS.gRPC.Services;
using NewTDBMS.LocalAdapter;
using NewTDBMS.RelationalAdapter;
using NewTDBMS.Service;

namespace NewTDBMS.gRPC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddGrpc();
            //builder.Services.AddScoped<IDBContext, RelationalDBContext>();
            services.AddScoped<IDBContext, LocalDBContext>();
            services.AddScoped<ITDBMSService, TDBMSService>();
            //builder.Services.AddDbContext<TDBMSContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NewTDMS")));
        }

        public void Configure(IApplicationBuilder app)
        {
            // Configure the HTTP request pipeline.
                        app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<ColumnService>();
            });
        }
    }
}
