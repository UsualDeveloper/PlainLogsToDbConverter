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
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    (context, services) =>
                    services
                    .AddSingleton<ISqlLogger, SqlLogger.SqlLogger>()
                    .AddSingleton<IFileService, FileService>()
                    .AddSingleton<IConversionConfigService, JsonConversionConfigService>()
                    .AddSingleton<LogConverter>())
                .Build();

            //Serilog.Debugging.SelfLog.Enable(m => Debug.WriteLine(m));
            //AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            var logConverter = host.Services.GetRequiredService<LogConverter>();
            logConverter.ProcessLogFile(ReportProgress);

            return;
        }

        private static void ReportProgress(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
