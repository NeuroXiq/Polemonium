using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Polemonium.Api.Web.Application;
using Polemonium.Api.Web.Common;
using Polemonium.Api.Web.Domain.Repositories;
using Polemonium.Api.Web.Domain.Services;
using Polemonium.Api.Web.Infrastructure.Repositories;
using Polemonium.Api.Web.Infrastructure.Shared;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Program
{
    static class Program
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
            app.UseSetupAuthToken();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSetCurrentUser();
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
            var poptions = new PolemoniumOptions();
            builder.Configuration.GetSection("Polemonium").Bind(poptions);

            // external services
            builder.Services.AddControllers();

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        //ValidateIssuer = true,
                        //ValidateAudience = true,
                        //ValidateLifetime = true,
                        //ValidateIssuerSigningKey = true,
                        ValidIssuer = "your_issuer",
                        ValidAudience = "your_audience",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(poptions.JwtKey))
                    };
                });

            // app services
            builder.Services.AddSingleton<IPAuthhentication, PAuthentication>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICurrentUser, CurrentUser>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWebsiteHostRepository, WebsiteHostRepository>();
            builder.Services.AddScoped<IHostService, HostService>();
            builder.Services.AddSingleton<IPolemoniumInfrastructure>(sp =>
            {
                return new PolemoniumInfrastructure(sp.GetRequiredService<IOptions<PolemoniumOptions>>().Value.DbConnectionString);
            });

            builder.Services.AddOptions<PolemoniumOptions>().Bind(builder.Configuration.GetSection("Polemonium"));
        }

        public static void UseSetCurrentUser(this WebApplication builder)
        {
            builder.Use(async (context, next) =>
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    var subClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    
                    if (subClaim != null && int.TryParse(subClaim, out var userId))
                    {
                        context.RequestServices.GetRequiredService<ICurrentUser>().Set(userId);
                    }
                }

                await next(context);
            });
        }

        public static void UseSetupAuthToken(this WebApplication builder)
        {
            builder.Use(async (context, next) =>
            {
                if (!context.Request.Cookies.TryGetValue("polemonium_authtoken", out var authToken) ||
                    string.IsNullOrWhiteSpace(authToken) ||
                    !context.User.Identity.IsAuthenticated)
                {
                    var userService = context.RequestServices.GetRequiredService<IUserService>();
                    var pauthentication = context.RequestServices.GetRequiredService<IPAuthhentication>();

                    var user = userService.CreateUser();
                    var token = pauthentication.CreateJwtToken(user);

                    context.Response.Cookies.Append("polemonium_authtoken", token);
                    authToken = token;
                }

                if (!string.IsNullOrWhiteSpace(authToken))
                { 
                    context.Request.Headers.Append("Authorization", "Bearer " + authToken);
                }

                await next(context);
            });
        }
    }
}