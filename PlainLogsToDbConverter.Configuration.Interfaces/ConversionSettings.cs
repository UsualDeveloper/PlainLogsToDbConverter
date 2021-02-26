using System.Collections.Generic;

namespace PlainLogsToDbConverter.Configuration
{
    public class ConversionSettings
    {
        public string InputLogFilePath { get; set; }

        public string ConnectionString { get; set; }

        // TODO: consider adding multiple log tables support
        //TableDescription[] TablesInOrder { get; set; }

        public string LogTableName { get; set; }

        // TODO: replace value tuple
        public IEnumerable<(string Pattern, string Template)> LogRegexTemplates { get; set; }
    }

    public class TableDescription
    {
        public string Name { get; set; }

        public IEnumerable<string> Columns { get; set; }
    }

    public class ColumnDescription
    {
        public string Name { get; set; }

        //public ValueType ValueType { get; set; }
    }

    //internal enum ValueType
    //{
    //    String = 1,
    //    Numeric,
    //    DateTime
    //}
}