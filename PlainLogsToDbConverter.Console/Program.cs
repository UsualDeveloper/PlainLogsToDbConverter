using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlainLogsToDbConverter.Configuration.Interfaces;
using PlainLogsToDbConverter.Processing;
using PlainLogsToDbConverter.SqlLogger.Interfaces;

namespace PlainLogsToDbConverter.Console
{
    partial class Program
    {
        static void Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(
                    (context, services) =>
                    services
                    .AddSingleton<ISqlLogger, SqlLogger.SqlLogger>()
                    .AddSingleton<IFileService, FileService>()
                    .AddSingleton<IConversionConfigService, ConversionConfigService>((provider) => new ConversionConfigService(args))
                    .AddSingleton<LogConverter>())
                .Build();

            //Serilog.Debugging.SelfLog.Enable(m => Debug.WriteLine(m));

            var logConverter = host.Services.GetRequiredService<LogConverter>();

            try
            {
                logConverter.ProcessLogFile(ReportProgress);

                ConsoleUtils.WriteLineInColor("Conversion completed successfully.", System.ConsoleColor.Green);
            }
            catch (System.Exception ex)
            {
                ConsoleUtils.WriteLineInColor(ex.Message, System.ConsoleColor.Red);
            }

            return;
        }

        private static void ReportProgress(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
