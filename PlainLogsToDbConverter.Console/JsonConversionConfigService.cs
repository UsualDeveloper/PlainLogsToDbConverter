using System.IO;
using Newtonsoft.Json;
using PlainLogsToDbConverter.Configuration;
using PlainLogsToDbConverter.Configuration.Interfaces;

namespace PlainLogsToDbConverter.Console
{
    internal class JsonConversionConfigService : IConversionConfigService
    {
        ConversionSettings settings = null;

        public ConversionSettings GetConfigurationSettings()
        {
            if (settings == null)
            {
                string configFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location), "log-conversion-config.json");
                string configurationString = File.ReadAllText(configFilePath);
                settings = (ConversionSettings)JsonConvert.DeserializeObject(configurationString, typeof(ConversionSettings));
            }

            return settings;
        }
    }
}
