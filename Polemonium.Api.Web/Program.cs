using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polemonium.Api.Web.Common;
using Polemonium.Api.Web.Domain.Services;
using Polemonium.Api.Web.Infrastructure.Repositories;
using Polemonium.Api.Web.Infrastructure.Shared;
using System;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddServices(builder);

            var app = builder.Build();

            app.UseCors(c =>
            {
                c.AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowAnyOrigin();
            });
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            Setup(app);

            app.Run();
        }

        private static void Setup(WebApplication app)
        {
            app.Services.GetRequiredService<IPolemoniumInfrastructure>().RunMigrations();

            if (app.Environment.IsDevelopment())
            {
                
            }
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddScoped<IHostRepository, HostRepository>();
            builder.Services.AddScoped<IHostService, HostService>();
            builder.Services.AddSingleton<IPolemoniumInfrastructure>(sp =>
            {
                return new PolemoniumInfrastructure(sp.GetRequiredService<IOptions<PolemoniumOptions>>().Value.DbConnectionString);
            });

            builder.Services.AddOptions<PolemoniumOptions>().Bind(builder.Configuration.GetSection("Polemonium"));
        }
    }
}