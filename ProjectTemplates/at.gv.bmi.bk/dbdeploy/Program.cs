using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using DbUp.Engine;
using $ext_safeprojectname$.dbdeploy.library;

namespace $safeprojectname$
{
    class Program
    {
        public static ILoggerFactory LoggerFactory;
        public static IConfigurationRoot Configuration;

        /// <summary>
        /// Default connection string name if not supplied in configuration file
        /// </summary>
        private const string _connectionStringNameDefault = @"Default";

        // https://stackoverflow.com/questions/38114761/asp-net-core-configuration-for-net-core-console-application
        // https://dasith.me/2020/06/08/database-project-conversion-to-migrations/
        static int Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Configuration = SetupJsonConfigFiles(environment)
                .SetupSecretJsonConfigFiles()
                .Build();
            /*
            LoggerFactory = new LoggerFactory()
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();
            */
            CheckPlatform();
            
            var appSettings = Configuration.GetSection("AppSettings");

            var connectionStringName = string.IsNullOrWhiteSpace(appSettings["ConnectionStringName"])
                ? _connectionStringNameDefault
                : appSettings["ConnectionStringName"];

            var connectionString = Configuration.GetConnectionString(connectionStringName);

            Console.WriteLine(@"DAEMeldestelleDBDeployer" +
                              $"Version ({typeof(IConfiguration).Assembly.GetName().Version})");

            // See how to use variables in your scripts: 
            // https://dbup.readthedocs.io/en/latest/more-info/variable-substitution/
            Console.WriteLine("\nListing variables...\n");
            var variables = LoadVariables(appSettings);

            SqlServerMigrationRunner migrationRunner = new(connectionString, null, null, variables);

            return RunMigrations(appSettings, migrationRunner);
        }

        private static void CheckPlatform()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.Linux))
            {
                Console.WriteLine("Running on Linux");
            }
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.Windows))
            {
                Console.WriteLine("Running on Windows");
            }
#if Linux
            Console.WriteLine("Really running on Linux");
#elif Windows
            Console.WriteLine("Running on Windows");
#endif

        }

        private static int RunMigrations(IConfigurationSection appSettings, SqlServerMigrationRunner migrationRunner)
        {
            // Pre deployments
            Console.WriteLine("Begin of executing pre-deployment scripts...");
            migrationRunner.RunPredeployment("PreDeployment");
            migrationRunner.RunAdditionalPredeployment(appSettings["AdditionalPreDeploymentNamespace"]);

            // Migrations
            Console.WriteLine("Begin of executing migration scripts...");
            migrationRunner.RunMigration("Migrations");
            HandleResult(migrationRunner.UpgradeResults[SqlServerMigrationRunner.ResultType.MIGRATION]);
            
            // Post deployments
            Console.WriteLine("Begin of executing post-deployment scripts...");
            migrationRunner.RunPostdeployment("PostDeployment");
            migrationRunner.RunAdditionalPostdeployment(appSettings["AdditionalPostDeploymentNamespace"]);

            return migrationRunner.GetMergedResult() ? 0 : -1;
        }
        
        private static int HandleResult(DatabaseUpgradeResult result)
        { 
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();

                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        private static ConfigurationBuilder SetupJsonConfigFiles(string environment)
        {
            ConfigurationBuilder builder = (ConfigurationBuilder) new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);
            if (environment == "Development")
            {
                builder
                    .AddJsonFile(
                        Path.Combine(AppContext.BaseDirectory,
                            string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), 
                            $"appsettings.{environment}.json"),
                        optional: true
                    );
            }
            else if (!string.IsNullOrWhiteSpace(environment))
            {
                builder
                    .AddJsonFile($"appsettings.{environment}.json", optional: false);
            }
            return builder;
        }

        
        private static Dictionary<string, string> LoadVariables(IConfigurationSection section)
        {
            Dictionary<string, string> variables = new();
            foreach (var k in section.AsEnumerable())
            {
                variables.Add(k.Key, k.Value);
                Console.WriteLine($"${k.Key}$ = \"{k.Value}\"");
            }

            return variables;
        }
    }

    static class CfgBuilderExtension
    {
        public static IConfigurationBuilder SetupSecretJsonConfigFiles(this ConfigurationBuilder builder)
        {
#if DEBUG
            return builder.AddUserSecrets<Program>();
#else
            return builder;
#endif
        }

    }
}
