using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PlainLogsToDbConverter.Configuration;
using PlainLogsToDbConverter.Configuration.Interfaces;

namespace PlainLogsToDbConverter.Console
{
    internal class ConversionConfigService : IConversionConfigService
    {

        ConversionSettings settings = null;

        string configFilePath;

        string inputLogFilePath;

        public ConversionConfigService(string[] args)
        {
            // TODO: change to use command line parsing API
            configFilePath = args[0];
            inputLogFilePath = args[1];
        }

        public ConversionSettings GetConfigurationSettings()
        {
            if (settings == null)
            {
                ValidateParameters();
                var conversionConfig = GetConfigFromJsonFile<LogConversionConfigurationSettings>(configFilePath);

                settings = new ConversionSettings
                {
                    InputLogFilePath = inputLogFilePath,
                    ConnectionString = conversionConfig.ConnectionString,
                    LogTableName = conversionConfig.LogTableName,
                    Patterns = conversionConfig.Patterns
                };
            }

            return settings;
        }

        private T GetConfigFromJsonFile<T>(string configFilePath)
        {
            string configurationContentString = File.ReadAllText(configFilePath);
            var configObject = JsonConvert.DeserializeObject<T>(configurationContentString);
            return configObject;
        }

        private void ValidateParameters()
        {
            if (!File.Exists(inputLogFilePath))
            {
                throw new ArgumentException($"Parameter {nameof(inputLogFilePath)} points to a non-existant file.");
            }

            if (!File.Exists(configFilePath))
            {
                throw new ArgumentException($"Parameter {nameof(configFilePath)} points to a non-existant file.");
            }
        }
    }
}
