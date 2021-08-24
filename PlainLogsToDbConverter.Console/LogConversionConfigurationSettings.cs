using PlainLogsToDbConverter.Configuration;
using System.Collections.Generic;

namespace PlainLogsToDbConverter.Console
{
    internal class LogConversionConfigurationSettings
    {
        public string ConnectionString { get; set; }

        public string LogTableName { get; set; }

        public IEnumerable<PatternAndTemplateDefinition> Patterns { get; set; }
    }
}
