using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

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

            pp.Run();
        }
    }
}