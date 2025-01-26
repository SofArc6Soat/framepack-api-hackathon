using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Api
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            LoggingConfig.ConfigureSerilog();

            try
            {
                Log.Information("Starting application");

                // Realiza a conversão dos arquivos para UTF-8
                ConvertFilesToUtf8(@"C:\Caminho\Para\Sua\Solucao");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application stopped due to an unhandled exception");
                throw;
            }
            finally
            {
                Log.Information("Server shutting down");
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Integração com Serilog
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConvertFilesToUtf8(string directoryPath)
        {
            var validExtensions = new[] { ".cs", ".cshtml", ".json", ".html", ".txt", ".css", ".js", ".xml" };

            foreach (var filePath in Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories))
            {
                if (Array.Exists(validExtensions, ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    try
                    {
                        var content = File.ReadAllText(filePath, Encoding.Default);
                        File.WriteAllText(filePath, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                        Log.Information($"File converted to UTF-8: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Failed to convert file: {filePath}");
                    }
                }
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public static class LoggingConfig
    {
        public static void ConfigureSerilog()
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                    .WithDefaultDestructurers()
                    .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() }))
                .Enrich.WithCorrelationId()
                .Enrich.WithCorrelationIdHeader()
                .MinimumLevel.Debug()
                .WriteTo.Async(wt => wt.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"));

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}