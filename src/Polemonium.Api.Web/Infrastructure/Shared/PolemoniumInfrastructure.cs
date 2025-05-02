using DbUp;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Infrastructure.Shared
{
    public interface IPolemoniumInfrastructure
    {
        string ConnectionString { get; }
        void RunMigrations();
    }

    public class PolemoniumInfrastructure : IPolemoniumInfrastructure
    {
        public string ConnectionString { get; private set; }

        public PolemoniumInfrastructure(string dbConnectionString)
        {
            ConnectionString = dbConnectionString;
        }

        public void RunMigrations()
        {
            var connectionString = this.ConnectionString;

            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithTransaction()
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                throw new Exception("failed to run migrations");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
        }
    }
}
