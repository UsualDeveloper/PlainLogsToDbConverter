using PlainLogsToDbConverter.Configuration.Interfaces;
using PlainLogsToDbConverter.SqlLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlainLogsToDbConverter.Processing
{
    public class LogConverter
    {
        private readonly ISqlLogger logger;
        private readonly IFileService fileService;
        private readonly IConversionConfigService conversionConfigService;
        Action<string> reportProgress;

        public LogConverter(ISqlLogger logger, IFileService fileService, IConversionConfigService conversionConfigService)
        {
            this.logger = logger;
            this.fileService = fileService;
            this.conversionConfigService = conversionConfigService;
        }

        public void ProcessLogFile(Action<string> reportProgress)
        {
            this.reportProgress = reportProgress;
            var settings = this.conversionConfigService.GetConfigurationSettings();

            // TODO: add support for multiline log entries
            var regexesToMatch = settings.LogRegexTemplates.Select(item => new { Pattern = new Regex(item.Pattern, RegexOptions.Singleline), Template = item.Template }).ToArray();

            ReportProgress($"Opening file: {settings.InputLogFilePath}...");

            using (var reader = fileService.OpenFileTextStream(settings.InputLogFilePath))
            {
                ReportProgress("File opened.");
                ReportProgress("Reading logs...");
                int logsRead = 0;
                int logsMatched = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    logsRead++;
                    foreach (var regex in regexesToMatch)
                    {
                        var matches = regex.Pattern.Matches(line);

                        if (matches.Any())
                        {
                            logsMatched++;
                            var fieldValues = matches[0].Groups.Cast<Group>().Select(g => (Name: g.Name, Value: g.Value));
                            var orderedValues = SortToMatchTemplate(fieldValues, regex.Template);

                            logger.AddToLog(regex.Template, orderedValues.Cast<object>().ToArray());

                            break;
                        }
                    }
                }
                ReportProgress($"Logs read (read: {logsRead}, matched: {logsMatched}).");
            }
        }

        private void ReportProgress(string message)
        {
            if (reportProgress != null)
            {
                reportProgress(message);
            }
        }

        private IEnumerable<string> SortToMatchTemplate(IEnumerable<(string Name, string Value)> fieldValues, string template)
        {
            List<string> orderedValues = new List<string>(fieldValues.Count());
            int fieldStartIndex = template.IndexOf('{');
            while (fieldStartIndex > -1 && fieldStartIndex < template.Length)
            {
                int fieldEndIndex = template.IndexOf('}', fieldStartIndex);
                if (fieldEndIndex == -1 || fieldEndIndex - fieldStartIndex == 1)
                {
                    break;
                }

                string fieldName = template.Substring(fieldStartIndex + 1, fieldEndIndex - fieldStartIndex - 1);
                orderedValues.Add(fieldValues.SingleOrDefault(item => item.Name == fieldName).Value);

                fieldStartIndex = template.IndexOf('{', fieldEndIndex);
            }

            return orderedValues;
        }

    }
}
