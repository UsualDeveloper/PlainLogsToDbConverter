using PlainLogsToDbConverter.Configuration;
using PlainLogsToDbConverter.Configuration.Interfaces;
using PlainLogsToDbConverter.SqlLogger.Interfaces;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.Generic;
using System.Linq;

namespace PlainLogsToDbConverter.SqlLogger
{
    public class SqlLogger : ISqlLogger
    {
        private readonly Logger logger;

        public SqlLogger(IConversionConfigService conversionConfigService)
        {
            var settings = conversionConfigService.GetConfigurationSettings();

            IEnumerable<string> tagColumns = FindFieldNames(settings);

            var columnOptions = new Serilog.Sinks.MSSqlServer.ColumnOptions();

            columnOptions.Store.Remove(StandardColumn.Level);
            columnOptions.Store.Remove(StandardColumn.Exception);
            columnOptions.Store.Remove(StandardColumn.TimeStamp);

            columnOptions.AdditionalColumns = tagColumns.Select(c => new Serilog.Sinks.MSSqlServer.SqlColumn()
            {
                ColumnName = c,
                PropertyName = c,
                AllowNull = true,
                DataType = System.Data.SqlDbType.NVarChar
            }).ToList();

            logger = new LoggerConfiguration()
                .WriteTo
                .MSSqlServer(
                    connectionString: settings.ConnectionString,
                    sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
                    {
                        TableName = settings.LogTableName,
                        //BatchPostingLimit = 7,
                        AutoCreateSqlTable = true,
                        EagerlyEmitFirstEvent = true,

                    },
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose,
                    columnOptions: columnOptions
                 )
                .CreateLogger();
        }

        private IEnumerable<string> FindFieldNames(ConversionSettings settings)
        {
            var fieldNamesFound = new HashSet<string>();

            foreach (var templateDefinition in settings.LogRegexTemplates)
            {
                string template = templateDefinition.Template;
                int fieldStartIndex = template.IndexOf('{');
                int fieldEndIndex;
                while (fieldStartIndex >= 0 && fieldStartIndex < template.Length - 2)
                {
                    fieldEndIndex = template.IndexOf('}', fieldStartIndex + 2);
                    if (fieldEndIndex == -1)
                    {
                        break;
                    }

                    string fieldName = template.Substring(fieldStartIndex + 1, fieldEndIndex - fieldStartIndex - 1);
                    if (!fieldNamesFound.Contains(fieldName))
                    {
                        fieldNamesFound.Add(fieldName);
                    }

                    fieldStartIndex = template.IndexOf('{', fieldEndIndex);
                }
            }

            return fieldNamesFound;
        }

        public void AddToLog(string message, object[] fieldValues)
        {
            logger.Information(message, fieldValues);
        }

        public void Dispose()
        {
            if (logger != null)
            {
                Log.CloseAndFlush();
                logger.Dispose();
            }
        }

    }
}
